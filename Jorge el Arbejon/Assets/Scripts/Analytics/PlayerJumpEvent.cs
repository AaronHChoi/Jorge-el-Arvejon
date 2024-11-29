using Unity.Services.Analytics;

public class PlayerJumpEvent : Unity.Services.Analytics.Event
{
    public PlayerJumpEvent() : base("PlayerJump") { }

    public int JumpCount
    {
        set { SetParameter("jumpCount", value); }
    }
}

