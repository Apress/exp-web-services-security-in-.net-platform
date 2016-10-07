using System;
using System.Configuration;
using System.ComponentModel;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

using COMAdmin;

/// Should obfuscate this assembly and register it in the GAC for additional security.
namespace SecureConnectionString
{
   /// <summary>
   /// Supported Storages
   /// </summary>
   public enum SupportedStorageTypes{DPAPI, COMPlus, Registry};

   /// <summary>
   /// Supported Encryption Algorithyms
   /// </summary>
   public enum SupportedEncryptionAlgorythms{DES, RC2, Rijndael, TripleDES};

   /// <summary>
   /// These are the fixed keys for the SecureConnString class usage.  This is better than a config file without giving any indication of what encryption algorythm or sql authentication you are using.  Technically this also masks if you are unsing sql or not.  This will work for any providers Connection String.
   /// </summary>
   public class RegKeys
   {
      public static string SUBKEY{get{return "Software\\SecureConnString";}}
      public static string STORETYPE{get{return "StorageType";}}
      public static string CONNCRYPTTYPE{get{return "EncryptionAlgorythm";}}
      public static string CRYPTOKEY{get{return "CryptoKey";}}
      public static string REGSTORAGESUBKEY{get{return "RegStorageSubKey";}}
      public static string REGSTORAGESUBVALUE{get{return "RegStorageSubValue";}}
      public static string DPAPISTORAGESUBKEY{get{return RegKeys.SUBKEY+"\\DPAPIStorage";}}
      public static string DPAPISTORAGESUBVALUE{get{return "DPAPIStorage";}}
   }

   /// <summary>
   /// This assembly gets it configuration information from the registry (HKLM\Software\SecureConnString).  Allow diferent Symetric Encryption Algorythms with Private Key also Stored in the registry.
   /// </summary>
   public class Configuration
   {
      private Microsoft.Win32.RegistryKey regkey;

      public Configuration()
      {
         regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeys.SUBKEY, true);
         if(regkey==null)
         {
            regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(RegKeys.SUBKEY);
         }
      }

      public SupportedStorageTypes StorageType
      {
         set{regkey.SetValue(RegKeys.STORETYPE, value.ToString());}
         get{return (SupportedStorageTypes)Enum.Parse(typeof(SupportedStorageTypes), (string)regkey.GetValue(RegKeys.STORETYPE));}

      }

      public SupportedEncryptionAlgorythms EncryptionAlgorythm
      {
         set{regkey.SetValue(RegKeys.CONNCRYPTTYPE, value.ToString());}
         get{return (SupportedEncryptionAlgorythms)Enum.Parse(typeof(SupportedEncryptionAlgorythms), (string)regkey.GetValue(RegKeys.CONNCRYPTTYPE));}
      }

      public string CryptoKey
      {
         set
         {
            if(value.Length > 0 && value.Length <= 16)
            {
               regkey.SetValue(RegKeys.CRYPTOKEY, value.ToString());
            }
            else
            {
               throw new ArgumentOutOfRangeException("Crypto Key must be great than 0 and less than 17.");
            }
         }
         get{return (string)regkey.GetValue(RegKeys.CRYPTOKEY);}
      }

      public string RegistryStorageKey
      {
         set{regkey.SetValue(RegKeys.REGSTORAGESUBKEY, value.ToString());}
         get{return (string)regkey.GetValue(RegKeys.REGSTORAGESUBKEY);}
      }

