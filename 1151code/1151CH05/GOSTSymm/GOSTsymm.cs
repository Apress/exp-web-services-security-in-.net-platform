///////////////////////////////////////////////////////////////////////////////////
/////
/////		GOST symmetric encryption algorythm implementation for .NET
/////
/////		Author: Ivan Medvedev
/////		Date:	18 Jul 2001
/////

namespace GOST
{
	using System;
	using System.Security.Cryptography;


	////////////////////////////////////////////////////////////////////////////////
	//
	//	Abstract GOSTsymm class
	//
	public abstract class GOSTsymm : SymmetricAlgorithm
	{
		// GOST block size is 64 bits
		private static  KeySizes[] s_legalBlockSizes = { new KeySizes(64, 64, 0) };
		// GOST key size is 256 bits
		private static  KeySizes[] s_legalKeySizes = { new KeySizes(256, 256, 0) };
		// S-boxes
		protected byte[,]		SBoxesValue;

		public GOSTsymm()
		{
			KeySizeValue = 256;
			BlockSizeValue = 64;
			FeedbackSizeValue = BlockSizeValue;
			LegalBlockSizesValue = s_legalBlockSizes;
			LegalKeySizesValue = s_legalKeySizes;
		}

		public virtual byte[,] SBoxes {
			get { 
				if (SBoxesValue==null) GenerateSBoxes();
				return (byte[,])SBoxesValue.Clone();
			}
			set { 
				if (value == null) throw new ArgumentNullException();
				if ( (value.Rank!=2) ||
					 (value.GetLength(0)!=8) ||
					 (value.GetLength(1)!=16) ) throw new CryptographicException("S-Boxes must be a 2-dimentional array 8x16 bytes");
				SBoxesValue = (byte[,])value.Clone();
			}
		}

		public abstract void GenerateSBoxes();

		new static public GOSTsymm Create() 
		{
			return Create("GOST.GOSTsymm");
		}

		new static public GOSTsymm Create(String algName) 
		{
			return (GOSTsymm) CryptoConfig.CreateFromName(algName);
		}

		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{

			return CreateEncryptor(rgbKey, rgbIV, SBoxes);
		}

		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			return CreateDecryptor(rgbKey, rgbIV, SBoxes);
		}

