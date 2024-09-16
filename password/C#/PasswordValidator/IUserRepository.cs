namespace PasswordValidator
{
    using System;

    public interface IUserRepository
    {
        User? GetUserByUserName(string userName);
        User? GetUserByEmail(string email);
        void SavePasswordResetToken(string userName, string token, DateTime expiry);
        (string Token, DateTime Expiry) GetPasswordResetToken(string userName);
    }
}
