import os
import datetime
from authentication_service import AuthenticationService, User
from in_memory_user_repository import InMemoryUserRepository, MockEmailService

def test_given_valid_credentials_when_are_valid_user_credentials_then_returns_true():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    password = "password123"
    salt = os.urandom(16)
    password_hash = auth_service._hash_password(password, salt)
    user = User("testuser", "user@example.com", password_hash, salt)
    user_repository.add_user(user)
    
    # Act
    result = auth_service.are_valid_user_credentials("testuser", "password123")
    
    # Assert
    assert result is True

def test_given_invalid_password_when_are_valid_user_credentials_then_returns_false():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    password = "password123"
    salt = os.urandom(16)
    password_hash = auth_service._hash_password(password, salt)
    user = User("testuser", "user@example.com", password_hash, salt)
    user_repository.add_user(user)
    
    # Act
    result = auth_service.are_valid_user_credentials("testuser", "wrongpassword")
    
    # Assert
    assert result is False

def test_given_nonexistent_user_when_are_valid_user_credentials_then_returns_false():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    # Act
    result = auth_service.are_valid_user_credentials("nonexistent", "password123")
    
    # Assert
    assert result is False

def test_given_valid_email_when_send_reset_email_then_sends_email():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    password = "password123"
    salt = os.urandom(16)
    password_hash = auth_service._hash_password(password, salt)
    user = User("testuser", "user@example.com", password_hash, salt)
    user_repository.add_user(user)
    
    # Act
    auth_service.send_reset_email("user@example.com")
    
    # Assert
    # Check that an email was sent
    assert len(email_service.sent_emails) == 1
    email_address, reset_link = email_service.sent_emails[0]
    assert email_address == "user@example.com"
    
    # Check that the reset link contains the token
    token_expiry = user_repository.get_password_reset_token("testuser")
    assert token_expiry is not None
    token, expiry = token_expiry
    assert token in reset_link
    assert expiry > datetime.datetime.utcnow()

def test_given_invalid_email_when_send_reset_email_then_does_not_send_email():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    # Act
    auth_service.send_reset_email("invalid@example.com")
    
    # Assert
    # Check that no email was sent
    assert len(email_service.sent_emails) == 0

def test_given_null_username_when_are_valid_user_credentials_then_returns_false():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    # Act
    result = auth_service.are_valid_user_credentials(None, "password123")
    
    # Assert
    assert result is False

def test_given_empty_password_when_are_valid_user_credentials_then_returns_false():
    # Arrange
    user_repository = InMemoryUserRepository()
    email_service = MockEmailService()
    auth_service = AuthenticationService(user_repository, email_service)
    
    # Add a user to the repository
    password = "password123"
    salt = os.urandom(16)
    password_hash = auth_service._hash_password(password, salt)
    user = User("testuser", "user@example.com", password_hash, salt)
    user_repository.add_user(user)
    
    # Act
    result = auth_service.are_valid_user_credentials("testuser", "")
    
    # Assert
    assert result is False

