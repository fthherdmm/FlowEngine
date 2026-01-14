using System.Collections.Generic;

namespace FlowEngine.Application.DTOs
{
    // Kullanıcıdan Workflow oluştururken isteyeceğimiz veriler sadece bunlar.
    public class CreateWorkflowDto
    {
        public string Name { get; set; }
        public string TriggerType { get; set; }
        public string TriggerSettings { get; set; }
        public List<CreateStepDto> Steps { get; set; }
    }

    public class CreateStepDto
    {
        public string ActionType { get; set; }
        public string Settings { get; set; }
        public int Order { get; set; }
    }
}