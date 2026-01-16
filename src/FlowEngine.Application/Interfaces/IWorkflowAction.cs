namespace FlowEngine.Application.Interfaces
{
    public interface IWorkflowAction
    {
        // Bu action hangi tipi karşılıyor? (Örn: "Email")
        string ActionType { get; }

        // Action'ın yaptığı asıl iş
        Task ExecuteAsync(string settingsJson);
    }
}