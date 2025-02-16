using Unity.Services.Analytics;

public class LevelsPassedEvent : Unity.Services.Analytics.Event
{
    public LevelsPassedEvent() : base("LevelsPassedCount") { } // Sigue siendo LevelsPassedCount

    public int TotalLevelsPassed
    {
        set { SetParameter("total_levels_passed", value); } 
    }
}