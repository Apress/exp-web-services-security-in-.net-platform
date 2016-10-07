// $Id: $
//
// $Log: $
// Revision 1.0  1998/03/24  raif
// + start of history.
//
// $Endlog$
/*
* Copyright (c) 1997, 1998 Systemics Ltd on behalf of
* the Cryptix Development Team. All rights reserved.
*/
namespace Twofish
{
	using System;
	
	//...........................................................................
	/// <summary> Twofish is an AES candidate algorithm. It is a balanced 128-bit Feistel
	/// cipher, consisting of 16 rounds. In each round, a 64-bit S-box value is
	/// computed from 64 bits of the block, and this value is xored into the other
	/// half of the block. The two half-blocks are then exchanged, and the next
	/// round begins. Before the first round, all input bits are xored with key-
	/// dependent "whitening" subkeys, and after the final round the output bits
	/// are xored with other key-dependent whitening subkeys; these subkeys are
	/// not used anywhere else in the algorithm.<p>
	/// *
	/// Twofish was submitted by Bruce Schneier, Doug Whiting, John Kelsey, Chris
	/// Hall and David Wagner.<p>
	/// *
	/// Reference:<ol>
	/// <li>TWOFISH2.C -- Optimized C API calls for TWOFISH AES submission,
	/// Version 1.00, April 1998, by Doug Whiting.</ol><p>
	/// *
	/// <b>Copyright</b> &copy; 1998
	/// <a href="http://www.systemics.com/">Systemics Ltd</a> on behalf of the
	/// <a href="http://www.systemics.com/docs/cryptix/">Cryptix Development Team</a>.
	/// <br>All rights reserved.<p>
	/// *
	/// <b>$Revision: $</b>
	/// </summary>
	/// <author>  Raif S. Naffah
	/// 
	/// </author>
	public sealed class Twofish_Algorithm
	{
		public Twofish_Algorithm()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			TRACE = Twofish_Properties.isTraceable(NAME);
		}
		static Twofish_Algorithm()
		{
			{
				long time = (System.DateTime.Now.Ticks - 621355968000000000) / 10000;
				
				if (DEBUG && debuglevel > 6)
				{
					System.Console.Out.WriteLine("Algorithm Name: " + Twofish_Properties.FULL_NAME);
					System.Console.Out.WriteLine("Electronic Codebook (ECB) Mode");
					System.Console.Out.WriteLine();
				}
				//
				// precompute the MDS matrix
				//
            MDS[0]=new int[256];
            MDS[1]=new int[256];
            MDS[2]=new int[256];
            MDS[3]=new int[256];
				int[] m1 = new int[2];
				int[] mX = new int[2];
				int[] mY = new int[2];
				int i, j;
				 for (i = 0; i < 256; i++)
				{
					j = P[0][i] & 0xFF; // compute all the matrix elements
					m1[0] = j;
					mX[0] = Mx_X(j) & 0xFF;
					mY[0] = Mx_Y(j) & 0xFF;
					
					j = P[1][i] & 0xFF;
					m1[1] = j;
					mX[1] = Mx_X(j) & 0xFF;
					mY[1] = Mx_Y(j) & 0xFF;
					
					MDS[0][i] = m1[P_00] << 0 | mX[P_00] << 8 | mY[P_00] << 16 | mY[P_00] << 24;
					MDS[1][i] = mY[P_10] << 0 | mY[P_10] << 8 | mX[P_10] << 16 | m1[P_10] << 24;
					MDS[2][i] = mX[P_20] << 0 | mY[P_20] << 8 | m1[P_20] << 16 | mY[P_20] << 24;
					MDS[3][i] = mX[P_30] << 0 | m1[P_30] << 8 | mY[P_30] << 16 | mX[P_30] << 24;
				}
				
				time = (System.DateTime.Now.Ticks - 621355968000000000) / 10000 - time;
				
				if (DEBUG && debuglevel > 8)
				{
					System.Console.Out.WriteLine("==========");
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("Static Data");
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("MDS[0][]:");
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(MDS[0][i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("MDS[1][]:");
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(MDS[1][i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("MDS[2][]:");
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(MDS[2][i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("MDS[3][]:");
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(MDS[3][i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("Total initialization time: " + time + " ms.");
					System.Console.Out.WriteLine();
				}
			}
			 for (int i2 = 0; i2 < 4; i2++)
			{
				MDS[i2] = new int[256];
			}
		}
		// implicit no-argument constructor
		// Debugging methods and variables
		//...........................................................................
		
		internal const System.String NAME = "Twofish_Algorithm";
		internal const bool IN = true;
		internal const bool OUT = false;
		
		internal static bool DEBUG = Twofish_Properties.GLOBAL_DEBUG;
		internal static int debuglevel = DEBUG?Twofish_Properties.getLevel(NAME):0;
		internal static System.IO.StreamWriter err = DEBUG?Twofish_Properties.Output:null;
		internal static bool TRACE;
		
		internal static void  debug(System.String s)
		{
			err.WriteLine(">>> " + NAME + ": " + s);
		}
		internal static void  trace(bool in_Renamed, System.String s)
		{
			if (TRACE)
				err.WriteLine((in_Renamed?"==> ":"<== ") + NAME + "." + s);
		}
		internal static void  trace(System.String s)
		{
			if (TRACE)
				err.WriteLine("<=> " + NAME + "." + s);
		}
		
		
		// Constants and variables
		//...........................................................................
		
		internal const int BLOCK_SIZE = 16; // bytes in a data-block
		private const int ROUNDS = 16;
		private const int MAX_ROUNDS = 16; // max # rounds (for allocating subkeys)
		
		/* Subkey array indices */
		private const int INPUT_WHITEN = 0;
		private static int OUTPUT_WHITEN = INPUT_WHITEN + BLOCK_SIZE / 4;
		private static int ROUND_SUBKEYS = OUTPUT_WHITEN + BLOCK_SIZE / 4;
		// 2*(# rounds)
		
		private static int TOTAL_SUBKEYS = ROUND_SUBKEYS + 2 * MAX_ROUNDS;
		
		private const int SK_STEP = 0x02020202;
		private const int SK_BUMP = 0x01010101;
		private const int SK_ROTL = 9;
		
		/// <summary>Fixed 8x8 permutation S-boxes 
		/// </summary>
		private static byte[][] P = {new byte[]{(byte) 0xA9, (byte) 0x67, (byte) 0xB3, (byte) 0xE8, (byte) 0x04, (byte) 0xFD, (byte) 0xA3, (byte) 0x76, (byte) 0x9A, (byte) 0x92, (byte) 0x80, (byte) 0x78, (byte) 0xE4, (byte) 0xDD, (byte) 0xD1, (byte) 0x38, (byte) 0x0D, (byte) 0xC6, (byte) 0x35, (byte) 0x98, (byte) 0x18, (byte) 0xF7, (byte) 0xEC, (byte) 0x6C, (byte) 0x43, (byte) 0x75, (byte) 0x37, (byte) 0x26, (byte) 0xFA, (byte) 0x13, (byte) 0x94, (byte) 0x48, (byte) 0xF2, (byte) 0xD0, (byte) 0x8B, (byte) 0x30, (byte) 0x84, (byte) 0x54, (byte) 0xDF, (byte) 0x23, (byte) 0x19, (byte) 0x5B, (byte) 0x3D, (byte) 0x59, (byte) 0xF3, (byte) 0xAE, (byte) 0xA2, (byte) 0x82, (byte) 0x63, (byte) 0x01, (byte) 0x83, (byte) 0x2E, (byte) 0xD9, (byte) 0x51, (byte) 0x9B, (byte) 0x7C, (byte) 0xA6, (byte) 0xEB, (byte) 0xA5, (byte) 0xBE, (byte) 0x16, (byte) 0x0C, (byte) 0xE3, (byte) 0x61, (byte) 0xC0, (byte) 0x8C, (byte) 0x3A, (byte) 0xF5, (byte) 0x73, (byte) 0x2C, (byte) 0x25, (byte) 0x0B, (byte) 0xBB, (byte) 0x4E, (byte) 0x89, (byte) 0x6B, (byte) 0x53, (byte) 0x6A, (byte) 0xB4, (byte) 0xF1, (byte) 0xE1, (byte) 0xE6, (byte) 0xBD, (byte) 0x45, (byte) 0xE2, (byte) 0xF4, (byte) 0xB6, (byte) 0x66, (byte) 0xCC, (byte) 0x95, (byte) 0x03, (byte) 0x56, (byte) 0xD4, (byte) 0x1C, (byte) 0x1E, (byte) 0xD7, (byte) 0xFB, (byte) 0xC3, (byte) 0x8E, (byte) 0xB5, (byte) 0xE9, (byte) 0xCF, (byte) 0xBF, (byte) 0xBA, (byte) 0xEA, (byte) 0x77, (byte) 0x39, (byte) 0xAF, (byte) 0x33, (byte) 0xC9, (byte) 0x62, (byte) 0x71, (byte) 0x81, (byte) 0x79, (byte) 0x09, (byte) 0xAD, (byte) 0x24, (byte) 0xCD, (byte) 0xF9, (byte) 0xD8, (byte) 0xE5, (byte) 0xC5, (byte) 0xB9, (byte) 0x4D, (byte) 0x44, (byte) 0x08, (byte) 0x86, (byte) 0xE7, (byte) 0xA1, (byte) 0x1D, (byte) 0xAA, (byte) 0xED, (byte) 0x06, (byte) 0x70, (byte) 0xB2, (byte) 0xD2, (byte) 0x41, (byte) 0x7B, (byte) 0xA0, (byte) 0x11, (byte) 0x31, (byte) 0xC2, (byte
			) 0x27, (byte) 0x90, (byte) 0x20, (byte) 0xF6, (byte) 0x60, (byte) 0xFF, (byte) 0x96, (byte) 0x5C, (byte) 0xB1, (byte) 0xAB, (byte) 0x9E, (byte) 0x9C, (byte) 0x52, (byte) 0x1B, (byte) 0x5F, (byte) 0x93, (byte) 0x0A, (byte) 0xEF, (byte) 0x91, (byte) 0x85, (byte) 0x49, (byte) 0xEE, (byte) 0x2D, (byte) 0x4F, (byte) 0x8F, (byte) 0x3B, (byte) 0x47, (byte) 0x87, (byte) 0x6D, (byte) 0x46, (byte) 0xD6, (byte) 0x3E, (byte) 0x69, (byte) 0x64, (byte) 0x2A, (byte) 0xCE, (byte) 0xCB, (byte) 0x2F, (byte) 0xFC, (byte) 0x97, (byte) 0x05, (byte) 0x7A, (byte) 0xAC, (byte) 0x7F, (byte) 0xD5, (byte) 0x1A, (byte) 0x4B, (byte) 0x0E, (byte) 0xA7, (byte) 0x5A, (byte) 0x28, (byte) 0x14, (byte) 0x3F, (byte) 0x29, (byte) 0x88, (byte) 0x3C, (byte) 0x4C, (byte) 0x02, (byte) 0xB8, (byte) 0xDA, (byte) 0xB0, (byte) 0x17, (byte) 0x55, (byte) 0x1F, (byte) 0x8A, (byte) 0x7D, (byte) 0x57, (byte) 0xC7, (byte) 0x8D, (byte) 0x74, (byte) 0xB7, (byte) 0xC4, (byte) 0x9F, (byte) 0x72, (byte) 0x7E, (byte) 0x15, (byte) 0x22, (byte) 0x12, (byte) 0x58, (byte) 0x07, (byte) 0x99, (byte) 0x34, (byte) 0x6E, (byte) 0x50, (byte) 0xDE, (byte) 0x68, (byte) 0x65, (byte) 0xBC, (byte) 0xDB, (byte) 0xF8, (byte) 0xC8, (byte) 0xA8, (byte) 0x2B, (byte) 0x40, (byte) 0xDC, (byte) 0xFE, (byte) 0x32, (byte) 0xA4, (byte) 0xCA, (byte) 0x10, (byte) 0x21, (byte) 0xF0, (byte) 0xD3, (byte) 0x5D, (byte) 0x0F, (byte) 0x00, (byte) 0x6F, (byte) 0x9D, (byte) 0x36, (byte) 0x42, (byte) 0x4A, (byte) 0x5E, (byte) 0xC1, (byte) 0xE0}, new byte[]{(byte) 0x75, (byte) 0xF3, (byte) 0xC6, (byte) 0xF4, (byte) 0xDB, (byte) 0x7B, (byte) 0xFB, (byte) 0xC8, (byte) 0x4A, (byte) 0xD3, (byte) 0xE6, (byte) 0x6B, (byte) 0x45, (byte) 0x7D, (byte) 0xE8, (byte) 0x4B, (byte) 0xD6, (byte) 0x32, (byte) 0xD8, (byte) 0xFD, (byte) 0x37, (byte) 0x71, (byte) 0xF1, (byte) 0xE1, (byte) 0x30, (byte) 0x0F, (byte) 0xF8, (byte) 0x1B, (byte) 0x87, (byte) 0xFA, (byte) 
			0x06, (byte) 0x3F, (byte) 0x5E, (byte) 0xBA, (byte) 0xAE, (byte) 0x5B, (byte) 0x8A, (byte) 0x00, (byte) 0xBC, (byte) 0x9D, (byte) 0x6D, (byte) 0xC1, (byte) 0xB1, (byte) 0x0E, (byte) 0x80, (byte) 0x5D, (byte) 0xD2, (byte) 0xD5, (byte) 0xA0, (byte) 0x84, (byte) 0x07, (byte) 0x14, (byte) 0xB5, (byte) 0x90, (byte) 0x2C, (byte) 0xA3, (byte) 0xB2, (byte) 0x73, (byte) 0x4C, (byte) 0x54, (byte) 0x92, (byte) 0x74, (byte) 0x36, (byte) 0x51, (byte) 0x38, (byte) 0xB0, (byte) 0xBD, (byte) 0x5A, (byte) 0xFC, (byte) 0x60, (byte) 0x62, (byte) 0x96, (byte) 0x6C, (byte) 0x42, (byte) 0xF7, (byte) 0x10, (byte) 0x7C, (byte) 0x28, (byte) 0x27, (byte) 0x8C, (byte) 0x13, (byte) 0x95, (byte) 0x9C, (byte) 0xC7, (byte) 0x24, (byte) 0x46, (byte) 0x3B, (byte) 0x70, (byte) 0xCA, (byte) 0xE3, (byte) 0x85, (byte) 0xCB, (byte) 0x11, (byte) 0xD0, (byte) 0x93, (byte) 0xB8, (byte) 0xA6, (byte) 0x83, (byte) 0x20, (byte) 0xFF, (byte) 0x9F, (byte) 0x77, (byte) 0xC3, (byte) 0xCC, (byte) 0x03, (byte) 0x6F, (byte) 0x08, (byte) 0xBF, (byte) 0x40, (byte) 0xE7, (byte) 0x2B, (byte) 0xE2, (byte) 0x79, (byte) 0x0C, (byte) 0xAA, (byte) 0x82, (byte) 0x41, (byte) 0x3A, (byte) 0xEA, (byte) 0xB9, (byte) 0xE4, (byte) 0x9A, (byte) 0xA4, (byte) 0x97, (byte) 0x7E, (byte) 0xDA, (byte) 0x7A, (byte) 0x17, (byte) 0x66, (byte) 0x94, (byte) 0xA1, (byte) 0x1D, (byte) 0x3D, (byte) 0xF0, (byte) 0xDE, (byte) 0xB3, (byte) 0x0B, (byte) 0x72, (byte) 0xA7, (byte) 0x1C, (byte) 0xEF, (byte) 0xD1, (byte) 0x53, (byte) 0x3E, (byte) 0x8F, (byte) 0x33, (byte) 0x26, (byte) 0x5F, (byte) 0xEC, (byte) 0x76, (byte) 0x2A, (byte) 0x49, (byte) 0x81, (byte) 0x88, (byte) 0xEE, (byte) 0x21, (byte) 0xC4, (byte) 0x1A, (byte) 0xEB, (byte) 0xD9, (byte) 0xC5, (byte) 0x39, (byte) 0x99, (byte) 0xCD, (byte) 0xAD, (byte) 0x31, (byte) 0x8B, (byte) 0x01, (byte) 0x18, (byte) 0x23, (byte) 0xDD, (byte) 0x1F, (byte) 0x4E, (byte) 0x2D, (byte) 0xF9, (byte) 
			0x48, (byte) 0x4F, (byte) 0xF2, (byte) 0x65, (byte) 0x8E, (byte) 0x78, (byte) 0x5C, (byte) 0x58, (byte) 0x19, (byte) 0x8D, (byte) 0xE5, (byte) 0x98, (byte) 0x57, (byte) 0x67, (byte) 0x7F, (byte) 0x05, (byte) 0x64, (byte) 0xAF, (byte) 0x63, (byte) 0xB6, (byte) 0xFE, (byte) 0xF5, (byte) 0xB7, (byte) 0x3C, (byte) 0xA5, (byte) 0xCE, (byte) 0xE9, (byte) 0x68, (byte) 0x44, (byte) 0xE0, (byte) 0x4D, (byte) 0x43, (byte) 0x69, (byte) 0x29, (byte) 0x2E, (byte) 0xAC, (byte) 0x15, (byte) 0x59, (byte) 0xA8, (byte) 0x0A, (byte) 0x9E, (byte) 0x6E, (byte) 0x47, (byte) 0xDF, (byte) 0x34, (byte) 0x35, (byte) 0x6A, (byte) 0xCF, (byte) 0xDC, (byte) 0x22, (byte) 0xC9, (byte) 0xC0, (byte) 0x9B, (byte) 0x89, (byte) 0xD4, (byte) 0xED, (byte) 0xAB, (byte) 0x12, (byte) 0xA2, (byte) 0x0D, (byte) 0x52, (byte) 0xBB, (byte) 0x02, (byte) 0x2F, (byte) 0xA9, (byte) 0xD7, (byte) 0x61, (byte) 0x1E, (byte) 0xB4, (byte) 0x50, (byte) 0x04, (byte) 0xF6, (byte) 0xC2, (byte) 0x16, (byte) 0x25, (byte) 0x86, (byte) 0x56, (byte) 0x55, (byte) 0x09, (byte) 0xBE, (byte) 0x91}};
		
		/// <summary> Define the fixed p0/p1 permutations used in keyed S-box lookup.
		/// By changing the following constant definitions, the S-boxes will
		/// automatically get changed in the Twofish engine.
		/// </summary>
		private const int P_00 = 1;
		private const int P_01 = 0;
		private const int P_02 = 0;
		private static int P_03 = P_01 ^ 1;
		private const int P_04 = 1;
		
		private const int P_10 = 0;
		private const int P_11 = 0;
		private const int P_12 = 1;
		private static int P_13 = P_11 ^ 1;
		private const int P_14 = 0;
		
		private const int P_20 = 1;
		private const int P_21 = 1;
		private const int P_22 = 0;
		private static int P_23 = P_21 ^ 1;
		private const int P_24 = 0;
		
		private const int P_30 = 0;
		private const int P_31 = 1;
		private const int P_32 = 1;
		private static int P_33 = P_31 ^ 1;
		private const int P_34 = 1;
		
		/// <summary>Primitive polynomial for GF(256) 
		/// </summary>
		private const int GF256_FDBK = 0x169;
		private static int GF256_FDBK_2 = 0x169 / 2;
		private static int GF256_FDBK_4 = 0x169 / 4;
		
		/// <summary>MDS matrix 
		/// </summary>
      private static int[][] MDS = new int[4][];
      //private static int[,] MDS = new int[4,256];
		// blank final
		
		private const int RS_GF_FDBK = 0x14D; // field generator
		
		/// <summary>data for hexadecimal visualisation. 
		/// </summary>
		private static char[] HEX_DIGITS = new char[]{'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};
		
		
		// Static code - to intialise the MDS matrix
		//...........................................................................
		
		
		private static int LFSR1(int x)
		{
			return (x >> 1) ^ ((x & 0x01) != 0?GF256_FDBK_2:0);
		}
		
		private static int LFSR2(int x)
		{
			return (x >> 2) ^ ((x & 0x02) != 0?GF256_FDBK_2:0) ^ ((x & 0x01) != 0?GF256_FDBK_4:0);
		}
		
		private static int Mx_1(int x)
		{
			return x;
		}
		private static int Mx_X(int x)
		{
			return x ^ LFSR2(x);
		}
		// 5B
		private static int Mx_Y(int x)
		{
			return x ^ LFSR1(x) ^ LFSR2(x);
		}
		// EF
		
		
		// Basic API methods
		//...........................................................................
		
		/// <summary> Expand a user-supplied key material into a session key.
		/// *
		/// </summary>
		/// <param name="key"> The 64/128/192/256-bit user-key to use.
		/// </param>
		/// <returns> This cipher's round keys.
		/// </returns>
		/// <exception cref=""> ApplicationException  If the key is invalid.
		/// 
		/// </exception>
		public static System.Object makeKey(byte[] k)
		{
			lock(typeof(Twofish.Twofish_Algorithm))
			{
				if (DEBUG)
					trace(IN, "makeKey(" + k + ")");
				if (k == null)
				{
					//UPGRADE_TODO: Constructor java.security.ApplicationException.ApplicationException was not converted. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1095"'
					throw new ApplicationException("Empty key");
				}
				int length = k.Length;
				if (!(length == 8 || length == 16 || length == 24 || length == 32))
				{
					//UPGRADE_TODO: Constructor java.security.ApplicationException.ApplicationException was not converted. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1095"'
					throw new ApplicationException("Incorrect key length");
				}
				
				if (DEBUG && debuglevel > 7)
				{
					System.Console.Out.WriteLine("Intermediate Session Key Values");
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("Raw=" + toString(k));
					System.Console.Out.WriteLine();
				}
				int k64Cnt = length / 8;
				int subkeyCnt = ROUND_SUBKEYS + 2 * ROUNDS;
				int[] k32e = new int[4]; // even 32-bit entities
				int[] k32o = new int[4]; // odd 32-bit entities
				int[] sBoxKey = new int[4];
				//
				// split user key material into even and odd 32-bit entities and
				// compute S-box keys using (12, 8) Reed-Solomon code over GF(256)
				//
				int i, j, offset = 0;
				 for (i = 0, j = k64Cnt - 1; i < 4 && offset < length; i++, j--)
				{
					k32e[i] = (k[offset++] & 0xFF) | (k[offset++] & 0xFF) << 8 | (k[offset++] & 0xFF) << 16 | (k[offset++] & 0xFF) << 24;
					k32o[i] = (k[offset++] & 0xFF) | (k[offset++] & 0xFF) << 8 | (k[offset++] & 0xFF) << 16 | (k[offset++] & 0xFF) << 24;
					sBoxKey[j] = RS_MDS_Encode(k32e[i], k32o[i]); // reverse order
				}
				// compute the round decryption subkeys for PHT. these same subkeys
				// will be used in encryption but will be applied in reverse order.
				int q, A, B;
				int[] subKeys = new int[subkeyCnt];
				 for (i = q = 0; i < subkeyCnt / 2; i++, q += SK_STEP)
				{
					A = F32(k64Cnt, q, k32e); // A uses even key entities
					B = F32(k64Cnt, q + SK_BUMP, k32o); // B uses odd  key entities
					B = B << 8 | URShift(B, 24);
					A += B;
					subKeys[2 * i] = A; // combine with a PHT
					A += B;
					subKeys[2 * i + 1] = A << SK_ROTL | URShift(A, (32 - SK_ROTL));
				}
				//
				// fully expand the table for speed
				//
				int k0 = sBoxKey[0];
				int k1 = sBoxKey[1];
				int k2 = sBoxKey[2];
				int k3 = sBoxKey[3];
				int b0, b1, b2, b3;
				int[] sBox = new int[4 * 256];
				 for (i = 0; i < 256; i++)
				{
					b0 = b1 = b2 = b3 = i;
					switch (k64Cnt & 3)
					{
						case 1: 
							sBox[2 * i] = MDS[0][(P[P_01][b0] & 0xFF) ^ Twofish_Algorithm.b0(k0)];
							sBox[2 * i + 1] = MDS[1][(P[P_11][b1] & 0xFF) ^ Twofish_Algorithm.b1(k0)];
							sBox[0x200 + 2 * i] = MDS[2][(P[P_21][b2] & 0xFF) ^ Twofish_Algorithm.b2(k0)];
							sBox[0x200 + 2 * i + 1] = MDS[3][(P[P_31][b3] & 0xFF) ^ Twofish_Algorithm.b3(k0)];
							break;
						
						case 0: 
							// same as 4
							b0 = (P[P_04][b0] & 0xFF) ^ Twofish_Algorithm.b0(k3);
							b1 = (P[P_14][b1] & 0xFF) ^ Twofish_Algorithm.b1(k3);
							b2 = (P[P_24][b2] & 0xFF) ^ Twofish_Algorithm.b2(k3);
							b3 = (P[P_34][b3] & 0xFF) ^ Twofish_Algorithm.b3(k3);
							goto case 3;
						
						case 3: 
							b0 = (P[P_03][b0] & 0xFF) ^ Twofish_Algorithm.b0(k2);
							b1 = (P[P_13][b1] & 0xFF) ^ Twofish_Algorithm.b1(k2);
							b2 = (P[P_23][b2] & 0xFF) ^ Twofish_Algorithm.b2(k2);
							b3 = (P[P_33][b3] & 0xFF) ^ Twofish_Algorithm.b3(k2);
							goto case 2;
						
						case 2: 
							// 128-bit keys
							sBox[2 * i] = MDS[0][(P[P_01][(P[P_02][b0] & 0xFF) ^ Twofish_Algorithm.b0(k1)] & 0xFF) ^ Twofish_Algorithm.b0(k0)];
							sBox[2 * i + 1] = MDS[1][(P[P_11][(P[P_12][b1] & 0xFF) ^ Twofish_Algorithm.b1(k1)] & 0xFF) ^ Twofish_Algorithm.b1(k0)];
							sBox[0x200 + 2 * i] = MDS[2][(P[P_21][(P[P_22][b2] & 0xFF) ^ Twofish_Algorithm.b2(k1)] & 0xFF) ^ Twofish_Algorithm.b2(k0)];
							sBox[0x200 + 2 * i + 1] = MDS[3][(P[P_31][(P[P_32][b3] & 0xFF) ^ Twofish_Algorithm.b3(k1)] & 0xFF) ^ Twofish_Algorithm.b3(k0)];
							break;
						
					}
				}
				
				System.Object sessionKey = new System.Object[]{sBox, subKeys};
				
				if (DEBUG && debuglevel > 7)
				{
					System.Console.Out.WriteLine("S-box[]:");
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(sBox[i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(sBox[256 + i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(sBox[512 + i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					 for (i = 0; i < 64; i++)
					{
						 for (j = 0; j < 4; j++)
							System.Console.Out.Write("0x" + intToString(sBox[768 + i * 4 + j]) + ", ");
						System.Console.Out.WriteLine();
					}
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("User (odd, even) keys  --> S-Box keys:");
					 for (i = 0; i < k64Cnt; i++)
					{
						System.Console.Out.WriteLine("0x" + intToString(k32o[i]) + "  0x" + intToString(k32e[i]) + " --> 0x" + intToString(sBoxKey[k64Cnt - 1 - i]));
					}
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("Round keys:");
					 for (i = 0; i < ROUND_SUBKEYS + 2 * ROUNDS; i += 2)
					{
						System.Console.Out.WriteLine("0x" + intToString(subKeys[i]) + "  0x" + intToString(subKeys[i + 1]));
					}
					System.Console.Out.WriteLine();
					
				}
				if (DEBUG)
					trace(OUT, "makeKey()");
				return sessionKey;
			}
		}
		
		/// <summary> Encrypt exactly one block of plaintext.
		/// *
		/// </summary>
		/// <param name="in">       The plaintext.
		/// </param>
		/// <param name="inOffset">  Index of in from which to start considering data.
		/// </param>
		/// <param name="sessionKey"> The session key to use for encryption.
		/// </param>
		/// <returns>The ciphertext generated from a plaintext using the session key.
		/// 
		/// </returns>
		public static byte[] blockEncrypt(byte[] in_Renamed, int inOffset, System.Object sessionKey)
		{
			if (DEBUG)
				trace(IN, "blockEncrypt(" + in_Renamed + ", " + inOffset + ", " + sessionKey + ")");
			System.Object[] sk = (System.Object[]) sessionKey; // extract S-box and session key
			int[] sBox = (int[]) sk[0];
			int[] sKey = (int[]) sk[1];
			
			if (DEBUG && debuglevel > 6)
				System.Console.Out.WriteLine("PT=" + toString(in_Renamed, inOffset, BLOCK_SIZE));
			
			int x0 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			int x1 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			int x2 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			int x3 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			
			x0 ^= sKey[INPUT_WHITEN];
			x1 ^= sKey[INPUT_WHITEN + 1];
			x2 ^= sKey[INPUT_WHITEN + 2];
			x3 ^= sKey[INPUT_WHITEN + 3];
			if (DEBUG && debuglevel > 6)
				System.Console.Out.WriteLine("PTw=" + intToString(x0) + intToString(x1) + intToString(x2) + intToString(x3));
			
			int t0, t1;
			int k = ROUND_SUBKEYS;
			 for (int R = 0; R < ROUNDS; R += 2)
			{
				t0 = Fe32(sBox, x0, 0);
				t1 = Fe32(sBox, x1, 3);
				x2 ^= t0 + t1 + sKey[k++];
				x2 = URShift(x2, 1) | x2 << 31;
				x3 = x3 << 1 | URShift(x3, 31);
				x3 ^= t0 + 2 * t1 + sKey[k++];
				if (DEBUG && debuglevel > 6)
					System.Console.Out.WriteLine("CT" + (R) + "=" + intToString(x0) + intToString(x1) + intToString(x2) + intToString(x3));
				
				t0 = Fe32(sBox, x2, 0);
				t1 = Fe32(sBox, x3, 3);
				x0 ^= t0 + t1 + sKey[k++];
				x0 = URShift(x0, 1) | x0 << 31;
				x1 = x1 << 1 | URShift(x1, 31);
				x1 ^= t0 + 2 * t1 + sKey[k++];
				if (DEBUG && debuglevel > 6)
					System.Console.Out.WriteLine("CT" + (R + 1) + "=" + intToString(x0) + intToString(x1) + intToString(x2) + intToString(x3));
			}
			x2 ^= sKey[OUTPUT_WHITEN];
			x3 ^= sKey[OUTPUT_WHITEN + 1];
			x0 ^= sKey[OUTPUT_WHITEN + 2];
			x1 ^= sKey[OUTPUT_WHITEN + 3];
			if (DEBUG && debuglevel > 6)
				System.Console.Out.WriteLine("CTw=" + intToString(x0) + intToString(x1) + intToString(x2) + intToString(x3));
			
			byte[] result = new byte[]{(byte) x2, (byte) (URShift(x2, 8)), (byte) (URShift(x2, 16)), (byte) (URShift(x2, 24)), (byte) x3, (byte) (URShift(x3, 8)), (byte) (URShift(x3, 16)), (byte) (URShift(x3, 24)), (byte) x0, (byte) (URShift(x0, 8)), (byte) (URShift(x0, 16)), (byte) (URShift(x0, 24)), (byte) x1, (byte) (URShift(x1, 8)), (byte) (URShift(x1, 16)), (byte) (URShift(x1, 24))};
			
			if (DEBUG && debuglevel > 6)
			{
				System.Console.Out.WriteLine("CT=" + toString(result));
				System.Console.Out.WriteLine();
			}
			if (DEBUG)
				trace(OUT, "blockEncrypt()");
			return result;
		}
		
		/// <summary> Decrypt exactly one block of ciphertext.
		/// *
		/// </summary>
		/// <param name="in">       The ciphertext.
		/// </param>
		/// <param name="inOffset">  Index of in from which to start considering data.
		/// </param>
		/// <param name="sessionKey"> The session key to use for decryption.
		/// </param>
		/// <returns>The plaintext generated from a ciphertext using the session key.
		/// 
		/// </returns>
		public static byte[] blockDecrypt(byte[] in_Renamed, int inOffset, System.Object sessionKey)
		{
			if (DEBUG)
				trace(IN, "blockDecrypt(" + in_Renamed + ", " + inOffset + ", " + sessionKey + ")");
			System.Object[] sk = (System.Object[]) sessionKey; // extract S-box and session key
			int[] sBox = (int[]) sk[0];
			int[] sKey = (int[]) sk[1];
			
			if (DEBUG && debuglevel > 6)
				System.Console.Out.WriteLine("CT=" + toString(in_Renamed, inOffset, BLOCK_SIZE));
			
			int x2 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			int x3 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			int x0 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			int x1 = (in_Renamed[inOffset++] & 0xFF) | (in_Renamed[inOffset++] & 0xFF) << 8 | (in_Renamed[inOffset++] & 0xFF) << 16 | (in_Renamed[inOffset++] & 0xFF) << 24;
			
			x2 ^= sKey[OUTPUT_WHITEN];
			x3 ^= sKey[OUTPUT_WHITEN + 1];
			x0 ^= sKey[OUTPUT_WHITEN + 2];
			x1 ^= sKey[OUTPUT_WHITEN + 3];
			if (DEBUG && debuglevel > 6)
				System.Console.Out.WriteLine("CTw=" + intToString(x2) + intToString(x3) + intToString(x0) + intToString(x1));
			
			int k = ROUND_SUBKEYS + 2 * ROUNDS - 1;
			int t0, t1;
			 for (int R = 0; R < ROUNDS; R += 2)
			{
				t0 = Fe32(sBox, x2, 0);
				t1 = Fe32(sBox, x3, 3);
				x1 ^= t0 + 2 * t1 + sKey[k--];
				x1 = URShift(x1, 1) | x1 << 31;
				x0 = x0 << 1 | URShift(x0, 31);
				x0 ^= t0 + t1 + sKey[k--];
				if (DEBUG && debuglevel > 6)
					System.Console.Out.WriteLine("PT" + (ROUNDS - R) + "=" + intToString(x2) + intToString(x3) + intToString(x0) + intToString(x1));
				
				t0 = Fe32(sBox, x0, 0);
				t1 = Fe32(sBox, x1, 3);
				x3 ^= t0 + 2 * t1 + sKey[k--];
				x3 = URShift(x3, 1) | x3 << 31;
				x2 = x2 << 1 | URShift(x2, 31);
				x2 ^= t0 + t1 + sKey[k--];
				if (DEBUG && debuglevel > 6)
					System.Console.Out.WriteLine("PT" + (ROUNDS - R - 1) + "=" + intToString(x2) + intToString(x3) + intToString(x0) + intToString(x1));
			}
			x0 ^= sKey[INPUT_WHITEN];
			x1 ^= sKey[INPUT_WHITEN + 1];
			x2 ^= sKey[INPUT_WHITEN + 2];
			x3 ^= sKey[INPUT_WHITEN + 3];
			if (DEBUG && debuglevel > 6)
				System.Console.Out.WriteLine("PTw=" + intToString(x2) + intToString(x3) + intToString(x0) + intToString(x1));
			
			byte[] result = new byte[]{(byte) x0, (byte) (URShift(x0, 8)), (byte) (URShift(x0, 16)), (byte) (URShift(x0, 24)), (byte) x1, (byte) (URShift(x1, 8)), (byte) (URShift(x1, 16)), (byte) (URShift(x1, 24)), (byte) x2, (byte) (URShift(x2, 8)), (byte) (URShift(x2, 16)), (byte) (URShift(x2, 24)), (byte) x3, (byte) (URShift(x3, 8)), (byte) (URShift(x3, 16)), (byte) (URShift(x3, 24))};
			
			if (DEBUG && debuglevel > 6)
			{
				System.Console.Out.WriteLine("PT=" + toString(result));
				System.Console.Out.WriteLine();
			}
			if (DEBUG)
				trace(OUT, "blockDecrypt()");
			return result;
		}
		
		/// <summary>A basic symmetric encryption/decryption test. 
		/// </summary>
		public static bool self_test()
		{
			return self_test(BLOCK_SIZE);
		}
		
		
		// own methods
		//...........................................................................
		
		private static int b0(int x)
		{
			return x & 0xFF;
		}
		private static int b1(int x)
		{
			return (URShift(x, 8)) & 0xFF;
		}
		private static int b2(int x)
		{
			return (URShift(x, 16)) & 0xFF;
		}
		private static int b3(int x)
		{
			return (URShift(x, 24)) & 0xFF;
		}
		
		/// <summary> Use (12, 8) Reed-Solomon code over GF(256) to produce a key S-box
		/// 32-bit entity from two key material 32-bit entities.
		/// *
		/// </summary>
		/// <param name="k0"> 1st 32-bit entity.
		/// </param>
		/// <param name="k1"> 2nd 32-bit entity.
		/// </param>
		/// <returns> Remainder polynomial generated using RS code
		/// 
		/// </returns>
		private static int RS_MDS_Encode(int k0, int k1)
		{
			int r = k1;
			 for (int i = 0; i < 4; i++)
				r = RS_rem(r);
			r ^= k0;
			 for (int i = 0; i < 4; i++)
				r = RS_rem(r);
			return r;
		}
		
		/*
		* Reed-Solomon code parameters: (12, 8) reversible code:<p>
		* <pre>
		*   g(x) = x**4 + (a + 1/a) x**3 + a x**2 + (a + 1/a) x + 1
		* </pre>
		* where a = primitive root of field generator 0x14D
		*/
		private static int RS_rem(int x)
		{
			int b = (URShift(x, 24)) & 0xFF;
			int g2 = ((b << 1) ^ ((b & 0x80) != 0?RS_GF_FDBK:0)) & 0xFF;
			int g3 = (URShift(b, 1)) ^ ((b & 0x01) != 0?(URShift(RS_GF_FDBK, 1)):0) ^ g2;
			int result = (x << 8) ^ (g3 << 24) ^ (g2 << 16) ^ (g3 << 8) ^ b;
			return result;
		}
		
		private static int F32(int k64Cnt, int x, int[] k32)
		{
			int b0 = Twofish_Algorithm.b0(x);
			int b1 = Twofish_Algorithm.b1(x);
			int b2 = Twofish_Algorithm.b2(x);
			int b3 = Twofish_Algorithm.b3(x);
			int k0 = k32[0];
			int k1 = k32[1];
			int k2 = k32[2];
			int k3 = k32[3];
			
			int result = 0;
			switch (k64Cnt & 3)
			{
				case 1: 
					result = MDS[0][(P[P_01][b0] & 0xFF) ^ Twofish_Algorithm.b0(k0)] ^ MDS[1][(P[P_11][b1] & 0xFF) ^ Twofish_Algorithm.b1(k0)] ^ MDS[2][(P[P_21][b2] & 0xFF) ^ Twofish_Algorithm.b2(k0)] ^ MDS[3][(P[P_31][b3] & 0xFF) ^ Twofish_Algorithm.b3(k0)];
					break;
				
				case 0: 
					// same as 4
					b0 = (P[P_04][b0] & 0xFF) ^ Twofish_Algorithm.b0(k3);
					b1 = (P[P_14][b1] & 0xFF) ^ Twofish_Algorithm.b1(k3);
					b2 = (P[P_24][b2] & 0xFF) ^ Twofish_Algorithm.b2(k3);
					b3 = (P[P_34][b3] & 0xFF) ^ Twofish_Algorithm.b3(k3);
					goto case 3;
				
				case 3: 
					b0 = (P[P_03][b0] & 0xFF) ^ Twofish_Algorithm.b0(k2);
					b1 = (P[P_13][b1] & 0xFF) ^ Twofish_Algorithm.b1(k2);
					b2 = (P[P_23][b2] & 0xFF) ^ Twofish_Algorithm.b2(k2);
					b3 = (P[P_33][b3] & 0xFF) ^ Twofish_Algorithm.b3(k2);
					goto case 2;
				
				case 2: 
					// 128-bit keys (optimize for this case)
					result = MDS[0][(P[P_01][(P[P_02][b0] & 0xFF) ^ Twofish_Algorithm.b0(k1)] & 0xFF) ^ Twofish_Algorithm.b0(k0)] ^ MDS[1][(P[P_11][(P[P_12][b1] & 0xFF) ^ Twofish_Algorithm.b1(k1)] & 0xFF) ^ Twofish_Algorithm.b1(k0)] ^ MDS[2][(P[P_21][(P[P_22][b2] & 0xFF) ^ Twofish_Algorithm.b2(k1)] & 0xFF) ^ Twofish_Algorithm.b2(k0)] ^ MDS[3][(P[P_31][(P[P_32][b3] & 0xFF) ^ Twofish_Algorithm.b3(k1)] & 0xFF) ^ Twofish_Algorithm.b3(k0)];
					break;
				
			}
			return result;
		}
		
		private static int Fe32(int[] sBox, int x, int R)
		{
			return sBox[2 * _b(x, R)] ^ sBox[2 * _b(x, R + 1) + 1] ^ sBox[0x200 + 2 * _b(x, R + 2)] ^ sBox[0x200 + 2 * _b(x, R + 3) + 1];
		}
		
		private static int _b(int x, int N)
		{
			int result = 0;
			switch (N % 4)
			{
				case 0: 
					result = b0(x); break;
				
				case 1: 
					result = b1(x); break;
				
				case 2: 
					result = b2(x); break;
				
				case 3: 
					result = b3(x); break;
				
			}
			return result;
		}
		
		/// <returns>The length in bytes of the Algorithm input block. 
		/// </returns>
		public static int blockSize()
		{
			return BLOCK_SIZE;
		}
		
		/// <summary>A basic symmetric encryption/decryption test for a given key size. 
		/// </summary>
		public static bool self_test(int keysize)
		{
			if (DEBUG)
				trace(IN, "self_test(" + keysize + ")");
			bool ok = false;
			try
			{
				byte[] kb = new byte[keysize];
				byte[] pt = new byte[BLOCK_SIZE];
				int i;
				
				 for (i = 0; i < keysize; i++)
					kb[i] = (byte) i;
				 for (i = 0; i < BLOCK_SIZE; i++)
					pt[i] = (byte) i;
				
				if (DEBUG && debuglevel > 6)
				{
					System.Console.Out.WriteLine("==========");
					System.Console.Out.WriteLine();
					System.Console.Out.WriteLine("KEYSIZE=" + (8 * keysize));
					System.Console.Out.WriteLine("KEY=" + toString(kb));
					System.Console.Out.WriteLine();
				}
				System.Object key = makeKey(kb);
				
				if (DEBUG && debuglevel > 6)
				{
					System.Console.Out.WriteLine("Intermediate Ciphertext Values (Encryption)");
					System.Console.Out.WriteLine();
				}
				byte[] ct = blockEncrypt(pt, 0, key);
				
				if (DEBUG && debuglevel > 6)
				{
					System.Console.Out.WriteLine("Intermediate Plaintext Values (Decryption)");
					System.Console.Out.WriteLine();
				}
				byte[] cpt = blockDecrypt(ct, 0, key);
				
				ok = areEqual(pt, cpt);
				if (!ok)
					throw new System.SystemException("Symmetric operation failed");
			}
			catch (System.Exception x)
			{
				if (DEBUG && debuglevel > 0)
				{
					debug("Exception encountered during self-test: " + x.Message);
					WriteStackTrace(x, Console.Error);
				}
			}
			if (DEBUG && debuglevel > 0)
				debug("Self-test OK? " + ok);
			if (DEBUG)
				trace(OUT, "self_test()");
			return ok;
		}
		
		
		// utility static methods (from cryptix.util.core ArrayUtil and Hex classes)
		//...........................................................................
		
		/// <returns>True iff the arrays have identical contents. 
		/// </returns>
		private static bool areEqual(byte[] a, byte[] b)
		{
			int aLength = a.Length;
			if (aLength != b.Length)
				return false;
			 for (int i = 0; i < aLength; i++)
				if (a[i] != b[i])
					return false;
			return true;
		}
		
		/// <summary> Returns a string of 8 hexadecimal digits (most significant
		/// digit first) corresponding to the integer <i>n</i>, which is
		/// treated as unsigned.
		/// </summary>
		private static System.String intToString(int n)
		{
			char[] buf = new char[8];
			 for (int i = 7; i >= 0; i--)
			{
				buf[i] = HEX_DIGITS[n & 0x0F];
				n = URShift(n, 4);
			}
			return new String(buf);
		}
		
		/// <summary> Returns a string of hexadecimal digits from a byte array. Each
		/// byte is converted to 2 hex symbols.
		/// </summary>
		private static System.String toString(byte[] ba)
		{
			return toString(ba, 0, ba.Length);
		}
		private static System.String toString(byte[] ba, int offset, int length)
		{
			char[] buf = new char[length * 2];
			 for (int i = offset, j = 0, k; i < offset + length; )
			{
				k = ba[i++];
				buf[j++] = HEX_DIGITS[(URShift(k, 4)) & 0x0F];
				buf[j++] = HEX_DIGITS[k & 0x0F];
			}
			return new String(buf);
		}
		
      // main(): use to generate the Intermediate Values KAT
      //...........................................................................

      public static int URShift(int number, int bits)
      {
         if ( number >= 0)
            return number >> bits;
         else
            return (number >> bits) + (2 << ~bits);
      }

      public static int URShift(int number, long bits)
      {
         return URShift(number, (int)bits);
      }

      public static long URShift(long number, int bits)
      {
         if ( number >= 0)
            return number >> bits;
         else
            return (number >> bits) + (2L << ~bits);
      }

      public static long URShift(long number, long bits)
      {
         return URShift(number, (int)bits);
      }

      public static void WriteStackTrace(System.Exception throwable, System.IO.TextWriter stream)
      {
         stream.Write(throwable.StackTrace);
         stream.Flush();
      }

		
		// main(): use to generate the Intermediate Values KAT
		//...........................................................................
		
		[STAThread]
		public static void  Main(System.String[] args)
		{
			self_test(16);
			self_test(24);
			self_test(32);
		}
	}
}