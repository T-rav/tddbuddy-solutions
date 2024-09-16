import hashlib
import hmac
import datetime
import uuid

class User:
    def __init__(self, username, email, password_hash, password_salt):
        self.username = username
        self.email = email
        self.password_hash = password_hash
        self.password_salt = password_salt
        self.reset_token = None
        self.reset_token_expiry = None

class IUserRepository:
    def get_user_by_username(self, username):
        pass

    def get_user_by_email(self, email):
        pass

    def save_password_reset_token(self, username, token, expiry):
        pass

    def get_password_reset_token(self, username):
        pass

class IEmailService:
    def send_password_reset_email(self, email_address, reset_link):
        pass

class AuthenticationService:
    def __init__(self, user_repository, email_service):
        self.user_repository = user_repository
        self.email_service = email_service

    def are_valid_user_credentials(self, username, password):
        user = self.user_repository.get_user_by_username(username)
        if not user:
            return False

        password_hash = self._hash_password(password, user.password_salt)
        return hmac.compare_digest(password_hash, user.password_hash)

    def send_reset_email(self, email_address):
        user = self.user_repository.get_user_by_email(email_address)
        if not user:
            return

        token = str(uuid.uuid4())
        expiry = datetime.datetime.utcnow() + datetime.timedelta(hours=1)

        self.user_repository.save_password_reset_token(user.username, token, expiry)

        reset_link = f"https://example.com/resetpassword?token={token}"
        self.email_service.send_password_reset_email(email_address, reset_link)

    def _hash_password(self, password, salt):
        return hashlib.pbkdf2_hmac(
            'sha256',
            password.encode(),
            salt,
            100000
        )
