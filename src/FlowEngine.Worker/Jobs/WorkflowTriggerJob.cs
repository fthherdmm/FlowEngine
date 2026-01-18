using FlowEngine.Application.IntegrationEvents;
using MassTransit;
using Quartz;

namespace FlowEngine.Worker.Jobs
{
    // IJob arayüzü Quartz'dan gelir
    public class WorkflowTriggerJob : IJob
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<WorkflowTriggerJob> _logger;

        public WorkflowTriggerJob(IPublishEndpoint publishEndpoint, ILogger<WorkflowTriggerJob> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Verileri Quartz'ın hafızasından (DataMap) okuyoruz
            var rawId = context.JobDetail.JobDataMap.GetString("WorkflowId");
            var workflowName = context.JobDetail.JobDataMap.GetString("WorkflowName") ?? "İsimsiz Akış"; // <--- İsmi al

            if (Guid.TryParse(rawId, out var workflowId))
            {
                _logger.LogInformation("⏰ [Scheduler] Zamanı geldi! Tetikleniyor... ID: {Id}, İsim: {Name}", workflowId, workflowName);

                // Event'i yeni haliyle (ID + Name) gönderiyoruz
                await _publishEndpoint.Publish(new WorkflowCreatedEvent(workflowId, workflowName));
            }
        }
    }
}