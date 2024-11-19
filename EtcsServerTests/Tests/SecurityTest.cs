using EtcsServer.DriverAppDto;
using EtcsServer.InMemoryData.Contract;
using EtcsServer.Security;
using EtcsServerTests.Helpers;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtcsServerTests.Tests
{
    public class SecurityTest
    {
        private ISecurityManager securityManager;

        public SecurityTest()
        {
            securityManager = new TestServiceProvider().GetService<ISecurityManager>();
        }

        [Theory]
        [InlineData("test")]
        [InlineData("Text with spaces, (special signs) and 1 number")]
        [InlineData("")]
        public void ISecurityManager_Encrypt_SimpleString(string plaintext)
        {
            //When
            string ciphertext = securityManager.Encrypt(plaintext);
            string decrypted = securityManager.Decrypt(ciphertext);

            //Then
            Assert.Equal(plaintext, decrypted);
        }

        [Fact]
        public void ISecurityManager_Encrypt_MovementAuthorityEncryption()
        {
            //Given
            MovementAuthority movementAuthority = new()
            {
                Speeds = [200, 0],
                SpeedDistances = [0, 1500],
                Gradients = [20],
                GradientsDistances = [0, 1500],
                Lines = [1],
                LinesDistances = [0, 1500],
                Messages = [],
                MessagesDistances = [],
                ServerPosition = 0.5
            };

            //When
            string ciphertext = securityManager.Encrypt(movementAuthority);
            MovementAuthority decrypted = securityManager.Decrypt<MovementAuthority>(ciphertext);

            //Then
            Assert.Equal(movementAuthority, decrypted);
        }
    }
}
