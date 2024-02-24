namespace BlogService.EventProcessing.Interfaces
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
}
