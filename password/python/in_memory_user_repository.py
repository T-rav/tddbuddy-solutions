from authentication_service import IEmailService, IUserRepository


class InMemoryUserRepository(IUserRepository):
    def __init__(self):
        self.users = {}
        self.reset_tokens = {}

    def add_user(self, user):
        self.users[user.username] = user

    def get_user_by_username(self, username):
        return self.users.get(username)

    def get_user_by_email(self, email):
        for user in self.users.values():
            if user.email == email:
                return user
        return None

    def save_password_reset_token(self, username, token, expiry):
        self.reset_tokens[username] = (token, expiry)

    def get_password_reset_token(self, username):
        return self.reset_tokens.get(username)

class MockEmailService(IEmailService):
    def __init__(self):
        self.sent_emails = []

    def send_password_reset_email(self, email_address, reset_link):
        self.sent_emails.append((email_address, reset_link))
