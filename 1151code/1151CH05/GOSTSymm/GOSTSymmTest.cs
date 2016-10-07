using System;
using System.Security.Cryptography;
using System.IO;
using GOST;

class TestGostSymmetric
{
	[STAThread]
	static void Main(string[] args)
	{
		//byte[] plain_text = new byte[]{0,1,2,3,4,5,6,7,8,9,10};
		byte[] plain_text = new Byte[512];
		System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
		byte[] strbytes = enc.GetBytes("Novodachnaja.  50 years anniversary 7890");
		Array.Copy(strbytes, 0, plain_text, 0, strbytes.Length);

		byte[] key = new Byte[32]{0,0,0,0,1,0,0,0,2,0,0,0,3,0,0,0,4,0,0,0,5,0,0,0,6,0,0,0,7,0,0,0};
		byte[] iv = new Byte[8] {0,1,2,3,4,5,6,7};

		Console.WriteLine("Plain text to encrypt:");
		PrintByteArray(plain_text);

		GOSTsymmManaged gost = new GOSTsymmManaged();
		gost.LoadTestSBoxes(2);
		gost.Key = key;
		gost.IV = iv;
		gost.Mode = CipherMode.CBC;


		GOSTsymmTransform ct_e = (GOSTsymmTransform)gost.CreateEncryptor();
		ct_e.UseExpandedSBoxes = false;
		GOSTsymmTransform ct_d = (GOSTsymmTransform)gost.CreateDecryptor();

		MemoryStream ms1 = new MemoryStream();
		MemoryStream ms2 = new MemoryStream();

		CryptoStream cs1 = new CryptoStream(ms1, ct_e, CryptoStreamMode.Write);

		cs1.Write(plain_text, 0, plain_text.Length);
		cs1.Close();

	        byte[] cipher_text = ms1.ToArray();

		Console.WriteLine("Cipher text:");
		PrintByteArray(cipher_text);

		CryptoStream cs2 = new CryptoStream(ms2, ct_d, CryptoStreamMode.Write);

		cs2.Write(cipher_text, 0, cipher_text.Length);
		cs2.Close();

		byte[] decrypted_text = ms2.ToArray();

		Console.WriteLine("DecryptedText:");
		PrintByteArray(decrypted_text);

		Console.WriteLine("Done.");
	}



	static void PrintByteArray(Byte[] arr)
	{
		if (arr==null) 
		{
			Console.WriteLine("null");
			return;
		}
		int i;
		for (i=0; i<arr.Length; i++) 
		{
			Console.Write("{0:X} ", arr[i]);
			if ( (i+9)%8 == 0 ) Console.WriteLine();
		}
		if (i%8 != 0) Console.WriteLine();
	}

}
