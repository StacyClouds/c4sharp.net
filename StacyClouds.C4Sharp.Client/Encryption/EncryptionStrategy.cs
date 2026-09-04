using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Encryption
{

    /// <summary>
    /// Defines the contract for client-side workspace encryption strategies.
    /// </summary>
    [DataContract]
    public abstract class EncryptionStrategy
    {

        /// <summary>
        /// Identifies the serialized encryption strategy type.
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public abstract string Type { get; }

        /// <summary>
        /// Supplies the passphrase used by the encryption strategy.
        /// </summary>
        public string Passphrase { get; set; }

        /// <summary>
        /// Identifies the encryption location expected by Structurizr.
        /// </summary>
        [DataMember(Name = "location", EmitDefaultValue = false)]
        public string Location
        {
            get
            {
                return "Client";
            }
        }

        /// <summary>
        /// Initializes an empty encryption strategy for serializers.
        /// </summary>
        public EncryptionStrategy() { }

        /// <summary>
        /// Initializes an encryption strategy with a passphrase.
        /// </summary>
        /// <param name="passphrase">The passphrase used by the strategy.</param>
        public EncryptionStrategy(string passphrase)
        {
            this.Passphrase = passphrase;
        }

        /// <summary>
        /// Encrypts plaintext content.
        /// </summary>
        /// <param name="plaintext">The plaintext to encrypt.</param>
        /// <returns>The encrypted payload.</returns>
        public abstract string Encrypt(string plaintext);
        /// <summary>
        /// Decrypts previously encrypted content.
        /// </summary>
        /// <param name="ciphertext">The encrypted payload.</param>
        /// <returns>The decrypted plaintext.</returns>
        public abstract string Decrypt(string ciphertext);

    }
}
