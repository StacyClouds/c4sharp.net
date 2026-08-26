using System.IO;
using StacyClouds.C4Sharp.Encryption;
using Xunit;

namespace StacyClouds.C4Sharp.Api.Encryption.Tests
{
    public class EncryptedJsonSerializationTests
    {
        [Fact]
        public void Test_EncryptedJsonWriter_WritesIndentedJson_WhenIndentOutputIsTrue()
        {
            var workspace = new Workspace("Name", "Description");
            var strategy = new AesEncryptionStrategy(128, 1000, "06DC30A48ADEEE72D98E33C2CEAEAD3E", "ED124530AF64A5CAD8EF463CF5628434", "password");
            var encryptedWorkspace = new EncryptedWorkspace(workspace, strategy);
            var writer = new StringWriter();

            new EncryptedJsonWriter(true).Write(encryptedWorkspace, writer);
            var json = writer.ToString();

            Assert.Contains("\n  \"encryptionStrategy\":", json);
            Assert.Contains("\"ciphertext\":", json);
            Assert.Contains("\"encryptionStrategy\":", json);
        }

        [Fact]
        public void Test_EncryptedJsonWriter_WritesCompactJson_WhenIndentOutputIsFalse()
        {
            var workspace = new Workspace("Name", "Description");
            var strategy = new AesEncryptionStrategy(128, 1000, "06DC30A48ADEEE72D98E33C2CEAEAD3E", "ED124530AF64A5CAD8EF463CF5628434", "password");
            var encryptedWorkspace = new EncryptedWorkspace(workspace, strategy);
            var writer = new StringWriter();

            new EncryptedJsonWriter(false).Write(encryptedWorkspace, writer);
            var json = writer.ToString();

            Assert.DoesNotContain("\n  \"encryptionStrategy\":", json);
            Assert.Contains("\"ciphertext\":", json);
            Assert.Contains("\"encryptionStrategy\":", json);
        }

        [Fact]
        public void Test_EncryptedJsonReader_ReadsEncryptedWorkspace_WithAesStrategy()
        {
            var workspace = new Workspace("Name", "Description");
            var strategy = new AesEncryptionStrategy(128, 1000, "06DC30A48ADEEE72D98E33C2CEAEAD3E", "ED124530AF64A5CAD8EF463CF5628434", "password");
            var encryptedWorkspace = new EncryptedWorkspace(workspace, strategy);
            var serialized = new StringWriter();
            new EncryptedJsonWriter(false).Write(encryptedWorkspace, serialized);

            var reader = new EncryptedJsonReader();
            var deserialized = reader.Read(new StringReader(serialized.ToString()));

            Assert.NotNull(deserialized);
            Assert.Equal(encryptedWorkspace.Ciphertext, deserialized.Ciphertext);
            Assert.IsType<AesEncryptionStrategy>(deserialized.EncryptionStrategy);
            Assert.Equal("aes", deserialized.EncryptionStrategy.Type);
        }
    }
}
