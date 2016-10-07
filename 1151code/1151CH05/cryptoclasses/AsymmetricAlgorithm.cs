// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/**
 *
 * AsymmetricAlgorithm.cs
 *
 */

namespace System.Security.Cryptography {
    using System.Security;
    using System;
    public abstract class AsymmetricAlgorithm : IDisposable {
		protected int           KeySizeValue;
        protected KeySizes[]    LegalKeySizesValue;

        // *********************** CONSTRUCTORS *************************

        protected AsymmetricAlgorithm() {
        }

        // AsymmetricAlgorithm implements IDisposable

        void IDisposable.Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Clear() {
            ((IDisposable) this).Dispose();
        }

        protected abstract void Dispose(bool disposing);
    
        /************************* Property Methods **********************/
    
        public virtual int KeySize {
            get { return KeySizeValue; }
            set {
                int   i;
                int   j;

                for (i=0; i<LegalKeySizesValue.Length; i++) {
                    if (LegalKeySizesValue[i].SkipSize == 0) {
                        if (LegalKeySizesValue[i].MinSize == value) { // assume MinSize = MaxSize
                            KeySizeValue = value;
                            return;
                        }
                    } else {
                        for (j = LegalKeySizesValue[i].MinSize; j<=LegalKeySizesValue[i].MaxSize;
                             j += LegalKeySizesValue[i].SkipSize) {
                            if (j == value) {
                                KeySizeValue = value;
                                return;
                            }
                        }
                    }
                }
                throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidKeySize"));
            }
        }
        
        public virtual KeySizes[] LegalKeySizes { 
            get { return LegalKeySizesValue; }
        }

        public abstract String SignatureAlgorithm {
            get;
        }

        public abstract String KeyExchangeAlgorithm {
            get;
        }
        
        /************************* PUBLIC METHODS ************************/

        static public AsymmetricAlgorithm Create() {
            // Use the crypto config system to return an instance of
            // the default AsymmetricAlgorithm on this machine
            return Create("System.Security.Cryptography.AsymmetricAlgorithm");
        }

        static public AsymmetricAlgorithm Create(String algName) {
            return (AsymmetricAlgorithm) CryptoConfig.CreateFromName(algName);
        }

        public abstract void FromXmlString(String xmlString);
        public abstract String ToXmlString(bool includePrivateParameters);

        // ** Internal Utility Functions ** //

        internal static String DiscardWhiteSpaces(String inputBuffer) {
            return DiscardWhiteSpaces(inputBuffer, 0, inputBuffer.Length);
        }

        internal static String DiscardWhiteSpaces(String inputBuffer, int inputOffset, int inputCount) {
            int i, iCount = 0;
            for (i=0; i<inputCount; i++)
                if (Char.IsWhiteSpace(inputBuffer[inputOffset + i])) iCount++;
            char[] rgbOut = new char[inputCount - iCount];
            iCount = 0;
            for (i=0; i<inputCount; i++)
                if (!Char.IsWhiteSpace(inputBuffer[inputOffset + i])) {
                    rgbOut[iCount++] = inputBuffer[inputOffset + i];
                }
            return new String(rgbOut);
        }

    }
}    
