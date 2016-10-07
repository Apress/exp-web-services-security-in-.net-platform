
/* 
  Copyright 2001-2003 Markus Hahn <markus_hahn@gmx.net>
  All rights reserved.
  See Documentation for license details.  
*/


using System;
using System.Text;

using Blowfish_NET;


/// <summary>
///   some Blowfish test class
/// </summary>
class BlowfishTest
{

	static byte[] StringToBlocks
		(String sText)
	{
		int nI, nR = sText.Length % Blowfish.BLOCKSIZE;
		if (0 != nR) 
		{
			for (nI = 0; nI < (Blowfish.BLOCKSIZE - nR); nI++) sText += '.';
		}

		System.Console.WriteLine(sText);
    
		ASCIIEncoding ascEnc = new ASCIIEncoding();

		return ascEnc.GetBytes(sText);
	}

	static String BlocksToString
		(byte[] data)
	{
		ASCIIEncoding ascEnc = new ASCIIEncoding();

		return ascEnc.GetString(data);
	}



	const int BIGBUFDIM = 100000;
	const int TESTLOOPS = 200;
  

	static void TestBlowfish()
	{
		if (!Blowfish.SelfTest())
		{
			System.Console.WriteLine("selftest failed.");
			return;
		}

		System.Console.WriteLine("selftest passed.");

		byte[] key = new byte[16];

		for (byte bI = 0; bI < key.Length; bI++) key[bI] = bI; 

		Blowfish bf = new Blowfish(key);

		System.Console.WriteLine((bf.IsWeakKey) ? "weak key detected." :
                                                  "no weak key.");

		String sTest = "this is something to encrypt";

		System.Console.WriteLine(sTest);
    
		byte[] plainText = StringToBlocks(sTest);

		byte[] cipherText = new byte[plainText.Length];

		bf.Encrypt(plainText, cipherText, 0, 0, plainText.Length);

		System.Console.WriteLine(BlocksToString(cipherText));

		bf.Decrypt(cipherText, cipherText, 0, 0, cipherText.Length);

		System.Console.WriteLine(BlocksToString(cipherText));     

		int nI, nSize = Blowfish.BLOCKSIZE * BIGBUFDIM;
		byte[] bigBuf = new byte[nSize];
		for (nI = 0; nI < nSize; nI++) bigBuf[nI] = (byte)nI;

		System.Console.WriteLine("benchmark running ...");     
  
		long lTm = DateTime.Now.Ticks;

		for (nI = 0; nI < TESTLOOPS; nI++)  
		{
			bf.Encrypt(bigBuf, bigBuf, 0, 0, nSize);

			if ((nI & 0x0f) == 0) System.Console.Write(".");
		}
      
		lTm = DateTime.Now.Ticks - lTm;

		lTm /= 10000;

		System.Console.WriteLine("\n{0} bytes in {1} millisecs", 
			TESTLOOPS * nSize,
			lTm);
         
		long lSize = (long)nSize * 1000 * TESTLOOPS;  
		lSize /= lTm;
    
		System.Console.WriteLine("(average of {0} bytes per second)", lSize);

		bf.Burn();
	}


	// (taken from the Counterpane website at
	//  http://www.counterpane.com/vectors.txt
	//  triplets of key, plain, cipher)
	static readonly ulong[] TEST_VECTORS = 
	{
		0x0000000000000000, 0x0000000000000000, 0x4EF997456198DD78,
		0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF, 0x51866FD5B85ECB8A,
		0x3000000000000000, 0x1000000000000001, 0x7D856F9A613063F2,
		0x1111111111111111, 0x1111111111111111, 0x2466DD878B963C9D,
		0x0123456789ABCDEF, 0x1111111111111111, 0x61F9C3802281B096,
		0x1111111111111111, 0x0123456789ABCDEF, 0x7D0CC630AFDA1EC7,
		0x0000000000000000, 0x0000000000000000, 0x4EF997456198DD78,
		0xFEDCBA9876543210, 0x0123456789ABCDEF, 0x0ACEAB0FC6A0A28D,
		0x7CA110454A1A6E57, 0x01A1D6D039776742, 0x59C68245EB05282B,
		0x0131D9619DC1376E, 0x5CD54CA83DEF57DA, 0xB1B8CC0B250F09A0,
		0x07A1133E4A0B2686, 0x0248D43806F67172, 0x1730E5778BEA1DA4,
		0x3849674C2602319E, 0x51454B582DDF440A, 0xA25E7856CF2651EB,
		0x04B915BA43FEB5B6, 0x42FD443059577FA2, 0x353882B109CE8F1A,
		0x0113B970FD34F2CE, 0x059B5E0851CF143A, 0x48F4D0884C379918,
		0x0170F175468FB5E6, 0x0756D8E0774761D2, 0x432193B78951FC98,
		0x43297FAD38E373FE, 0x762514B829BF486A, 0x13F04154D69D1AE5,
		0x07A7137045DA2A16, 0x3BDD119049372802, 0x2EEDDA93FFD39C79,
		0x04689104C2FD3B2F, 0x26955F6835AF609A, 0xD887E0393C2DA6E3,
		0x37D06BB516CB7546, 0x164D5E404F275232, 0x5F99D04F5B163969,
		0x1F08260D1AC2465E, 0x6B056E18759F5CCA, 0x4A057A3B24D3977B,
		0x584023641ABA6176, 0x004BD6EF09176062, 0x452031C1E4FADA8E,
		0x025816164629B007, 0x480D39006EE762F2, 0x7555AE39F59B87BD,
		0x49793EBC79B3258F, 0x437540C8698F3CFA, 0x53C55F9CB49FC019,
		0x4FB05E1515AB73A7, 0x072D43A077075292, 0x7A8E7BFA937E89A3,
		0x49E95D6D4CA229BF, 0x02FE55778117F12A, 0xCF9C5D7A4986ADB5,
		0x018310DC409B26D6, 0x1D9D5C5018F728C2, 0xD1ABB290658BC778,
		0x1C587F1C13924FEF, 0x305532286D6F295A, 0x55CB3774D13EF201,
		0x0101010101010101, 0x0123456789ABCDEF, 0xFA34EC4847B268B2,
		0x1F1F1F1F0E0E0E0E, 0x0123456789ABCDEF, 0xA790795108EA3CAE,
		0xE0FEE0FEF1FEF1FE, 0x0123456789ABCDEF, 0xC39E072D9FAC631D,
		0x0000000000000000, 0xFFFFFFFFFFFFFFFF, 0x014933E0CDAFF6E4,
		0xFFFFFFFFFFFFFFFF, 0x0000000000000000, 0xF21E9A77B71C49BC,
		0x0123456789ABCDEF, 0x0000000000000000, 0x245946885754369A,
		0xFEDCBA9876543210, 0xFFFFFFFFFFFFFFFF, 0x6B5C5A9C5D9E0A5A
	};

