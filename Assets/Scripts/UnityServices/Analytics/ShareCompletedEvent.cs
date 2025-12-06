using Unity.Services.Analytics;

public class ShareCompletedEvent : Event
{
    public ShareCompletedEvent(int shareCount) : base("ShareCompleted")
    {
        SetParameter("ShareCount", shareCount);
    }
}
