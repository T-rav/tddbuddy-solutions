from password_validator import PasswordValidator

def test_given_valid_password_when_validate_then_returns_valid_message():
    # Arrange
    validator = PasswordValidator()
    password = "Password1!"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    assert result == "Password is valid."

def test_given_password_too_short_when_validate_then_returns_length_error_message():
    # Arrange
    validator = PasswordValidator()
    password = "Pass1!"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    assert result == "Password must contain at least 8 characters long."

def test_given_password_missing_uppercase_when_validate_then_returns_uppercase_error_message():
    # Arrange
    validator = PasswordValidator()
    password = "password1!"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    assert result == "Password must contain one uppercase letter."

def test_given_password_missing_lowercase_when_validate_then_returns_lowercase_error_message():
    # Arrange
    validator = PasswordValidator()
    password = "PASSWORD1!"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    assert result == "Password must contain one lowercase letter."

def test_given_password_missing_number_when_validate_then_returns_number_error_message():
    # Arrange
    validator = PasswordValidator()
    password = "Password!"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    assert result == "Password must contain one number."

def test_given_password_missing_special_char_when_validate_then_returns_special_char_error_message():
    # Arrange
    validator = PasswordValidator()
    password = "Password1"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    assert result == "Password must contain one special character."

def test_given_password_missing_multiple_criteria_when_validate_then_returns_combined_error_message():
    # Arrange
    validator = PasswordValidator()
    password = "password"
    
    # Act
    result = validator.validate(password)
    
    # Assert
    expected_message = "Password must contain one uppercase letter, one number, one special character."
    assert result == expected_message
