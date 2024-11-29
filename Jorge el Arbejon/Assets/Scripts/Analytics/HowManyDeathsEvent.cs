using Unity.Services.Analytics;

public class HowManyDeathsEvent : Unity.Services.Analytics.Event
{
    public HowManyDeathsEvent() : base("HowManyDeaths") { } // Match the dashboard event name

    public int DeathCount // Match the parameter name exactly
    {
        set { SetParameter("DeathCount", value); }
    }
}
