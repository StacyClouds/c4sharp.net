using StacyClouds.C4Sharp.Encryption;
using Xunit;

namespace StacyClouds.C4Sharp.Api.Encryption.Tests
{
    public class EncryptionStrategyTests
    {
        [Fact]
        public void Test_Location_ReturnsClient()
        {
            var strategy = new AesEncryptionStrategy("password");

            Assert.Equal("Client", strategy.Location);
        }

        [Fact]
        public void Test_Constructor_SetsPassphrase()
        {
            var strategy = new AesEncryptionStrategy("password");

            Assert.Equal("password", strategy.Passphrase);
        }
    }
}
