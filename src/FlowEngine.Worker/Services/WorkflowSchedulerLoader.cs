using System.Text.Json;
using FlowEngine.Application.Interfaces.Repositories;
using FlowEngine.Worker.Jobs;
using Quartz;

namespace FlowEngine.Worker.Services
{
    public class WorkflowSchedulerLoader : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<WorkflowSchedulerLoader> _logger;

        public WorkflowSchedulerLoader(
            ISchedulerFactory schedulerFactory, 
            IServiceScopeFactory scopeFactory,
            ILogger<WorkflowSchedulerLoader> logger)
        {
            _schedulerFactory = schedulerFactory;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("⏳ [SchedulerLoader] Zamanlanmış görevler yükleniyor...");

            // Quartz Scheduler'ı al
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            // Repository Scoped olduğu için yeni bir scope açıyoruz
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IWorkflowRepository>();

            // Sadece "Timer" tipli ve Aktif akışları çek
            // Not: Gerçek senaryoda veritabanında "Where TriggerType='Timer'" filtresi daha performanslı olur.
            var allWorkflows = await repository.GetAllAsync();
            var timerWorkflows = allWorkflows.Where(w => w.TriggerType == "Timer" && w.IsActive);

            foreach (var workflow in timerWorkflows)
            {
                try
                {
                    // TriggerSettings içindeki ifadeyi okur.
                    // Beklenen format: { "cron": "0/10 * * * * ?" }
                    var settings = JsonSerializer.Deserialize<Dictionary<string, string>>(workflow.TriggerSettingsJson);
                    
                    if (settings != null && settings.ContainsKey("cron"))
                    {
                        var cronExpression = settings["cron"];

                        // Job Oluştur
                        var job = JobBuilder.Create<WorkflowTriggerJob>()
                            .WithIdentity($"Job_{workflow.Id}", "WorkflowGroup")
                            .UsingJobData("WorkflowId", workflow.Id.ToString())
                            .UsingJobData("WorkflowName", workflow.Name) // <--- İSMİ BURAYA GÖMÜYORUZ
                            .Build();

                        // Tetikleyici (Trigger) Oluştur
                        var trigger = TriggerBuilder.Create()
                            .WithIdentity($"Trigger_{workflow.Id}", "WorkflowGroup")
                            .WithCronSchedule(cronExpression) // CRON zamanlaması
                            .Build();

                        // Zamanlayıcıya ekle
                        await scheduler.ScheduleJob(job, trigger, cancellationToken);
                        
                        _logger.LogInformation("✅ Görev Kuruldu: '{Name}' -> Cron: {Cron}", workflow.Name, cronExpression);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Görev kurulamadı: {Name}", workflow.Name);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}