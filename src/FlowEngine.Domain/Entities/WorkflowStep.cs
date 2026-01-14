using FlowEngine.Domain.Common;

namespace FlowEngine.Domain.Entities
{
    public class WorkflowStep : BaseEntity
    {
        // Private set: Dışarıdan kimse "step.ActionType = ..." diyemez.
        // Sadece constructor veya metotlarla değişebilir.
        public string ActionType { get; private set; } // Örn: "SendEmail", "WriteToDb"
        public string SettingsJson { get; private set; } // O aksiyonun ayarları (Kime mail gidecek vs.)
        public int Order { get; private set; } // Sıralama

        // Entity Framework ve Serialization için boş constructor gereklidir.
        protected WorkflowStep() { }

        public WorkflowStep(string actionType, string settingsJson, int order)
        {
            if (string.IsNullOrWhiteSpace(actionType))
                throw new ArgumentException("Action type cannot be empty.");

            ActionType = actionType;
            SettingsJson = settingsJson;
            Order = order;
        }
    }
}