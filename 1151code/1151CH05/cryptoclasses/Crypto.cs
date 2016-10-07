// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/**
 *
 * Crypto.cs
 *
 */

namespace System.Security.Cryptography {
    using System.Security;
    using System.Runtime.Serialization;
    using System;

    // This enum represents cipher chaining modes: cipher block chaining (CBC), 
    // electronic code book (ECB), output feedback (OFB), cipher feedback (CFB),
    // and ciphertext-stealing (CTS).  Not all implementations will support all modes.
    [Serializable]
    public enum CipherMode {            // Please keep with wincrypt.h
        CBC = 1,                        //    CRYPT_MODE_*
        ECB = 2,
        OFB = 3,
        CFB = 4,
        CTS = 5
    }

    // This enum represents the padding method to use for filling out short blocks.
    // "None" means no padding (whole blocks required). 
    // "PKCS7" is the padding mode defined in RFC 2898, Section 6.1.1, Step 4, generalized
    // to whatever block size is required.  
    // "Zeros" means pad with zero bytes to fill out the last block.
    [Serializable]
    public enum PaddingMode {
        None = 1,
        PKCS7 = 2,
        Zeros = 3
    }

    // This structure is used for returning the set of legal key sizes and
    // block sizes of the symmetric algorithms.
    public class KeySizes
    {
        private int          _MinSize;
        private int          _MaxSize;
        private int          _SkipSize;

        public int MinSize {
            get { return(_MinSize); }
        }

        public int MaxSize {
            get { return(_MaxSize); }
        }

        public int SkipSize {
            get { return(_SkipSize); }
        }

        public KeySizes(int minSize, int maxSize, int skipSize) {
            _MinSize = minSize; _MaxSize = maxSize; _SkipSize = skipSize;
        }
    }
    
    [Serializable]
    public class CryptographicException : SystemException
    {
        public CryptographicException() 
            : base(Environment.GetResourceString("Arg_CryptographyException")) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO);
        }
    
        public CryptographicException(String message) 
            : base(message) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO);
        }
    
        public CryptographicException(String format, String insert) 
            : base(String.Format(format, insert)) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO);
        }
    
        public CryptographicException(String message, Exception inner) 
            : base(message, inner) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO);
        }
    
        public CryptographicException(int hr) 
            : base() {
            SetErrorCode(hr);
        }

        protected CryptographicException(SerializationInfo info, StreamingContext context) : base (info, context) {}
    }
    
    [Serializable()]
    public class CryptographicUnexpectedOperationException : CryptographicException
    {
        public CryptographicUnexpectedOperationException() 
            : base() {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO_UNEX_OPER);
        }
    
        public CryptographicUnexpectedOperationException(String message) 
            : base(message) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO_UNEX_OPER);
        }
    
        public CryptographicUnexpectedOperationException(String format, String insert) 
            : base(String.Format(format, insert)) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO_UNEX_OPER);
        }
    
        public CryptographicUnexpectedOperationException(String message, Exception inner) 
            : base(message, inner) {
            SetErrorCode(__HResults.CORSEC_E_CRYPTO_UNEX_OPER);
        }

        protected CryptographicUnexpectedOperationException(SerializationInfo info, StreamingContext context) : base (info, context) {}
    }

}







