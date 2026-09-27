using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace FanHubPlus.Services
{
    // Plain POCO bound from the "Smtp" section of appsettings.json.
    public class SmtpSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string FromEmail { get; set; } = "";
        public string FromName { get; set; } = "Fan Hub Plus";
        public bool EnableSsl { get; set; } = true;
    }

    // Real email sender used by ASP.NET Identity for:
    // - Forgot password / reset password links
    // - Email confirmation links (Register / ResendEmailConfirmation)
    // - Change-email confirmation
    //
    // Configure real SMTP credentials under "Smtp" in appsettings.json
    // (Gmail App Password, Outlook, SendGrid SMTP relay, Mailtrap for testing, etc.).
    // If Host/Username are left blank (e.g. on a machine with no SMTP set up),
    // the email is written to the console/log instead of being sent, so the
    // app never crashes — but the actual reset link will only work once
    // Smtp settings are filled in.
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<SmtpSettings> settings, ILogger<EmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            if (string.IsNullOrWhiteSpace(_settings.Host) || string.IsNullOrWhiteSpace(_settings.Username))
            {
                // No SMTP configured yet — log instead of throwing, so Register/ForgotPassword
                // flows still work end-to-end in dev without a mail server.
                _logger.LogWarning(
                    "SMTP is not configured (Smtp:Host/Smtp:Username empty in appsettings.json). " +
                    "Email to {Email} was NOT sent. Subject: {Subject}\nBody:\n{Body}",
                    email, subject, htmlMessage);
                return;
            }

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            message.To.Add(email);

            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.Username, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Don't let a mail-server hiccup break the whole request (e.g. Register/ForgotPassword);
                // log it so it's visible during grading/demo instead of showing a 500 page.
                _logger.LogError(ex, "Failed to send email to {Email}", email);
            }
        }
    }
}
