namespace PasswordValidator
{
    public interface IEmailService
    {
        void SendPasswordResetEmail(string emailAddress, string resetLink);
    }
}
