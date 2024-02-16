namespace BlogService.EventProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
}