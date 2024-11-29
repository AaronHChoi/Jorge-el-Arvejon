using Unity.Services.Analytics;

public class PlayerWallSlideEvent : Unity.Services.Analytics.Event
{
    public PlayerWallSlideEvent() : base("PlayerWallSlide") { } // Match dashboard schema

    public int Wallslidecount
    {
        set { SetParameter("Wallslidecount", value); }
    }
}
