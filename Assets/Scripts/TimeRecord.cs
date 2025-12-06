public class TimeRecord
{
    public float totalTime { get; set; }
    public float timeElapsed { get; set; }

    public ITimerClient client { get; set; }

    public TimeRecord(float duration, ITimerClient c)
    {
        totalTime = duration;
        client = c;
        timeElapsed = 0;
    }
}