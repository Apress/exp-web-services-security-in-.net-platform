using System;
using SecureConnectionString;

namespace Configurator
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// he one type Initialization for the SecureConnString class.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
         // Validate the user is an administrator

         // Set up the global stuff
         Configuration config = new Configuration();
         SecureConnString scs = new SecureConnString();

         try
         {            
            // Example 1: Using the Registry
            config.EncryptionAlgorythm = SecureConnectionString.SupportedEncryptionAlgorythms.RC2;
            config.StorageType = SecureConnectionString.SupportedStorageTypes.Registry;
            config.CryptoKey = "a-1w.4i7]dz`4kdf";
            config.RegistryStorageKey = "Software\\APress\\SecureConnString\\RegistryExample";
            config.RegistryValueName = "ConnectionString";
            // Note have to set up the config stuff before assigning the ConnectionString
            scs.ConnectionString = "Server=MySqlServer;Database=MySqlDatabase;Integrated Security=SSPI";
            Console.WriteLine(scs.ConnectionString);
            
            // Example 2: Using COM+
            config = new Configuration();
            config.EncryptionAlgorythm = SecureConnectionString.SupportedEncryptionAlgorythms.Rijndael;
            config.StorageType = SecureConnectionString.SupportedStorageTypes.COMPlus;
            config.CryptoKey = "a-1w.4i7]dz`4kdf";
            // Note have to set up the config stuff before assigning the ConnectionString
            scs.ConnectionString = "Server=localhost;Database=Northwind;uid=someone;pwd=somewhere";
            Console.WriteLine(scs.ConnectionString);


            // Example 3: Using DPAPI
            config = new Configuration();
            config.EncryptionAlgorythm = SecureConnectionString.SupportedEncryptionAlgorythms.TripleDES;
            config.StorageType = SecureConnectionString.SupportedStorageTypes.DPAPI;
            config.CryptoKey = "a-1w.4i7]dz`4kdf";
            // Note have to set up the config stuff before assigning the ConnectionString
            scs.ConnectionString = "Server=localhost;Database=Northwind;uid=someone;pwd=somewhere";
            
            Console.WriteLine(scs.ConnectionString);
         }
         catch(Exception ex)
         {
            Console.WriteLine(ex.ToString());
         }
      }
	}
}
