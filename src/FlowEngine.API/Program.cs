using FlowEngine.Application;       // Bunu eklemeyi unutma
using FlowEngine.Infrastructure;
using FlowEngine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore; // Bunu eklemeyi unutma

var builder = WebApplication.CreateBuilder(args);

// --- TEMİZLENMİŞ ALAN ---
// Artık tek satırda tüm katmanları yüklüyoruz.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
// ------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        // Eğer veritabanı yoksa oluşturur, tablolar eksikse tamamlar
        Console.WriteLine("⏳ Veritabanı tabloları oluşturuluyor (Migration)...");
        context.Database.Migrate(); 
        Console.WriteLine("✅ Veritabanı tabloları başarıyla oluşturuldu!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Veritabanı oluşturulurken hata çıktı: {ex.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();