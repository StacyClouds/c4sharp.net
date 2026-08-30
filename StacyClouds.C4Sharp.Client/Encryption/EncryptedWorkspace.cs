using StacyClouds.C4Sharp.IO.Json;
using System.IO;
using System.Runtime.Serialization;

namespace StacyClouds.C4Sharp.Encryption
{

    /// <summary>
    /// Wraps a workspace together with the metadata needed to store it in encrypted form.
    /// </summary>
    [DataContract]
    public class EncryptedWorkspace : AbstractWorkspace
    {

        private Workspace _workspace;
        /// <summary>
        /// Gets the decrypted workspace, materializing it from <see cref="Ciphertext"/> when necessary.
        /// </summary>
        public Workspace Workspace
        {
            get
            {
                if (_workspace != null)
                {
                    return _workspace;
                }
                else if (Ciphertext != null)
                {
                    Plaintext = EncryptionStrategy.Decrypt(Ciphertext);
                    StringReader stringReader = new StringReader(Plaintext);
                    return new JsonReader().Read(stringReader);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                _workspace = value;
            }
        }

        /// <summary>
        /// Describes how the workspace payload is encrypted.
        /// </summary>
        [DataMember(Name = "encryptionStrategy", EmitDefaultValue = false)]
        public EncryptionStrategy EncryptionStrategy { get; internal set; }

        /// <summary>
        /// Stores the decrypted JSON payload while encryption or decryption is in progress.
        /// </summary>
        internal string Plaintext { get; set; }

        /// <summary>
        /// Stores the encrypted workspace JSON payload.
        /// </summary>
        [DataMember(Name = "ciphertext", EmitDefaultValue = false)]
        public string Ciphertext { get; internal set; }

        /// <summary>
        /// Initializes an encrypted workspace placeholder for serializers.
        /// </summary>
        public EncryptedWorkspace() { }

        /// <summary>
        /// Creates an encrypted wrapper around a workspace.
        /// </summary>
        /// <param name="workspace">The workspace to encrypt.</param>
        /// <param name="encryptionStrategy">The strategy used to encrypt the workspace JSON.</param>
        public EncryptedWorkspace(Workspace workspace, EncryptionStrategy encryptionStrategy)
        {
            Workspace = workspace;
            EncryptionStrategy = encryptionStrategy;
            
            Configuration = workspace.Configuration;
            workspace.ClearConfiguration();

            StringWriter stringWriter = new StringWriter();
            JsonWriter jsonWriter = new JsonWriter(false);
            jsonWriter.Write(workspace, stringWriter);

            Id = workspace.Id;
            Name = workspace.Name;
            Description = workspace.Description;
            Version = workspace.Version;
            Revision = workspace.Revision;
            LastModifiedAgent = workspace.LastModifiedAgent;
            LastModifiedUser = workspace.LastModifiedUser;
            Thumbnail = workspace.Thumbnail;

            Plaintext = stringWriter.ToString();
            Ciphertext = encryptionStrategy.Encrypt(Plaintext);
        }

    }
}
