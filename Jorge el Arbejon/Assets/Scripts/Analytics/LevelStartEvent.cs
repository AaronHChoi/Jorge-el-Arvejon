using Unity.Services.Analytics;

public class LevelStartEvent : Unity.Services.Analytics.Event
{
    public LevelStartEvent() : base("LevelStart") { }

    public string LevelName
    {
        set { SetParameter("levelName", value); }
    }

    public int LevelIndex
    {
        set { SetParameter("levelIndex", value); }
    }
}
