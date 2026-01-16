using System.Text.Json;
using FlowEngine.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace FlowEngine.Application.WorkflowActions
{
    public class LogAction : IWorkflowAction
    {
        private readonly ILogger<LogAction> _logger;

        public LogAction(ILogger<LogAction> logger)
        {
            _logger = logger;
        }

        public string ActionType => "Log"; // Bu sınıf "Log" işini yapar

        public Task ExecuteAsync(string settingsJson)
        {
            var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(settingsJson);
            var msg = settings?.ContainsKey("message") == true ? settings["message"] : "";

            _logger.LogInformation("📝 [Action: Log] Sistem günlüğüne yazıldı: {Msg}", msg);
            
            return Task.CompletedTask;
        }
    }
}