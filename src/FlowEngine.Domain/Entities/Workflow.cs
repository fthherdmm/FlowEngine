using System;
using System.Collections.Generic;
using System.Linq;
using FlowEngine.Domain.Common;

namespace FlowEngine.Domain.Entities
{
    public class Workflow : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        
        // TriggerType: Bu akış ne zaman çalışacak? (Örn: "OnTimer", "OnWebhook")
        // Şimdilik string tutalım, ileride Enum yapabiliriz.
        public string TriggerType { get; private set; } 
        
        // Trigger ayarları (Örn: "09:00", "Every 5 minutes")
        public string TriggerSettingsJson { get; private set; } 

        // Encapsulation (Kapsülleme): Listeyi dışarıya sadece okunabilir açıyoruz.
        private readonly List<WorkflowStep> _steps = new List<WorkflowStep>();
        public IReadOnlyCollection<WorkflowStep> Steps => _steps.AsReadOnly();

        protected Workflow() { }

        public Workflow(string name, string triggerType, string triggerSettingsJson)
        {
            // Validasyonlar Domain içinde yapılır (Fail Fast Principle)
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            
            Name = name;
            TriggerType = triggerType;
            TriggerSettingsJson = triggerSettingsJson;
            IsActive = true; // Varsayılan aktif
        }

        // Domain Metodu: Adım ekleme işini burada yönetiyoruz.
        public void AddStep(string actionType, string settings, int order)
        {
            // İş Kuralı: Aynı sırada iki işlem olamaz.
            if (_steps.Any(x => x.Order == order))
            {
                throw new InvalidOperationException($"Step with order {order} already exists.");
            }

            var step = new WorkflowStep(actionType, settings, order);
            _steps.Add(step);
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}