		public abstract ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV, byte[,] rgbSBoxes);
		
		public abstract ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV, byte[,] rgbSBoxes);

	}


	////////////////////////////////////////////////////////////////////////////////
	//
	//	GOSTsymmManaged class
	//
	public class GOSTsymmManaged : GOSTsymm
	{
		public GOSTsymmManaged()
		{
			//
		}

		public override void GenerateKey()
		{
			if ( (KeyValue==null) || (KeyValue.Length!=KeySizeValue/8) ) KeyValue = new byte[KeySizeValue/8];
			RNG.GetBytes(KeyValue);
		}

		public override void GenerateIV()
		{
			if ( (IVValue==null) || (IVValue.Length!=BlockSizeValue/8) ) IVValue = new byte[BlockSizeValue/8];
			RNG.GetBytes(IVValue);
		}

		public override void GenerateSBoxes()
		{
			if (SBoxesValue==null) SBoxesValue = new byte[8,16];
			byte[] tmp = new byte[128];
			RNG.GetBytes(tmp);
			for(int j=0; j<8; j++)
				for(int i=0; i<16; i++) SBoxesValue[j,i] = tmp[j*16+i];
		}

		public void LoadTestSBoxes(int i)
		{
			switch(i) 
			{
				case 0:
					SBoxesValue = new byte[8,16] {
										{4,10,9,2,13,8,0,14,6,11,1,12,7,15,5,3},
										{14,11,4,12,6,13,15,10,2,3,8,1,0,5,7,9},
										{5,8,1,13,10,3,4,2,14,15,12,7,6,0,9,11},
										{7,13,10,1,0,8,9,15,14,4,6,12,11,2,5,3},
										{6,12,7,1,5,15,13,8,4,10,9,14,0,3,11,2},
										{4,11,10,0,7,2,1,13,3,6,8,5,9,12,15,14},
										{13,11,4,1,3,15,5,9,0,10,14,7,6,8,2,12},
										{1,15,13,0,5,7,10,4,9,2,3,14,6,11,8,12} };
					break;
				case 1:
					SBoxesValue = new byte[8,16] {
							{ 14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7 },
							{ 15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10 },
							{ 10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8 },
							{  7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15 },
							{  2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9 },
							{ 12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11 },
							{  4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1 },
							{ 13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7 } };
					break;
				case 2:
					SBoxesValue = new byte[8,16] {
							{ 13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7 },
							{  4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1 },
							{ 12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11 },
							{  2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9 },
							{  7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15 },
							{ 10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8 },
							{ 15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10 },
							{ 14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7 }
						 };
					break;

			}


		}


		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV, byte[,] rgbSBoxes)
		{
			return _NewCryptor(rgbKey, ModeValue, rgbIV, FeedbackSizeValue, rgbSBoxes, true);
		}
		
		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV, byte[,] rgbSBoxes)
		{
			return _NewCryptor(rgbKey, ModeValue, rgbIV, FeedbackSizeValue, rgbSBoxes, false);
		}

		//
		//	private members
		//
		private RNGCryptoServiceProvider _rng;

		private RNGCryptoServiceProvider RNG 
		{
			get { if (_rng == null) { _rng = new RNGCryptoServiceProvider(); } return _rng; }
		}


		private ICryptoTransform _NewCryptor(byte[] rgbKey, CipherMode mode, byte[] rgbIV, int feedbackSize, byte[,] rgbSBoxes, bool bEncrypt)
		{
			if (rgbKey == null) rgbKey = Key;
			// we don't support stream modes
			if ( (mode != CipherMode.ECB) && (mode != CipherMode.CBC) )
				throw new CryptographicException("Only ECB and CBC modes are supported.");
			// if the mode is not ECB we will need an IV
			if (rgbIV == null) rgbIV = IV;

            return new GOSTsymmTransform(rgbKey, mode, rgbIV, BlockSizeValue, feedbackSize, rgbSBoxes, PaddingValue, bEncrypt);
		}



	}
	


	////////////////////////////////////////////////////////////////////////////////
	//
	//	GOSTsymmTransform class
	//
	public class GOSTsymmTransform : ICryptoTransform
	{
		private int			m_blockSize;
		private int			m_blockSizeInBytes;
		private byte[]		m_key;
		private UInt32[]	m_key_auint;
		private byte[]		m_IV;
		private byte[,]		m_SBoxes;
		private CipherMode	m_mode;
		private PaddingMode	m_padding;
		private bool		m_encrypt;	// true - we are encrypting, false - we are decrypting.
		private byte[,]		m_expandedSBoxes;
		private bool		m_Disposed;
		private byte[]		m_lastCipherBlock;
		private UInt32[]	m_working_block;
		private byte[]		m_depadBuffer;


		public	bool		UseExpandedSBoxes;


		// GOSTsymmTransform constructor 
		public GOSTsymmTransform(
								byte[] rgbKey,
								CipherMode mode,
								byte[] rgbIV,
								int blockSize,
								int feedbackSize,
								byte[,] rgbSBoxes,
								PaddingMode paddingValue,
								bool bEncrypt )
		{
			// do all the validation in the constructor so we don't bother later
			if (blockSize != 64) throw new ArgumentException("GOST block size should be 64", "blockSize");
			m_blockSize = blockSize;
			m_blockSizeInBytes = blockSize/8;

			m_padding = paddingValue;

			m_encrypt = bEncrypt;

			if (rgbKey == null) throw new ArgumentNullException("rgbKey");
			if (rgbKey.Length != 32)
				throw new ArgumentException("GOST key length should be 32 bytes / 256 bits", "rgbKey");
			m_key = (byte[])rgbKey.Clone();
			// keep the key in array-of-UInt32 form
			m_key_auint = new UInt32[8];
			_BytesToUInts(m_key, 0, 32, m_key_auint);

			if (mode == CipherMode.ECB) {
				m_IV = null;
			} else if (mode == CipherMode.CBC) {
				if (rgbIV == null) throw new ArgumentNullException("rgbIV");
				m_IV = (byte[])rgbIV.Clone();
			} else throw new ArgumentException("Only ECB and CBC modes are supported", "mode");
			m_mode = mode;

			if (rgbSBoxes == null) throw new ArgumentNullException("rgbSBoxes");
			if ( (rgbSBoxes.GetLength(0) != 8) || (rgbSBoxes.GetLength(1) != 16) )
				throw new ArgumentException("SBoxes should be an array of 8x16 bytes", "rgbSBoxes");
			m_SBoxes = (byte[,])rgbSBoxes.Clone();

			UseExpandedSBoxes = true;
			m_expandedSBoxes = null;

			m_Disposed = false;

			m_working_block = new UInt32[2];

			_Reset();
		}

		void IDisposable.Dispose() 
		{
			// clean up keys and sensitive information
			Array.Clear(m_key, 0, m_key.Length);
            if (m_IV != null) Array.Clear(m_IV, 0, m_IV.Length);
			Array.Clear(m_SBoxes, 0, m_SBoxes.Length);	// will this work for a 2-dim array?

			// TODO: clear the buffers

			// since we've cleared everything here, we don't need the finalizer to be called
			GC.SuppressFinalize(this);

			m_Disposed = true;
		}

		public int InputBlockSize
		{
			get
			{
				return m_blockSizeInBytes;
			}
		}

		public int OutputBlockSize
		{
			get
			{
				return m_blockSizeInBytes;
			}
		}

		public bool CanTransformMultipleBlocks
		{
			get
			{
				return true;
			}
		}

		public bool CanReuseTransform
		{
			get 
			{
				return true;
			}
		}

		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) 
		{
			if (m_Disposed == true)
				throw new ObjectDisposedException("this", "Transform object is disposed");

			if (inputBuffer == null)
				throw new ArgumentNullException( "inputBuffer" );

			if (outputBuffer == null)
				throw new ArgumentNullException( "outputBuffer" );

			if (inputBuffer.Length < inputOffset + inputCount)
				throw new CryptographicException("Requested processing of bytes beyound the end of the input buffer");

			if (inputCount%(m_blockSizeInBytes) != 0)
				throw new ArgumentException("Input data should be a multiple of the block size (8 bytes)", "inputCount");

			if(inputCount==0) return 0;

			if (outputBuffer.Length-outputOffset<inputCount)
				throw new CryptographicException("Output buffer has insufficient length");

			if (m_encrypt == true)
			{
				// we are encrypting
				int		offset = 0;
				byte[]	temp_block = new byte[m_blockSizeInBytes];
				byte[]	block_to_encrypt = null;
				int		offset_to_encrypt = 0;
				byte[]	last_cipher_block = m_lastCipherBlock;
				int		offset_cipher_block = 0;

				while(offset < inputCount) 
				{
					if (m_mode == CipherMode.CBC) 
					{
						if (offset>0) {last_cipher_block = outputBuffer; offset_cipher_block = offset-m_blockSizeInBytes; }
						_XorBlocks(inputBuffer, inputOffset+offset, last_cipher_block, offset_cipher_block, temp_block, 0);
						block_to_encrypt = temp_block;
						offset_to_encrypt = 0;
					} 
					else 
					{
						block_to_encrypt = inputBuffer;
						offset_to_encrypt = inputOffset + offset;
					}

					_EncryptBlock(block_to_encrypt, offset_to_encrypt, outputBuffer, outputOffset + offset);

					
					offset += m_blockSizeInBytes;
				}

				// memorize last cipher block to chain it in the future
				if (m_mode == CipherMode.CBC)
					Array.Copy(outputBuffer, outputOffset+offset-m_blockSizeInBytes, m_lastCipherBlock, 0, m_blockSizeInBytes);

				return (offset);
			}
			else
			{
				// we are decrypting
				int		inoffset = 0;
				int		outoffset = 0;
				byte[]	temp_block = new byte[m_blockSizeInBytes];
				byte[]	last_cipher_block = m_lastCipherBlock;
				int		offset_cipher_block = 0;

				if (m_padding == PaddingMode.PKCS7) 
				{
					inputCount -= m_blockSizeInBytes;
					if (m_depadBuffer == null) 
					{
						m_depadBuffer = new byte[m_blockSizeInBytes];
					} 
					else 
					{
						_DecryptBlock(m_depadBuffer, 0, outputBuffer, outputOffset);
						if (m_mode == CipherMode.CBC) 
						{
							_XorBlocks(outputBuffer, outputOffset, last_cipher_block, offset_cipher_block, outputBuffer, outputOffset);
							Array.Copy(m_depadBuffer, 0, m_lastCipherBlock, 0, m_blockSizeInBytes);
						}
						outoffset += m_blockSizeInBytes;
					}
					Array.Copy(inputBuffer, inputOffset+inputCount, m_depadBuffer, 0, m_blockSizeInBytes);
				}

				while(inoffset < inputCount) 
				{
					_DecryptBlock(inputBuffer, inputOffset + inoffset, outputBuffer, outputOffset + outoffset);

					if (m_mode == CipherMode.CBC) 
					{
						if (inoffset>0) {last_cipher_block = inputBuffer; offset_cipher_block = inoffset-m_blockSizeInBytes; }
						_XorBlocks(outputBuffer, outputOffset+outoffset, last_cipher_block, offset_cipher_block, outputBuffer, outputOffset+outoffset);
					}

					inoffset += m_blockSizeInBytes;
					outoffset += m_blockSizeInBytes;
				}

                // memorize last cipher block to chain it in the future
				if ( (m_mode == CipherMode.CBC) && (inoffset >= m_blockSizeInBytes) )
					Array.Copy(inputBuffer, inputOffset+inoffset-m_blockSizeInBytes, m_lastCipherBlock, 0, m_blockSizeInBytes);


				return (inoffset);
			}
		}

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) 
		{ 
			if (inputBuffer == null)
				throw new ArgumentNullException( "inputBuffer" );

			if (inputOffset+inputCount>inputBuffer.Length)
				throw new CryptographicException( "Requested processing of bytes beyound the end of the input buffer" );

			// if there is no padding, just call TransformBlock
			if (m_padding == PaddingMode.None) {
				byte[] resultBytes = new byte[inputCount];
				TransformBlock(inputBuffer, inputOffset, inputCount, resultBytes, 0);
				_Reset();
				return resultBytes;
			}


			if (m_encrypt) 
			{
				// we are encrypting
				byte[] paddedPlaintext = _Pad(inputBuffer, inputOffset, inputCount);
				byte[] resultBytes = new byte[paddedPlaintext.Length];
				TransformBlock(paddedPlaintext, 0, paddedPlaintext.Length, resultBytes, 0);
				_Reset();	// resets the transform so it could be used again
				return resultBytes;
			}		
			else 
			{
				// we are decrypting
				byte[] resultBytes = new byte[inputCount];
				TransformBlock(inputBuffer, inputOffset, inputCount, resultBytes, 0);
				if ((m_padding == PaddingMode.PKCS7) && (m_depadBuffer != null))
				{
					byte[] decryptedPadding = new byte[m_blockSizeInBytes];
                    TransformBlock(new byte[m_blockSizeInBytes], 0, m_blockSizeInBytes, decryptedPadding, 0);
					int	paddingCount = decryptedPadding[m_blockSizeInBytes-1];
					if (paddingCount > m_blockSizeInBytes)
						throw new CryptographicException("Invalid PKCS7 padding");
					if (paddingCount != m_blockSizeInBytes) {
						byte[] newResultBytes = new byte[resultBytes.Length + m_blockSizeInBytes - paddingCount];
						Array.Copy(resultBytes, 0, newResultBytes, 0, resultBytes.Length);
						Array.Copy(decryptedPadding, 0, newResultBytes, resultBytes.Length, m_blockSizeInBytes-paddingCount);
						resultBytes = newResultBytes;
					}
				}
				_Reset();	// resets the transform so it could be used again
				return resultBytes;
			}
		}


		//
		//	private methods
		//
		private void _XorBlocks(byte[] block1, int offset1, byte[] block2, int offset2, byte[] dest, int dest_offset)
		{
			for (int i=0; i<m_blockSizeInBytes; i++)
				dest[dest_offset+i] = (byte)(block1[offset1+i]^block2[offset2+i]);
		}

		private void _EncryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			_BytesToUInts(inputBuffer, inputOffset, 8, m_working_block);

			UInt32 n1 = m_working_block[0];
			UInt32 n2 = m_working_block[1];

			/* Instead of swapping halves, swap names each round */
			n2 ^= _f(n1+m_key_auint[0]);
			n1 ^= _f(n2+m_key_auint[1]);
			n2 ^= _f(n1+m_key_auint[2]);
			n1 ^= _f(n2+m_key_auint[3]);
			n2 ^= _f(n1+m_key_auint[4]);
			n1 ^= _f(n2+m_key_auint[5]);
			n2 ^= _f(n1+m_key_auint[6]);
			n1 ^= _f(n2+m_key_auint[7]);

			n2 ^= _f(n1+m_key_auint[0]);
			n1 ^= _f(n2+m_key_auint[1]);
			n2 ^= _f(n1+m_key_auint[2]);
			n1 ^= _f(n2+m_key_auint[3]);
			n2 ^= _f(n1+m_key_auint[4]);
			n1 ^= _f(n2+m_key_auint[5]);
			n2 ^= _f(n1+m_key_auint[6]);
			n1 ^= _f(n2+m_key_auint[7]);

			n2 ^= _f(n1+m_key_auint[0]);
			n1 ^= _f(n2+m_key_auint[1]);
			n2 ^= _f(n1+m_key_auint[2]);
			n1 ^= _f(n2+m_key_auint[3]);
			n2 ^= _f(n1+m_key_auint[4]);
			n1 ^= _f(n2+m_key_auint[5]);
			n2 ^= _f(n1+m_key_auint[6]);
			n1 ^= _f(n2+m_key_auint[7]);

			n2 ^= _f(n1+m_key_auint[7]);
			n1 ^= _f(n2+m_key_auint[6]);
			n2 ^= _f(n1+m_key_auint[5]);
			n1 ^= _f(n2+m_key_auint[4]);
			n2 ^= _f(n1+m_key_auint[3]);
			n1 ^= _f(n2+m_key_auint[2]);
			n2 ^= _f(n1+m_key_auint[1]);
			n1 ^= _f(n2+m_key_auint[0]);

			/* There is no swap after the last round */
			m_working_block[0] = n2;
			m_working_block[1] = n1;

			_UIntsToBytes(m_working_block, outputBuffer, outputOffset);

		}

		private void _DecryptBlock(byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			_BytesToUInts(inputBuffer, inputOffset, 8, m_working_block);

			UInt32 n1 = m_working_block[0];
			UInt32 n2 = m_working_block[1];

			n2 ^= _f(n1+m_key_auint[0]);
			n1 ^= _f(n2+m_key_auint[1]);
			n2 ^= _f(n1+m_key_auint[2]);
			n1 ^= _f(n2+m_key_auint[3]);
			n2 ^= _f(n1+m_key_auint[4]);
			n1 ^= _f(n2+m_key_auint[5]);
			n2 ^= _f(n1+m_key_auint[6]);
			n1 ^= _f(n2+m_key_auint[7]);

			n2 ^= _f(n1+m_key_auint[7]);
			n1 ^= _f(n2+m_key_auint[6]);
			n2 ^= _f(n1+m_key_auint[5]);
			n1 ^= _f(n2+m_key_auint[4]);
			n2 ^= _f(n1+m_key_auint[3]);
			n1 ^= _f(n2+m_key_auint[2]);
			n2 ^= _f(n1+m_key_auint[1]);
			n1 ^= _f(n2+m_key_auint[0]);

			n2 ^= _f(n1+m_key_auint[7]);
			n1 ^= _f(n2+m_key_auint[6]);
			n2 ^= _f(n1+m_key_auint[5]);
			n1 ^= _f(n2+m_key_auint[4]);
			n2 ^= _f(n1+m_key_auint[3]);
			n1 ^= _f(n2+m_key_auint[2]);
			n2 ^= _f(n1+m_key_auint[1]);
			n1 ^= _f(n2+m_key_auint[0]);

			n2 ^= _f(n1+m_key_auint[7]);
			n1 ^= _f(n2+m_key_auint[6]);
			n2 ^= _f(n1+m_key_auint[5]);
			n1 ^= _f(n2+m_key_auint[4]);
			n2 ^= _f(n1+m_key_auint[3]);
			n1 ^= _f(n2+m_key_auint[2]);
			n2 ^= _f(n1+m_key_auint[1]);
			n1 ^= _f(n2+m_key_auint[0]);

			m_working_block[0] = n2;
			m_working_block[1] = n1;

			_UIntsToBytes(m_working_block, outputBuffer, outputOffset);
		}

		private UInt32 _f(UInt32 x)
		{
			UInt32 res = 0;
			if (UseExpandedSBoxes == false) 
			{
				// so we do not use expanded our S-Boxes
				for (int i=0; i<8; i++) 
				{
					res |= ((UInt32)m_SBoxes[i,(x>>((i)*4))&0x0F])<<((i)*4);
				}
			} 
			else 
			{
				// we use expanded S-Boxes
				if (m_expandedSBoxes == null) ExpandSBoxes();

				res = (uint)(m_expandedSBoxes[3,x>>24 & 255] << 24 |
							m_expandedSBoxes[2,x>>16 & 255] << 16 |
							m_expandedSBoxes[1,x>>8 & 255] <<8 |
							m_expandedSBoxes[0,x&255]);
			}
			return res << 11 | res >> (32 - 11);
		}

		private void _BytesToUInts(byte[] inputBuffer, int inputOffset, int lenght, UInt32[] outputBuffer)
		{
			// this guy does not do any parameter checks so be careful
			for(int i=0; i<lenght; i+=4) 
			{
				// for now let's just assume we are always going to work on a little-endian machine
				outputBuffer[i/4] =	((UInt32)(inputBuffer[inputOffset+i+3]))<<24 |
									((UInt32)(inputBuffer[inputOffset+i+2]))<<16 |
									((UInt32)(inputBuffer[inputOffset+i+1]))<<8 |
									((UInt32)(inputBuffer[inputOffset+i]));
			}
		}

		private void _UIntsToBytes(UInt32[] inputBuffer, byte[] outputBuffer, int outputOffset)
		{
			// this guy does not do any parameter checks so be careful\
			int l = inputBuffer.Length;
			for(int i=0; i<l; i++) 
			{
				// for now let's just assume we are always going to work on a little-endian machine
				outputBuffer[outputOffset+i*4+3] = (byte)(inputBuffer[i]>>24);
				outputBuffer[outputOffset+i*4+2] = (byte)(inputBuffer[i]>>16);
				outputBuffer[outputOffset+i*4+1] = (byte)(inputBuffer[i]>>8);
				outputBuffer[outputOffset+i*4] = (byte)(inputBuffer[i]);
			}
		}

		public void ExpandSBoxes()
		{
			if (m_expandedSBoxes == null) m_expandedSBoxes = new byte[4,256];
			for(int i=0;i<256;i++) 
			{
				m_expandedSBoxes[3,i] = (byte)(m_SBoxes[7, i>>4] << 4 | m_SBoxes[6, i&15]);
				m_expandedSBoxes[2,i] = (byte)(m_SBoxes[5, i>>4] << 4 | m_SBoxes[4, i&15]);
				m_expandedSBoxes[1,i] = (byte)(m_SBoxes[3, i>>4] << 4 | m_SBoxes[2, i&15]);
				m_expandedSBoxes[0,i] = (byte)(m_SBoxes[1, i>>4] << 4 | m_SBoxes[0, i&15]);
			}
		}


		private void _Reset()
		{
			if (m_IV != null) 
			{
				if (m_lastCipherBlock == null) m_lastCipherBlock = new byte[m_blockSizeInBytes];
				Array.Copy(m_IV, 0, m_lastCipherBlock, 0, m_IV.Length);
			}
			m_depadBuffer = null;
		}

		private byte[] _Pad(byte[] buffer, int offset, int count)
		{
			byte[] result = null;

			if (m_padding == PaddingMode.PKCS7) 
			{
				// PKCS7 padding
				int bytes_to_pad = m_blockSizeInBytes-count%m_blockSizeInBytes;
				result = new byte[count+bytes_to_pad];
				Array.Copy(buffer, offset, result, 0, count);
				for(int i=count;i<count+bytes_to_pad;i++) result[i] = (byte)(bytes_to_pad);
			} 
			else 
				if (m_padding == PaddingMode.Zeros) 
			{
				// zeroes padding
				int bytes_to_pad = m_blockSizeInBytes-count%m_blockSizeInBytes;
				if (bytes_to_pad == m_blockSizeInBytes) bytes_to_pad = 0;
				result = new byte[count+bytes_to_pad];
				Array.Copy(buffer, offset, result, 0, count);
				for(int i=count;i<count+bytes_to_pad;i++) result[i] = 0;
			} 
			else 
			{
				result = new byte[count];
				Array.Copy(buffer, offset, result, 0, count);
			}
			return result;
		}

	}

}
