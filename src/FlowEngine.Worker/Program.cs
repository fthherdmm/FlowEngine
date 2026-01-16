using FlowEngine.Application;
using FlowEngine.Infrastructure;
using FlowEngine.Worker;
using FlowEngine.Worker.Consumers;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddApplication();

// TEK SATIRDA TÜM ALTYAPIYI YÜKLE
// Veritabanı, Repositoryler ve RabbitMQ buradan geliyor.
builder.Services.AddInfrastructure(builder.Configuration, massTransitConfig => 
{
    // Worker'a özel ayar: Consumer'ı ekle
    massTransitConfig.AddConsumer<WorkflowCreatedConsumer>();
    
    // Kuyruk ismini özel olarak belirtmek istersen (Opsiyonel, yukarıda ConfigureEndpoints otomatik halleder ama elle yazmak güvenlidir)
    // Not: UsingRabbitMq bloğu Infrastructure içinde olduğu için burada tekrar yazmıyoruz, 
    // Consumer'ı eklememiz yeterli, MassTransit gerisini halleder.
});

// Worker Servisini Kaydet
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();