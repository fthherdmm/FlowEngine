using FlowEngine.Domain.Common;
using FlowEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowEngine.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        // Constructor: Ayarları (Connection String vb.) dışarıdan alır.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tablolarımız
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DDD Entity Ayarları (Fluent API)
            // Attributes yerine Fluent API kullanmak best-practice'tir. Domain sınıfını kirletmeyiz.

            // --- Workflow Ayarları ---
            modelBuilder.Entity<Workflow>(builder =>
            {
                builder.HasKey(w => w.Id); // Primary Key

                // ÖNEMLİ: Domain'de "_steps" private listesini EF Core'a tanıtıyoruz.
                // EF Core, "Steps" property'sine erişemezse "_steps" field'ını kullanacak.
                var navigation = builder.Metadata.FindNavigation(nameof(Workflow.Steps));
                navigation?.SetPropertyAccessMode(PropertyAccessMode.Field); 
                
                // İlişki Tanımı: Bir Workflow silinirse, adımları da silinsin (Cascade Delete)
                builder.HasMany(w => w.Steps)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // --- Step Ayarları ---
            modelBuilder.Entity<WorkflowStep>(builder =>
            {
                builder.HasKey(s => s.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}