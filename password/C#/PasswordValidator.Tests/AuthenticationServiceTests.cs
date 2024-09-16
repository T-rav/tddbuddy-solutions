using NUnit.Framework;
using NSubstitute;
using System;
using System.Text;
using PasswordValidator;

namespace PasswordValidator.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test]
        public void AreValidUserCredentials_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var userName = "testuser";
            var password = "password123";
            var (authService, userRepositoryMock, _) = CreateAuthenticationService();

            // Create salt and hash for the password
            byte[] salt;
            byte[] hash;
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            var user = new User
            {
                UserName = userName,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            userRepositoryMock.GetUserByUserName(userName).Returns(user);

            // Act
            var result = authService.AreValidUserCredentials(userName, password);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void AreValidUserCredentials_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var userName = "testuser";
            var correctPassword = "password123";
            var incorrectPassword = "wrongpassword";
            var (authService, userRepositoryMock, _) = CreateAuthenticationService();

            // Create salt and hash for the correct password
            byte[] salt;
            byte[] hash;
            using (var hmac = new System.Security.Cryptography.HMACSHA256())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(correctPassword));
            }

            var user = new User
            {
                UserName = userName,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            userRepositoryMock.GetUserByUserName(userName).Returns(user);

            // Act
            var result = authService.AreValidUserCredentials(userName, incorrectPassword);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void AreValidUserCredentials_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userName = "nonexistentuser";
            var password = "password123";
            var (authService, userRepositoryMock, _) = CreateAuthenticationService();

            userRepositoryMock.GetUserByUserName(userName).Returns((User)null);

            // Act
            var result = authService.AreValidUserCredentials(userName, password);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void SendResetEmail_ValidEmail_SendsEmail()
        {
            // Arrange
            var emailAddress = "user@example.com";
            var userName = "testuser";
            var (authService, userRepositoryMock, emailServiceMock) = CreateAuthenticationService();

            var user = new User
            {
                UserName = userName,
                Email = emailAddress
            };

            userRepositoryMock.GetUserByEmail(emailAddress).Returns(user);

            string savedToken = null;
            DateTime savedExpiry = default;

            userRepositoryMock
                .When(repo => repo.SavePasswordResetToken(userName, Arg.Any<string>(), Arg.Any<DateTime>()))
                .Do(callInfo =>
                {
                    savedToken = callInfo.ArgAt<string>(1);
                    savedExpiry = callInfo.ArgAt<DateTime>(2);
                });

            string sentEmailAddress = null;
            string sentResetLink = null;

            emailServiceMock
                .When(emailService => emailService.SendPasswordResetEmail(Arg.Any<string>(), Arg.Any<string>()))
                .Do(callInfo =>
                {
                    sentEmailAddress = callInfo.ArgAt<string>(0);
                    sentResetLink = callInfo.ArgAt<string>(1);
                });

            // Act
            authService.SendResetEmail(emailAddress);

            // Assert
            Assert.That(sentEmailAddress, Is.EqualTo(emailAddress));
            Assert.That(savedToken, Is.Not.Null);
            Assert.That(savedExpiry, Is.GreaterThan(DateTime.UtcNow));
            Assert.That(sentResetLink, Contains.Substring(savedToken));
        }

        [Test]
        public void SendResetEmail_InvalidEmail_DoesNotSendEmail()
        {
            // Arrange
            var emailAddress = "invalid@example.com";
            var (authService, userRepositoryMock, emailServiceMock) = CreateAuthenticationService();

            userRepositoryMock.GetUserByEmail(emailAddress).Returns((User)null);

            // Act
            authService.SendResetEmail(emailAddress);

            // Assert
            emailServiceMock.DidNotReceive().SendPasswordResetEmail(Arg.Any<string>(), Arg.Any<string>());
            userRepositoryMock.DidNotReceive().SavePasswordResetToken(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<DateTime>());
        }

        // Factory method to create an instance of AuthenticationService and its dependencies
        private (AuthenticationService authService, IUserRepository userRepositoryMock, IEmailService emailServiceMock) CreateAuthenticationService()
        {
            var userRepositoryMock = Substitute.For<IUserRepository>();
            var emailServiceMock = Substitute.For<IEmailService>();
            var authService = new AuthenticationService(userRepositoryMock, emailServiceMock);

            return (authService, userRepositoryMock, emailServiceMock);
        }
    }
}