      public string RegistryValueName
      {
         set{regkey.SetValue(RegKeys.REGSTORAGESUBVALUE, value.ToString());}
         get{return (string)regkey.GetValue(RegKeys.REGSTORAGESUBVALUE);}
      }
   }

   /// <summary>
   /// This assembly gets it configuration information from the registry (HKLM\Software\SecureConnString).  Allow diferent Symetric Encryption Algorythms with Private Key also Stored in the registry.
   /// </summary>
   public class SecureConnString
   {
      // Key for Symetric Encryption
      private byte[] byteKey;
      private byte[] byteIn;
      private Microsoft.Win32.RegistryKey regkey;
      private Configuration config;
      // create a MemoryStream so that the process can be done without I/O files
      private System.IO.MemoryStream ms;
      private System.IO.StreamReader sr;

      public SecureConnString()
      {
         Debug.WriteLine(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
         regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeys.SUBKEY, true);
         config = new Configuration();
      }
      
      private string encrypt(string input)
      {
         byteIn = System.Text.ASCIIEncoding.ASCII.GetBytes(input);
         ms = new System.IO.MemoryStream();

         SymmetricAlgorithm crypto = SymmetricAlgorithm.Create();

         switch(config.EncryptionAlgorythm)
         {
            case SupportedEncryptionAlgorythms.DES:
               crypto = new DESCryptoServiceProvider();
               break;
            case SupportedEncryptionAlgorythms.RC2:
               crypto = new RC2CryptoServiceProvider();
               break;
            case SupportedEncryptionAlgorythms.Rijndael:
               crypto = new RijndaelManaged();
               break;
            case SupportedEncryptionAlgorythms.TripleDES:
               crypto = new TripleDESCryptoServiceProvider();
               break;
         }

         byteKey = GetKey((string)regkey.GetValue(RegKeys.CRYPTOKEY), crypto);

         // set the private key
         crypto.Key = byteKey;
         crypto.IV = byteKey;

         // create an Encryptor from the Provider Service instance
         ICryptoTransform encrypto = crypto.CreateEncryptor();

         // create Crypto Stream that transforms a stream using the encryption
         CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

         // write out encrypted content into MemoryStream
         cs.Write(byteIn, 0, byteIn.Length);
         cs.FlushFinalBlock();
            
         // get the output and trim the '\0' bytes
         byte[] byteOut = ms.GetBuffer();
         int i = 0;
         for (i = 0; i < byteOut.Length; i++)
            if (byteOut[i] == 0)
               break;
                    
         // convert into Base64 so that the result can be used in xml
         return System.Convert.ToBase64String(byteOut, 0, i);
      }

      private string decrypt(string input)
      {
         byteIn = System.Convert.FromBase64String(input);
         ms = new System.IO.MemoryStream(byteIn, 0, byteIn.Length);

         SymmetricAlgorithm crypto = SymmetricAlgorithm.Create();

         switch(config.EncryptionAlgorythm)
         {
            case SupportedEncryptionAlgorythms.DES:
               crypto = new DESCryptoServiceProvider();
               break;
            case SupportedEncryptionAlgorythms.RC2:
               crypto = new RC2CryptoServiceProvider();
               break;
            case SupportedEncryptionAlgorythms.Rijndael:
               crypto = new RijndaelManaged();
               break;
            case SupportedEncryptionAlgorythms.TripleDES:
               crypto = new TripleDESCryptoServiceProvider();
               break;
         }

         byteKey = GetKey((string)regkey.GetValue(RegKeys.CRYPTOKEY), crypto);

         // set the private key
         crypto.Key = byteKey;
         crypto.IV = byteKey;

         // create a Decryptor from the Provider Service instance
         ICryptoTransform encrypto = crypto.CreateDecryptor();
 
         // create Crypto Stream that transforms a stream using the decryption
         CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

         // read out the result from the Crypto Stream
         sr = new System.IO.StreamReader( cs );
         return sr.ReadToEnd();
      }

      /// <remarks>
      /// Depending on the legal key size limitations of a specific CryptoService provider
      /// and length of the private key provided, padding the secret key with space character
      /// to meet the legal size of the algorithm.
      /// </remarks>
      private byte[] GetKey(string Key, SymmetricAlgorithm  mobjCryptoService)
      {
         string sTemp;
         int lessSize = 0, moreSize = mobjCryptoService.LegalKeySizes[0].MinSize;

         if (mobjCryptoService.LegalKeySizes.Length > 0)
         {
            // 8 because key sizes are in bits
            while (Key.Length * 8 > moreSize)
            {
               lessSize = moreSize;
               moreSize += mobjCryptoService.LegalKeySizes[0].SkipSize;
            }
            sTemp = Key.PadRight(moreSize / 8, ' ');
         }
         else
         {
            sTemp = Key;
         }

         // convert the secret key to byte array
         return ASCIIEncoding.ASCII.GetBytes(sTemp);       
      }

      public string ConnectionString
      {
         get
         {
            StringBuilder sb = new StringBuilder();
            TimeSpan tsStart = new TimeSpan(System.DateTime.Now.Ticks);

            ISecureConnStringStorage storage = new RegistryStorage();
      
            switch(config.StorageType)
            {
               case SupportedStorageTypes.COMPlus:
                  storage = new COMPlusStorage();
                  break;
               case SupportedStorageTypes.DPAPI:
                  storage = new DPAPIStorage();
                  break;
               case SupportedStorageTypes.Registry:
                  storage = new RegistryStorage();
                  break;
            }

            string s = storage.read();

            TimeSpan tsEnd = new TimeSpan(System.DateTime.Now.Ticks);
            tsEnd = tsEnd - tsStart;

            sb.AppendFormat( @"ConnectionString(){0}Timespan:  {1} ms{0}",
               Environment.NewLine, 
               tsEnd.TotalMilliseconds);
            Debug.WriteLine(sb.ToString());

            return decrypt(s);
         }
         set
         {
            StringBuilder sb = new StringBuilder();
            TimeSpan tsStart = new TimeSpan(System.DateTime.Now.Ticks);

            ISecureConnStringStorage storage = new RegistryStorage();
      
            switch(config.StorageType)
            {
               case SupportedStorageTypes.COMPlus:
                  storage = new COMPlusStorage();
                  break;
               case SupportedStorageTypes.DPAPI:
                  storage = new DPAPIStorage();
                  break;
               case SupportedStorageTypes.Registry:
                  storage = new RegistryStorage();
                  break;
            }

            storage.write(encrypt(value));

            TimeSpan tsEnd = new TimeSpan(System.DateTime.Now.Ticks);
            tsEnd = tsEnd - tsStart;

            sb.AppendFormat( @"ConnectionString(){0}Timespan:  {1} ms{0}",
               Environment.NewLine, 
               tsEnd.TotalMilliseconds);
            Debug.WriteLine(sb.ToString());
         }
      }
   }

   public interface ISecureConnStringStorage
   {
      string read();
      void write(string s);
   }

   /// <summary>
   /// This assumes that your are storing the connection string in DPAPI.  Destination is configurable.
   /// </summary>
   public class DPAPIStorage : ISecureConnStringStorage
   {
      #region DLL Imports

      /// <summary>
      /// Import of CryptoAPIs CryptProtectData. Parameters ripped from the SDK.
      /// </summary>
      [DllImport("Crypt32.dll", SetLastError=true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
      private static extern bool CryptProtectData(
         ref DATA_BLOB pDataIn, 
         String szDataDescr, 
         ref DATA_BLOB pOptionalEntropy,
         IntPtr pvReserved, 
         ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct, 
         int dwFlags, 
         ref DATA_BLOB pDataOut);

      /// <summary>
      /// Import of CryptoAPIs CryptUnprotectData. Parameters ripped from the SDK.
      /// </summary>
      [DllImport("Crypt32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
      private static extern bool CryptUnprotectData(
         ref DATA_BLOB pDataIn, 
         String szDataDescr, 
         ref DATA_BLOB pOptionalEntropy, 
         IntPtr pvReserved, 
         ref CRYPTPROTECT_PROMPTSTRUCT pPromptStruct, 
         int dwFlags, 
         ref DATA_BLOB pDataOut);

      #endregion

      #region Structure Definitions and Constraints
      [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
         internal struct DATA_BLOB
      {
         public int cbData;
         public IntPtr pbData;
      }

      [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
         internal struct CRYPTPROTECT_PROMPTSTRUCT
      {
         public int cbSize;
         public int dwPromptFlags;
         public IntPtr hwndApp;
         public String szPrompt;
      }

      private const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
      private const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;
      #endregion

      private string _entropy;
      private Microsoft.Win32.RegistryKey regkey;

      public DPAPIStorage()
      {
         _entropy = "My Secret Entropy String";

         // Open a reg key to store the Cypher text this allows a user to not have to worry about keeping the text around.
         regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeys.DPAPISTORAGESUBKEY, true);
         if(regkey==null)
         {
            regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(RegKeys.DPAPISTORAGESUBKEY);
         }
      }
      
      public string read()
      {
         //Get data from registry
         string regstring = (string)regkey.GetValue(RegKeys.DPAPISTORAGESUBVALUE);;

         bool retVal = false;
         DATA_BLOB plainTextBlob = new DATA_BLOB();
         DATA_BLOB cipherBlob = new DATA_BLOB();
         CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
         InitPromptstruct(ref prompt);

         byte[] cipherText = Convert.FromBase64String(regstring);
         byte[] entropyText = System.Text.Encoding.Unicode.GetBytes(_entropy);

         try
         {
            try
            {
               int cipherTextSize = cipherText.Length;
               cipherBlob.pbData = Marshal.AllocHGlobal(cipherTextSize);
               if (cipherBlob.pbData == IntPtr.Zero)
               {
                  throw new OutOfMemoryException("Unable to allocate cipherText buffer.");
               }
               cipherBlob.cbData = cipherTextSize;	
               Marshal.Copy(cipherText, 0, cipherBlob.pbData, cipherBlob.cbData);
            }
            catch (Exception ex)
            {
               throw new Exception("Exception marshalling data.", ex);
            }
            DATA_BLOB entropyBlob = new DATA_BLOB();
            
            // MachineStore
            int dwFlags = CRYPTPROTECT_LOCAL_MACHINE | CRYPTPROTECT_UI_FORBIDDEN;
					
            try
            {
               int bytesSize = entropyText.Length;
               entropyBlob.pbData = Marshal.AllocHGlobal(bytesSize);
               if (entropyBlob.pbData == IntPtr.Zero)
               {
                  throw new OutOfMemoryException("Unable to allocate entropy buffer.");
               }
               entropyBlob.cbData = bytesSize;
               Marshal.Copy(entropyText, 0, entropyBlob.pbData, bytesSize);
            }
            catch (Exception ex)
            {
               throw new Exception("Exception entropy marshalling data. ",	ex);
            }

            retVal = CryptUnprotectData(ref cipherBlob, null, ref entropyBlob, IntPtr.Zero, ref prompt, dwFlags, ref plainTextBlob);

            if (!retVal)
            {
               Win32Exception w32e = new Win32Exception(Marshal.GetLastWin32Error());
               throw new ApplicationException("Decryption failed. " + w32e.Message);
            }
				
            //Free the blob and entropy.
            if (cipherBlob.pbData != IntPtr.Zero)
            {
               Marshal.FreeHGlobal(cipherBlob.pbData);
            }
            if (entropyBlob.pbData != IntPtr.Zero)
            {
               Marshal.FreeHGlobal(entropyBlob.pbData);
            }
         }
         catch (Exception ex)
         {
            throw new Exception("Exception decrypting.", ex);
         }
         byte[] plainText = new byte[plainTextBlob.cbData];
         Marshal.Copy(plainTextBlob.pbData, plainText, 0, plainTextBlob.cbData);
         
         return  Encoding.Unicode.GetString(plainText);
      }

      
      public void write(string connString)
      {
         bool retVal = false;

         DATA_BLOB plainTextBlob = new DATA_BLOB();
         DATA_BLOB cipherTextBlob = new DATA_BLOB();
         DATA_BLOB entropyBlob = new DATA_BLOB();

         CRYPTPROTECT_PROMPTSTRUCT prompt = new CRYPTPROTECT_PROMPTSTRUCT();
         InitPromptstruct(ref prompt);

         byte[] plainText = System.Text.Encoding.Unicode.GetBytes(connString);
         byte[] entropyText = System.Text.Encoding.Unicode.GetBytes(_entropy);

         int dwFlags;
         try
         {
            try
            {
               try
               {
                  int bytesSize = plainText.Length;
                  plainTextBlob.pbData = Marshal.AllocHGlobal(bytesSize);
                  if (plainTextBlob.pbData == IntPtr.Zero)
                  {
                     throw new OutOfMemoryException("Unable to allocate memory for buffer.");
                  }
                  plainTextBlob.cbData = bytesSize;
                  Marshal.Copy(plainText, 0, plainTextBlob.pbData, bytesSize);
               }
               catch (Exception ex)
               {
                  throw new Exception("Exception marshalling data.", ex);
               }

               // MachineStore
               dwFlags = CRYPTPROTECT_LOCAL_MACHINE | CRYPTPROTECT_UI_FORBIDDEN;
					
               try
               {
                  int bytesSize = entropyText.Length;
                  entropyBlob.pbData = Marshal.AllocHGlobal(bytesSize);
                  if (entropyBlob.pbData == IntPtr.Zero)
                  {
                     throw new OutOfMemoryException("Unable to allocate entropy data buffer.");
                  }
                  Marshal.Copy(entropyText, 0, entropyBlob.pbData, bytesSize);
                  entropyBlob.cbData = bytesSize;

                  retVal = CryptProtectData(ref plainTextBlob, null, ref entropyBlob, IntPtr.Zero, ref prompt, dwFlags, ref cipherTextBlob);
                  if (!retVal)
                  {
                     Win32Exception w32e = new Win32Exception(Marshal.GetLastWin32Error());
                     throw new ApplicationException("Encryption failed. " + w32e.Message);
                  }
               }
               catch (Exception ex)
               {
                  throw new Exception("Exception entropy marshalling data. ", ex);
               }
            }
            catch (Exception ex)
            {
               throw new Exception("Exception marshalling data.", ex);
            }

            byte[] cipherText = new byte[cipherTextBlob.cbData];
            Marshal.Copy(cipherTextBlob.pbData, cipherText, 0, cipherTextBlob.cbData);
            regkey.SetValue( RegKeys.DPAPISTORAGESUBVALUE , Convert.ToBase64String(cipherText));
         }
         catch (Exception ex)
         {
            throw new Exception("Exception storing data.", ex);
         }
      }

      private void InitPromptstruct(ref CRYPTPROTECT_PROMPTSTRUCT ps) 
      {
         ps.cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT));
         ps.dwPromptFlags = 0;
         ps.hwndApp = IntPtr.Zero;
         ps.szPrompt = null;
      }
   }

   /// <summary>
   /// This assumes that your are storing the connection string in a COM+ .  Destination is configurable.
   /// </summary>
   [ConstructionEnabled(Enabled = true, Default="Server=localhost;Database=Northwind;uid=someone;pwd=somewhere")]
   public class COMPlusStorage : ServicedComponent, ISecureConnStringStorage
   {
      private string m_connString;

      override protected void Construct(string s)
      {
         m_connString = s;
      }

      public COMPlusStorage()
      {
      }

      public string read()
      {
         return m_connString;
      }

      public void write(string connString)
      {
         try
         {
            // Get all the Applications
            COMAdminCatalog cat = new COMAdminCatalog();
            ICatalogCollection colA = cat.GetCollection("Applications") as ICatalogCollection;
            colA.Populate();

            // Find the component
            foreach(ICatalogObject co in colA)
            {
               if((string)co.Name == "SecureConnectionString")
               {
                  ICatalogCollection colC = colA.GetCollection("Components", co.Key) as ICatalogCollection;
                  colC.Populate();

                  foreach(ICatalogObject co2 in colC)
                  {
                     if((string)co2.Name == "SecureConnectionString.COMPlusStorage")
                     {
                        co2.set_Value("ConstructorString", connString);
                        break;
                     }
                  }
                  colC.SaveChanges();
                  break;
               }
            }

            colA.SaveChanges();
         }
         catch(Exception ex)
         {
            Debug.WriteLine(ex.ToString());
         }
      }
   }

   /// <summary>
   /// This assumes that your are storing the connection string in the registry.  Destination is configurable.
   /// </summary>
	public class RegistryStorage : ISecureConnStringStorage
	{
      private Microsoft.Win32.RegistryKey configregkey;
      private Microsoft.Win32.RegistryKey regkey;

      public RegistryStorage()
      {
         configregkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(RegKeys.SUBKEY);

         regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey((string)configregkey.GetValue(RegKeys.REGSTORAGESUBKEY), true);
         if(regkey==null)
         {
            regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey((string)configregkey.GetValue(RegKeys.REGSTORAGESUBKEY));
         }
      }

      public string read()
      {
         return (string)regkey.GetValue((string)configregkey.GetValue(RegKeys.REGSTORAGESUBVALUE));
      }

      public void write(string connString)
      {
         regkey.SetValue( (string)configregkey.GetValue(RegKeys.REGSTORAGESUBVALUE) , connString);
      }
   }
}
