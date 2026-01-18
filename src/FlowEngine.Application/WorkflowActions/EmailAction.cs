using System.Text.Json;
using FlowEngine.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowEngine.Application.WorkflowActions
{
    public class EmailAction : IWorkflowAction
    {
        private readonly IEmailSender _emailSender;

        public EmailAction(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public string ActionType => "Email";

        public async Task ExecuteAsync(string settingsJson)
        {
            // 1. Ayarları Oku
            var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(settingsJson);
            
            // 2. Verileri Al (Yoksa varsayılan değer ata)
            string to = settings != null && settings.ContainsKey("to") ? settings["to"] : "";
            string subject = settings != null && settings.ContainsKey("subject") ? settings["subject"] : "Konusuz Bildirim";
            string body = settings != null && settings.ContainsKey("body") ? settings["body"] : "İçerik yok.";

            if (string.IsNullOrEmpty(to))
            {
                // Eğer alıcı yoksa hata vermesin, loglayıp geçsin
                return;
            }

            // 3. GERÇEK MAİLİ GÖNDER 
            await _emailSender.SendEmailAsync(to, subject, body);
        }
    }
}