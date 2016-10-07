
/* 
  Copyright 2001-2003 Markus Hahn <markus_hahn@gmx.net>
  All rights reserved.
  See Documentation for license details.  
*/


using System;
using System.Text;
using System.Security.Cryptography;


namespace Blowfish_NET
{

	/// <summary>
	///   simple and easy to use string encryption and 
	///   decryption using the Blowfish algorithm
	/// </summary>
	/// <remarks>
	///   As a simple solution for developers who want nothing
	///   more than protect simple strings with a simple key this
	///   class provides such a functionality. 
	///   The password is hashed using the build-in SHA-1 class
	///   of the .NET framework. Additionally the random number
	///   generation for the CBC initialization vector (IV) and
	///   the BASE64 encoding and decoding is also taken from
	///   the system security services.
	/// </remarks>
	public class BlowfishSimple
	{
		BlowfishCBC m_bfc;

		UTF8Encoding m_ue;
		RNGCryptoServiceProvider m_rng;

		String m_sKeyChecksum;


		/// <summary>
		///   secure checksum of the key used
		/// </summary>
		/// <remarks> 
		///   Store this checksum somewhere to be able to check later on
		///   by calling the VerifyKey() method to see if a key matches
		///   for decryption or not.
		/// </remarks> 
		public String KeyChecksum
		{
			get
			{
				return m_sKeyChecksum;
			} 
		}


		static byte[] TransformKey
			(String sKey)
		{
			UTF8Encoding ue = new UTF8Encoding();
			return ue.GetBytes(sKey);
		}


		static byte[] CalcKeyChecksum
			(byte[] salt,
			byte[] key)
		{
			byte[] keyCombo = new byte[20 + key.Length];

			Array.Copy(salt, 0, keyCombo, 0, 20);
			Array.Copy(key, 0, keyCombo, 20, key.Length);

			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			byte[] result = sha.ComputeHash(keyCombo);
			sha = null; 

			Array.Clear(keyCombo, 0, keyCombo.Length);

			return result;
		}
  

		/// <summary>
		///   to verify a key before it is used for decryption
		/// </summary>
		/// <remarks> 
		///   By passing the current key available and a key checksum
		///   retrieved during the former encryption process you can
		///   check (with a propability of the SHA-1 collision safety)
		///   that this key will decrypt the data correctly. 
		/// </remarks> 
		/// <param name="sKey"> 
		///   the key to verify 
		/// </param> 
		/// <param name="sKeyChecksum"> 
		///   the original key checksum
		/// </param> 
		/// <returns> 
		///   true: key seems to be the right one / false: no match
		/// </returns> 
		public static bool VerifyKey
			(String sKey,
 			String sKeyChecksum)
		{
			// decode what we got
			byte[] checksumCombo = Convert.FromBase64String(sKeyChecksum);
 
			// correct size?     
			if (40 != checksumCombo.Length) return false;

			// calculate the new checksum
			byte[] keyRaw = TransformKey(sKey);
			byte[] checksum = CalcKeyChecksum(checksumCombo, keyRaw);
 
			// is it equal to the existing one?
			int nI = 0;
			while (nI < checksum.Length)
			{
				if (checksum[nI] != checksumCombo[checksum.Length + nI]) break;
				nI++;
			} 
   
			return (nI == checksum.Length);
		}


		/// <summary>
		///   standard constructor
		/// </summary>
		/// <param name="sKey"> 
		///   the string which is used as the key material, internally 
		///   a UTF8 representation is used, hashed with SHA-1, thus
		///   we use a 160bit key (which does not make weak keys safe!)
		/// </param>
		public BlowfishSimple
			(String sKey)
		{
			m_ue = new UTF8Encoding();

			byte[] keyRaw = TransformKey(sKey);

			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			byte[] key = sha.ComputeHash(keyRaw);
			sha = null;

			m_rng = new RNGCryptoServiceProvider();

			byte[] checksumSalt = new byte[20];
			m_rng.GetBytes(checksumSalt); 

			byte[] checksum = CalcKeyChecksum(checksumSalt, keyRaw);
			byte[] checksumCombo = new byte[checksumSalt.Length + checksum.Length];
			Array.Copy(checksumSalt, 0, checksumCombo, 0, checksumSalt.Length);
			Array.Copy(checksum, 0, checksumCombo, checksumSalt.Length, checksum.Length);

			m_sKeyChecksum = Convert.ToBase64String(checksumCombo);

			// (start with a dummy IV)    
			byte[] iv = new byte[Blowfish.BLOCKSIZE];
 
			m_bfc = new BlowfishCBC(key, iv);

			Array.Clear(keyRaw, 0, keyRaw.Length);
			Array.Clear(key, 0, key.Length);
			Array.Clear(iv, 0, iv.Length);
		}