	static void TestBlowfishVectors()
	{
		int nI = 0, nJ;
		while (nI < TEST_VECTORS.Length)
		{
			byte[] key = new byte[8];		ulong ulKey = TEST_VECTORS[nI++];
			byte[] plain = new byte[8];		ulong ulPlain = TEST_VECTORS[nI++];
			byte[] cipher = new byte[8];	ulong ulCipher = TEST_VECTORS[nI++];

			for (nJ = 7; nJ >= 0; nJ--)
			{
				key[nJ] = (byte)(ulKey & 0x0ff);		ulKey >>= 8; 	
				plain[nJ] = (byte)(ulPlain & 0x0ff);	ulPlain >>= 8; 	
				cipher[nJ] = (byte)(ulCipher & 0x0ff);	ulCipher >>= 8; 	
			}
			
			byte[] testBuf = new byte[8];

			Blowfish bf = new Blowfish(key);

			bf.Encrypt(plain, testBuf, 0, 0, 8);

			for (nJ = 0; nJ < 8; nJ++)
			{
				if (testBuf[nJ] != cipher[nJ])
				{
					Console.WriteLine("error on vector #{0}", nI / 3);
				}
			}
		}
		Console.WriteLine("all tests passed");
	}

	public static void TestBlowfishCBC()
	{
		String sDemo = "The Blowfish encryption algorithm was introduced in 1994.";

		System.Console.WriteLine(sDemo);

		byte[] ptext = StringToBlocks(sDemo);

		byte[] key = new byte[16];

		byte bI;
		for (bI = 0; bI < key.Length; bI++) key[bI] = bI; 

		byte[] iv = new byte[Blowfish.BLOCKSIZE];
		for (bI = 0; bI < iv.Length; bI++) iv[bI] = (byte)bI; 

		BlowfishCBC bfc = new BlowfishCBC(key, iv);

		byte[] ctext = new byte[ptext.Length];
 
		bfc.Encrypt(ptext, ctext, 0, 0, ptext.Length);

		System.Console.WriteLine(BlocksToString(ctext));

		bfc.Iv = iv;

		bfc.Decrypt(ctext, ctext, 0, 0, ctext.Length);
		
		bfc.Burn();
  
		System.Console.WriteLine(BlocksToString(ctext));
	}

	public static void TestBlowfishSimple()
	{
		String sKey = "psst, don't tell";

		BlowfishSimple bfs = new BlowfishSimple(sKey);

		String sKeyChecksum = bfs.KeyChecksum; 

		System.Console.WriteLine("the key checksum is \"{0}\"", sKeyChecksum);

		String sStr = "Hello, please make that a secret.";

		String sEnc = bfs.Encrypt(sStr);

		System.Console.WriteLine("<<<{0}>>>", sStr);
		System.Console.WriteLine(sEnc);

		bfs.Burn();
		bfs = null;

		System.Console.WriteLine(
			(BlowfishSimple.VerifyKey("hubba!", sKeyChecksum)) ? "?!?" : "as expected");

		System.Console.WriteLine(
			(BlowfishSimple.VerifyKey(sKey, sKeyChecksum)) ? "as expected" : "?!?");

		bfs = new BlowfishSimple(sKey);

		String sDec = bfs.Decrypt(sEnc);
    
		System.Console.WriteLine("<<<{0}>>>", sDec);

		bfs.Burn();
	}

	/// <summary>
	///   application entry point
	/// </summary>
	public static void Main()
	{
		TestBlowfish();
		TestBlowfishVectors();
		TestBlowfishCBC();
		TestBlowfishSimple();
	}
}




