using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class EnemiesKilledEvent : Unity.Services.Analytics.Event
{
    public EnemiesKilledEvent() : base("EnemiesKilledInSession") { }

    public int TotalEnemiesKilled
    {
        set { SetParameter("total_enemies_killed", value); }
    }
}