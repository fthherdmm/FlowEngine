using System;

namespace FlowEngine.Application.IntegrationEvents
{
    // Record kullanıyoruz çünkü event'ler değişmez (immutable) veri paketleridir.
    public class WorkflowCreatedEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // <-- Yeni alan

        public WorkflowCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}