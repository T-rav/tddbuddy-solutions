namespace PasswordValidator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PasswordValidator
    {
        public string Validate(string password)
        {
            var missingCriteria = new List<string>();

            if (password.Length < 8)
                missingCriteria.Add("at least 8 characters long");

            if (!password.Any(char.IsUpper))
                missingCriteria.Add("one uppercase letter");

            if (!password.Any(char.IsLower))
                missingCriteria.Add("one lowercase letter");

            if (!password.Any(char.IsDigit))
                missingCriteria.Add("one number");

            var specialCharacters = "!@#$%^&*()_+";
            if (!password.Any(c => specialCharacters.Contains(c)))
                missingCriteria.Add("one special character");

            if (missingCriteria.Count == 0)
                return "Password is valid.";

            return "Password must contain " + string.Join(", ", missingCriteria) + ".";
        }
    }
}
