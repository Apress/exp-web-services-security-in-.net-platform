
/* 
  Copyright 2001-2003 Markus Hahn <markus_hahn@gmx.net>
  All rights reserved.
  See Documentation for license details.  
*/

using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;

using Blowfish_NET;


class BlowfishAlgorithmTest
{
	public static string BufToStr
		(byte[] buf)
	{
		StringBuilder result = new StringBuilder(buf.Length * 3);

		for (int nI = 0, nC = buf.Length; nI < nC; nI++)
		{
			if (0 < nI) result.Append(' ');
			result.Append(buf[nI].ToString("x2"));
		}
		return result.ToString();
	}

	static SymmetricAlgorithm MakeAlgo
	  (bool blBF)
	{
		SymmetricAlgorithm result = (blBF) ? new BlowfishAlgorithm() :
											 SymmetricAlgorithm.Create();
		result.Mode = CipherMode.CBC;
	
		if (blBF) result.KeySize = 40;
		result.GenerateKey();
		result.GenerateIV();
		// result.Padding = PaddingMode.Zeros;

		return result;
	}

	static void Main(string[] args)
	{
		long lLen;
		int nRead, nReadTotal;
		byte[] buf = new byte[3];
		byte[] encData;
		byte[] decData;
		SymmetricAlgorithm alg;
		MemoryStream sin;
		MemoryStream sout;
		CryptoStream encStream;
		CryptoStream decStream;

		// set up the algorithm (false to go with AES for comparison purposes)

		alg = MakeAlgo(true);

		// we encrypt and decrypt from and to memory stream,
		// so first we have to set up a source and a target

		int nI, nC = 11;
		sin = new MemoryStream();

		for (nI = 0; nI < nC; nI++)
		{
			sin.WriteByte((byte)nI);	

		}
		sin.Position = 0;

		sout = new MemoryStream();

		// now we create a crypto stream, to show that the
		// BlowfishAlgorithm plays together with a standard
		// .NET framework security component

		encStream = new CryptoStream(
			sout, 
			alg.CreateEncryptor(), 
			CryptoStreamMode.Write);

		lLen = sin.Length;
		nReadTotal = 0;

		while (nReadTotal < lLen)
		{
			nRead = sin.Read(buf, 0, buf.Length);

			encStream.Write(buf, 0, nRead);
			nReadTotal += nRead;
		}

		encStream.Close();  

		// show what we got as the encrypted data

		encData = sout.ToArray();
		Console.WriteLine("plain    : " + BufToStr(sin.ToArray()));			
		Console.WriteLine("encrypted: " + BufToStr(encData));

		// reset the streams and do the decryption the same way

		sin = new MemoryStream(encData);
		sout = new MemoryStream(nC + Blowfish.BLOCKSIZE); 

		decStream = new CryptoStream(
			sin, 
			alg.CreateDecryptor(), 
			CryptoStreamMode.Read);

		lLen = sin.Length;
		nReadTotal = 0;

		while (nReadTotal < lLen)
		{
			nRead = decStream.Read(buf, 0, buf.Length);
			if (0 == nRead) break;
			
			sout.Write(buf, 0, nRead);
			nReadTotal += nRead;
		}

		decStream.Close();  

		decData = sout.ToArray();		 
		Console.WriteLine("decrypted: " + BufToStr(decData));

		// (NOTE: eventually we get back padding bytes, which we ignore in our test here)
		for (nI = 0; nI < nC; nI++)
		{
			if (decData[nI] != nI) 
			{
				Console.WriteLine("decryption error");		
				break;
			}
		}

		// last stage: (almost) futile weak key test search (10k attempts)

		byte[] testKey = new byte[alg.KeySize];
		
		Random rnd = new Random((int)DateTime.Now.Ticks);

		Console.Write("searching for weak keys ");
		
		nC = 10000;
		for (nI = 0; nI < nC; nI++)
		{
			rnd.NextBytes(testKey);
			if (BlowfishAlgorithm.IsWeakKey(testKey))
			{
				Console.WriteLine("\nweak key found after " + nI + " attempt(s)");
				Console.WriteLine(BufToStr(testKey));
				break;
			}
			else
			{
				if (0 == (nI % (nC / 10))) Console.Write(".");
			}
		}
		if (nC == nI) Console.WriteLine("\nno weak key found.");		
	}
}
