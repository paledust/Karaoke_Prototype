using System;

public static class PlayerSingingEvent
{
    public static event Action<AudioAnalysis.AudioAnalyzer> E_OnPlayerStartToSing;
    public static void Call_OnPlayerStartToSing(AudioAnalysis.AudioAnalyzer analyzer)=>E_OnPlayerStartToSing?.Invoke(analyzer);
    public static event Action E_OnPlayerStopSinging;
    public static void Call_OnPlayerStopSinging()=>E_OnPlayerStopSinging?.Invoke();
}
