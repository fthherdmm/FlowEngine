using System;

namespace FlowEngine.Domain.Common
{
    // Abstract yapıyoruz çünkü tek başına "BaseEntity" diye bir şey new'lenemez.
    // Ancak bir Workflow veya Step bu sınıftan türetilebilir.
    public abstract class BaseEntity
    {
        // Guid kullanıyoruz çünkü dağıtık sistemlerde (RabbitMQ vs) ID çakışması istemeyiz.
        // DB'ye gitmeden kod tarafında ID oluşturabilmek büyük avantajdır.
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedOn { get; set; }
    }
}