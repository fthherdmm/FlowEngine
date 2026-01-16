using System;

namespace FlowEngine.Application.IntegrationEvents
{
    // Record kullanıyoruz çünkü event'ler değişmez (immutable) veri paketleridir.
    public record WorkflowCreatedEvent(Guid Id, string Name);
}