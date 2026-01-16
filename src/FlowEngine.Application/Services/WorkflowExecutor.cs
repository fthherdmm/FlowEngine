using FlowEngine.Application.Interfaces;
using FlowEngine.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FlowEngine.Application.Services
{
    public class WorkflowExecutor : IWorkflowExecutor
    {
        private readonly ILogger<WorkflowExecutor> _logger;
        private readonly IEnumerable<IWorkflowAction> _actions; // TÜM ACTION'LAR BURAYA GELİR

        // Constructor'da sistemdeki tüm IWorkflowAction implementasyonlarını istiyoruz.
        public WorkflowExecutor(ILogger<WorkflowExecutor> logger, IEnumerable<IWorkflowAction> actions)
        {
            _logger = logger;
            _actions = actions;
        }

        public async Task ExecuteAsync(Workflow workflow)
        {
            _logger.LogInformation("⚙️ [Engine] '{Name}' çalıştırılıyor...", workflow.Name);

            foreach (var step in workflow.Steps.OrderBy(s => s.Order))
            {
                try
                {
                    // STRATEGY PATTERN:
                    // Listeden, şu anki adımın ActionType'ına (örn: "Email") uyan sınıfı bul.
                    var action = _actions.FirstOrDefault(x => x.ActionType == step.ActionType);

                    if (action == null)
                    {
                        _logger.LogWarning("⚠️ Tanımsız Action Tipi: {Type}", step.ActionType);
                        continue;
                    }

                    _logger.LogInformation("👉 [Adım {Order}] {Type} çalıştırılıyor...", step.Order, step.ActionType);

                    // İşini yapması için Action sınıfına devret
                    await action.ExecuteAsync(step.SettingsJson);
                    
                    await Task.Delay(500);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Hata: {Step}", step.ActionType);
                    throw;
                }
            }
            _logger.LogInformation("🏁 Tamamlandı.");
        }
    }
}