using Unity.Services.Analytics;

public class GameRecordEvent : Event
{
    public GameRecordEvent(string gameProcess, int stepsCount) : base("GameRecord")
    {
        SetParameter("gameProcess", gameProcess);
        SetParameter("stepsCount", stepsCount);
    }
}
