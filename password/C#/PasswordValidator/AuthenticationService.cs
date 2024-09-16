namespace PasswordValidator
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class AuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public AuthenticationService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public bool AreValidUserCredentials(string userName, string password)
        {
            var user = _userRepository.GetUserByUserName(userName);
            if (user == null)
                return false;

            using (var hmac = new HMACSHA256(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != user.PasswordHash[i]) return false;
                }
            }

            return true;
        }

        public void SendResetEmail(string emailAddress)
        {
            var user = _userRepository.GetUserByEmail(emailAddress);
            if (user == null)
                return;

            // Generate a token
            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddHours(1);

            // Save the token with expiry
            _userRepository.SavePasswordResetToken(user.UserName, token, expiry);

            // Generate the reset link
            var resetLink = $"https://example.com/resetpassword?token={token}";

            // Send the email
            _emailService.SendPasswordResetEmail(emailAddress, resetLink);
        }
    }
}
