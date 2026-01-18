using FlowEngine.Application;
using FlowEngine.Infrastructure;
using FlowEngine.Worker;
using FlowEngine.Worker.Consumers;
using MassTransit;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();

// Veritabanı, Repositoryler ve RabbitMQ ekliyoruz.
builder.Services.AddInfrastructure(builder.Configuration, massTransitConfig => 
{
    massTransitConfig.AddConsumer<WorkflowCreatedConsumer>();
    
    // Kuyruk ismini özel olarak belirtmek istersen (Opsiyonel, yukarıda ConfigureEndpoints otomatik halleder ama elle yazmak güvenlidir)
    // Not: UsingRabbitMq bloğu Infrastructure içinde olduğu için burada tekrar yazmıyoruz, 
    // Consumer'ı eklememiz yeterli, MassTransit gerisini halleder.
});

builder.Services.AddQuartz(q =>
{
    // Quartz DI kullansın.
    q.UseMicrosoftDependencyInjectionJobFactory();
});

// Arka planda Quartz çalışsın 
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});

builder.Services.AddHostedService<FlowEngine.Worker.Services.WorkflowSchedulerLoader>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();