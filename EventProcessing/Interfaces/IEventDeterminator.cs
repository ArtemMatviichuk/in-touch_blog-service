namespace BlogService.EventProcessing.Interfaces
{
    public interface IEventDeterminator
    {
        EventType DetermineEvent(string message);
    }
}
