using FlowEngine.Application.Interfaces;
using MailKit.Net.Smtp; // MailKit kütüphanesi
using MimeKit; // Mesaj oluşturmak için
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlowEngine.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration config, ILogger<EmailSender> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                
                // Kimden gidiyor?
                email.From.Add(new MailboxAddress(
                    _config["EmailSettings:SenderName"], 
                    _config["EmailSettings:SenderEmail"]));
                
                // Kime gidiyor?
                email.To.Add(MailboxAddress.Parse(to));
                
                // Konu ve İçerik
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                // SMTP Bağlantısı (MailKit)
                using var smtp = new SmtpClient();
                
                // Gmail için genelde Port 587 ve StartTls kullanılır
                await smtp.ConnectAsync(
                    _config["EmailSettings:SmtpServer"], 
                    int.Parse(_config["EmailSettings:Port"]), 
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                // Giriş yap
                await smtp.AuthenticateAsync(
                    _config["EmailSettings:SenderEmail"], 
                    _config["EmailSettings:Password"]
                );

                // Gönder
                await smtp.SendAsync(email);
                
                // Bağlantıyı kes
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("📧 Email başarıyla gönderildi: {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Email gönderilirken hata oluştu! Kime: {To}", to);
                // Hata fırlatmıyoruz, akış durmasın diye loglayıp geçiyoruz (veya fırlatabilirsin)
                throw; 
            }
        }
    }
}