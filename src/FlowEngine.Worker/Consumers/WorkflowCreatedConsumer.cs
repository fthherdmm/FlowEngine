using MassTransit;
using FlowEngine.Application.IntegrationEvents;
using FlowEngine.Application.Interfaces; // Executor Interface'i burada
using FlowEngine.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace FlowEngine.Worker.Consumers
{
    public class WorkflowCreatedConsumer : IConsumer<WorkflowCreatedEvent>
    {
        private readonly ILogger<WorkflowCreatedConsumer> _logger;
        private readonly IWorkflowRepository _repository;
        private readonly IWorkflowExecutor _executor; // <-- YENİ: Motoru çağırıyoruz

        public WorkflowCreatedConsumer(
            ILogger<WorkflowCreatedConsumer> logger, 
            IWorkflowRepository repository,
            IWorkflowExecutor executor) // <-- Constructor'a ekle
        {
            _logger = logger;
            _repository = repository;
            _executor = executor;
        }

        public async Task Consume(ConsumeContext<WorkflowCreatedEvent> context)
        {
            var message = context.Message;
            
            // 1. Veriyi Çek
            var workflow = await _repository.GetByIdAsync(message.Id);

            if (workflow == null)
            {
                _logger.LogError("❌ Kayıt bulunamadı: {Id}", message.Id);
                return; 
            }

            // 2. Motoru Çalıştır (Tüm işi burası yapacak)
            await _executor.ExecuteAsync(workflow);
        }
    }
}