		/// <summary>
		///   encrypts a string
		/// </summary>
		/// <remarks>
		///   for efficiency the given string will be UTF8 encoded
		///   and padded to the next block border, the CBC IV plus
		///   the encrypted data will then be BASE64 encoded and
		///   returned as an string
		/// </remarks>
		/// <param name="sPlainText"> 
		///   the string to encrypt
		/// </param> 
		/// <returns> 
		///   the encrypted string 
		/// </returns> 
		public String Encrypt
			(String sPlainText)
		{
			int nI;

			// convert string
			byte[] ueData = m_ue.GetBytes(sPlainText);

			// prepare the input and the output buffer
			int nOrigLen = ueData.Length;
			int nLen = nOrigLen;

			// (we need aligned and "pad-able" buffers) 
			int nMod = nLen % Blowfish.BLOCKSIZE;
			nLen = (nLen - nMod) + Blowfish.BLOCKSIZE;

			// allocate the input buffer
			byte[] inBuf = new byte[nLen];

			// copy the data and do the padding
			Array.Copy(ueData, 0, inBuf, 0, nOrigLen);
    
			nI = nLen - (Blowfish.BLOCKSIZE - nMod);
			while (nI < nLen) inBuf[nI++] = (byte)nMod;
   
			// allocate the output buffer
			byte[] outBuf = new byte[inBuf.Length + Blowfish.BLOCKSIZE];

			// create and set a new IV
			byte[] iv = new byte[Blowfish.BLOCKSIZE];
			m_rng.GetBytes(iv);
			m_bfc.Iv = iv;

			// do the encryption
			m_bfc.Encrypt(inBuf, 
				outBuf, 
				0, 
				Blowfish.BLOCKSIZE, 
				inBuf.Length);

			// copy the IV
			Array.Copy(iv, 0, outBuf, 0, Blowfish.BLOCKSIZE);  

			// BASE64 encode the whole thing
			String sResult = Convert.ToBase64String(outBuf);
 
			// finally clear the plaintext buffer
			Array.Clear(inBuf, 0, inBuf.Length);

			return sResult;
		}


		/// <summary>
		///   decrypts a string
		/// </summary>
		/// <remarks>
		///   The string has to be decrypted with the same key, 
		///   otherwise the result will be just garbage. If you
		///   want to check if the key is the right one use the
		///   VerifyKey() method.
		/// </remarks>
		/// <param name="sCipherText"> 
		///   the string to decrypt
		/// </param> 
		/// <returns> 
		///   the decrypted (original) string (null on error)
		/// </returns> 
		public String Decrypt
			(String sCipherText)
		{
			byte[] cdata = Convert.FromBase64String(sCipherText);

			if (cdata.Length < Blowfish.BLOCKSIZE) return null;

			// set the cbc IV
			m_bfc.Iv = cdata;
  
			// decrypt the data
			byte[] outBuf = new byte[cdata.Length];

			int nDataAbs = cdata.Length - Blowfish.BLOCKSIZE; 
			nDataAbs /= Blowfish.BLOCKSIZE;  
			nDataAbs *= Blowfish.BLOCKSIZE;  

			m_bfc.Decrypt(cdata, 
				outBuf, 
				Blowfish.BLOCKSIZE, 
				0, 
				nDataAbs);

			// calculate the original size
			int nOrigSize = nDataAbs - 
				Blowfish.BLOCKSIZE + 
				outBuf[nDataAbs - 1];
 
			// UTF8 decode the result and pass it back

			return m_ue.GetString(outBuf, 0, nOrigSize);
		}


		/// <summary>
		///   securely invalidates this instance
		/// </summary>
		/// <remarks>
		///   Removes all sensitive data, after this call it is 
		///   not possible to encrypt or data anymore properly!  
		/// </remarks>
		/// <returns>
		///   self reference
		/// </returns>
		public BlowfishSimple Burn()
		{
			m_bfc.Burn();
			return this;
		}
	}


}	// (end of namespace)

