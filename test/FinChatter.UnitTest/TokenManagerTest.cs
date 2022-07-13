using FinChatter.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace FinChatter.UnitTest
{
    [TestClass]
    public class TokenManagerTest
    {
        private readonly ITokenManager _tokenManager;

        public TokenManagerTest()
        {
            _tokenManager = SetupSetting.TokenManager;
        }

        [TestMethod]
        public async Task GenerateTokenSuccess()
        {
            var tokenResult = await _tokenManager.GenerateToken(_tokenManager.IssuerSigningKey, 2, null);
            Assert.IsNotNull(tokenResult);
            Assert.IsTrue(tokenResult.IsSuccess);
            Assert.IsTrue(!string.IsNullOrEmpty(tokenResult.Data));
        }
    }
}
