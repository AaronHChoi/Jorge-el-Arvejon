using Unity.Services.Analytics;

public class PlayerDashEvent : Unity.Services.Analytics.Event
{
    public PlayerDashEvent() : base("PlayerDash") { }

    public int DashCount
    {
        set { SetParameter("DashCount", value); }
    }
}
