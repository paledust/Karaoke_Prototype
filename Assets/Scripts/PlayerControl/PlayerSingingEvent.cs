using System;

public static class PlayerSingingEvent
{
    public static event Action E_OnPlayerStartToSing;
    public static void Call_OnPlayerStartToSing()=>E_OnPlayerStartToSing?.Invoke();
    public static event Action E_OnPlayerStopSinging;
    public static void Call_OnPlayerStopSinging()=>E_OnPlayerStopSinging?.Invoke();
}
