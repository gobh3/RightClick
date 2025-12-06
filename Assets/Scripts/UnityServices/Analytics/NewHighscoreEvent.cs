using Unity.Services.Analytics;

public class NewHighscoreEvent : Event
{
    public NewHighscoreEvent(int highscore) : base("NewHighscore")
    {
        SetParameter("NewHighscore", highscore);
    }
}
