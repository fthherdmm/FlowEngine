using System.Text.Json;
using FlowEngine.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowEngine.Application.WorkflowActions
{
    public class EmailAction : IWorkflowAction
    {
        private readonly ILogger<EmailAction> _logger;

        public EmailAction(ILogger<EmailAction> logger)
        {
            _logger = logger;
        }

        public string ActionType => "Email"; // Bu sınıf "Email" işini yapar

        public Task ExecuteAsync(string settingsJson)
        {
            var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(settingsJson);
            var to = settings?.ContainsKey("to") == true ? settings["to"] : "bilinmiyor";
            
            _logger.LogInformation("📧 [Action: Email] Mail gönderiliyor -> {To}", to);
            
            return Task.CompletedTask;
        }
    }
}
