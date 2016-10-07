using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace ConsoleApplication1
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{

         try
         {
            string plaintext = "Hello World";
            byte [] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(plaintext);
            MemoryStream memory = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();
            
            ICryptoTransform encrypter = aes.CreateEncryptor();
            CryptoStream cs = new CryptoStream(memory, encrypter, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            memory.Position = 0;
            StreamReader sr = new StreamReader(memory);
            string ciphertext = sr.ReadToEnd();
            sr.Close();
            cs.Close();
            memory.Close();
            Debug.WriteLine(ciphertext);
         }
         catch(System.Security.Cryptography.CryptographicException ex)
         {
            Debug.WriteLine(ex.ToString());
         }
		}
	}
}
