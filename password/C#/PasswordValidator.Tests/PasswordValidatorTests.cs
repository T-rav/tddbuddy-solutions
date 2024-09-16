using NUnit.Framework;
using PasswordValidator;

namespace PasswordValidator.Tests
{
    [TestFixture]
    public class PasswordValidatorTests
    {
        [Test]
        public void Validate_ValidPassword_ReturnsValidMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("Password1!");

            // Assert
            Assert.That(result, Is.EqualTo("Password is valid."));
        }

        [Test]
        public void Validate_PasswordTooShort_ReturnsLengthErrorMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("Pass1!");

            // Assert
            Assert.That(result, Is.EqualTo("Password must contain at least 8 characters long."));
        }

        [Test]
        public void Validate_PasswordMissingUppercase_ReturnsUppercaseErrorMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("password1!");

            // Assert
            Assert.That(result, Is.EqualTo("Password must contain one uppercase letter."));
        }

        [Test]
        public void Validate_PasswordMissingLowercase_ReturnsLowercaseErrorMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("PASSWORD1!");

            // Assert
            Assert.That(result, Is.EqualTo("Password must contain one lowercase letter."));
        }

        [Test]
        public void Validate_PasswordMissingNumber_ReturnsNumberErrorMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("Password!");

            // Assert
            Assert.That(result, Is.EqualTo("Password must contain one number."));
        }

        [Test]
        public void Validate_PasswordMissingSpecialChar_ReturnsSpecialCharErrorMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("Password1");

            // Assert
            Assert.That(result, Is.EqualTo("Password must contain one special character."));
        }

        [Test]
        public void Validate_PasswordMissingMultipleCriteria_ReturnsCombinedErrorMessage()
        {
            // Arrange
            var validator = CreatePasswordValidator();

            // Act
            var result = validator.Validate("password");

            // Assert
            Assert.That(result, Is.EqualTo("Password must contain one uppercase letter, one number, one special character."));
        }

        // Factory method to create an instance of PasswordValidator
        private PasswordValidator CreatePasswordValidator()
        {
            return new PasswordValidator();
        }
    }
}
