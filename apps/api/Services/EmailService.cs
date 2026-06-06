using Resend;

namespace MobroLens.Services;

public interface IEmailService
{
    Task SendPasswordResetAsync(string toEmail, string code);
}

public class EmailService(IResend resend) : IEmailService
{
    public async Task SendPasswordResetAsync(string toEmail, string code)
    {
        try
        {
            await resend.EmailSendAsync(new EmailMessage
            {
                From = "onboarding@resend.dev",
                To = toEmail,
                Subject = "MobroAI — Password Reset Code",
                HtmlBody = $"<p>Your password reset code is: <strong>{code}</strong></p><p>This code expires in 15 minutes.</p>"
            });
        }
        catch (ResendException)
        {
            // No API key configured — code is still saved in DB, email skipped
        }
    }
}
