using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace StacyClouds.C4Sharp.Encryption
{
    /// <summary>
    /// Encrypts workspace JSON using AES/CBC with PBKDF2-derived keys.
    /// </summary>
    [DataContract]
    public class AesEncryptionStrategy : EncryptionStrategy
    {

        private const int InitializationVectorSizeInBytes = 16;
        private static readonly HashAlgorithmName LegacyPbkdf2HashAlgorithm = HashAlgorithmName.SHA1;

        /// <summary>
        /// Identifies this strategy as AES-based encryption.
        /// </summary>
        public override string Type
        {
            get
            {
                return "aes";
            }
        }

        /// <summary>
        /// The AES key size in bits.
        /// </summary>
        [DataMember(Name = "keySize", EmitDefaultValue = false)]
        public int KeySize { get; private set; }

        /// <summary>
        /// The PBKDF2 iteration count used to derive the encryption key.
        /// </summary>
        [DataMember(Name = "iterationCount", EmitDefaultValue = false)]
        public int IterationCount { get; private set; }

        /// <summary>
        /// The hexadecimal salt used during key derivation.
        /// </summary>
        [DataMember(Name = "salt", EmitDefaultValue = false)]
        public string Salt { get; private set; }

        /// <summary>
        /// The hexadecimal initialization vector used during encryption.
        /// </summary>
        [DataMember(Name = "iv", EmitDefaultValue = false)]
        public string Iv { get; private set; }

        /// <summary>
        /// Initializes an empty AES strategy for serializers.
        /// </summary>
        public AesEncryptionStrategy() { }

        /// <summary>
        /// Creates an AES strategy with default key derivation settings.
        /// </summary>
        /// <param name="passphrase">The passphrase used to derive the encryption key.</param>
        public AesEncryptionStrategy(string passphrase) : this(128, 1000, passphrase) { }

        /// <summary>
        /// Creates an AES strategy with generated salt and IV values.
        /// </summary>
        /// <param name="keySize">The AES key size in bits.</param>
        /// <param name="iterationCount">The PBKDF2 iteration count.</param>
        /// <param name="passphrase">The passphrase used to derive the encryption key.</param>
        public AesEncryptionStrategy(int keySize, int iterationCount, string passphrase) : base(passphrase)
        {
            KeySize = keySize;
            IterationCount = iterationCount;

            // create a random salt
            byte[] saltAsBytes = CreateRandomBytes(keySize / 8);
            Salt = BitConverter.ToString(saltAsBytes).Replace("-", "");

            byte[] ivAsBytes = CreateRandomBytes(InitializationVectorSizeInBytes);
            Iv = BitConverter.ToString(ivAsBytes).Replace("-", "");
        }
        
        private byte[] CreateRandomBytes(int bits)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[bits];
                random.GetBytes(bytes);

                return bytes;
            }
        }

        /// <summary>
        /// Creates an AES strategy from persisted encryption parameters.
        /// </summary>
        /// <param name="keySize">The AES key size in bits.</param>
        /// <param name="iterationCount">The PBKDF2 iteration count.</param>
        /// <param name="salt">The hexadecimal salt.</param>
        /// <param name="iv">The hexadecimal initialization vector.</param>
        /// <param name="passphrase">The passphrase used to derive the encryption key.</param>
        public AesEncryptionStrategy(int keySize, int iterationCount, string salt, string iv, string passphrase) : base(passphrase)
        {
            this.KeySize = keySize;
            this.IterationCount = iterationCount;
            this.Salt = salt;
            this.Iv = iv;
        }

        /// <summary>
        /// Decrypts ciphertext previously produced by this strategy.
        /// </summary>
        /// <param name="ciphertext">The Base64-encoded ciphertext.</param>
        /// <returns>The decrypted plaintext.</returns>
        public override string Decrypt(string ciphertext)
        {
            string plaintext;
            byte[] decryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = 128;
                    aes.Key = DeriveKeyBytes(aes.KeySize / 8);
                    aes.IV = hexStringToByteArray(Iv);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] bytesToBeDecrypted = Convert.FromBase64String(ciphertext);
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    }

                    decryptedBytes = ms.ToArray();
                    plaintext = Encoding.UTF8.GetString(decryptedBytes);
                }
            }

            return plaintext;
        }

        /// <summary>
        /// Encrypts plaintext using the configured AES parameters.
        /// </summary>
        /// <param name="plaintext">The plaintext to encrypt.</param>
        /// <returns>The Base64-encoded ciphertext.</returns>
        public override string Encrypt(string plaintext)
        {
            string ciphertext = null;
            byte[] encryptedBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = KeySize;
                    aes.BlockSize = 128;
                    aes.Key = DeriveKeyBytes(aes.KeySize / 8);
                    aes.IV = hexStringToByteArray(Iv);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] plaintextAsBytes = Encoding.UTF8.GetBytes(plaintext);
                        cs.Write(plaintextAsBytes, 0, plaintextAsBytes.Length);
                    }
                    encryptedBytes = ms.ToArray();
                    ciphertext = Convert.ToBase64String(encryptedBytes);
                }
            }

            return ciphertext;
        }

        private byte[] hexStringToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string byteValue = hex.Substring(i * 2, 2);
                bytes[i] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return bytes;
        }

        private byte[] DeriveKeyBytes(int keySizeInBytes)
        {
            return Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(Passphrase),
                hexStringToByteArray(Salt),
                IterationCount,
                LegacyPbkdf2HashAlgorithm,
                keySizeInBytes);
        }

    }
}