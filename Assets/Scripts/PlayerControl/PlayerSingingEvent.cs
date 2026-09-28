using System;

public static class PlayerWhisperingEvent
{
    public static event Action<AudioAnalysis.AudioAnalyzer> E_OnPlayerStartWhispering;
    public static void Call_OnPlayerStartWhispering(AudioAnalysis.AudioAnalyzer analyzer)=>E_OnPlayerStartWhispering?.Invoke(analyzer);
    public static event Action E_OnPlayerStopWhispering;
    public static void Call_OnPlayerStopWhispering()=>E_OnPlayerStopWhispering?.Invoke();
}
