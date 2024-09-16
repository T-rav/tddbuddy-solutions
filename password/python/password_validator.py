# password_validator.py

class PasswordValidator:
    def validate(self, password):
        missing_criteria = []

        if len(password) < 8:
            missing_criteria.append("at least 8 characters long")

        if not any(c.isupper() for c in password):
            missing_criteria.append("one uppercase letter")

        if not any(c.islower() for c in password):
            missing_criteria.append("one lowercase letter")

        if not any(c.isdigit() for c in password):
            missing_criteria.append("one number")

        special_characters = "!@#$%^&*()_+"
        if not any(c in special_characters for c in password):
            missing_criteria.append("one special character")

        if not missing_criteria:
            return "Password is valid."

        return "Password must contain " + ", ".join(missing_criteria) + "."
