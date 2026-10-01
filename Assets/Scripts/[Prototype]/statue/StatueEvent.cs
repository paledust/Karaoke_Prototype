using System;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    public static class StatueEvent
    {
        public static event Action<StatueHarmonyWithPlayer> E_OnStatueSense;
        public static void Call_OnStatueSense(StatueHarmonyWithPlayer statue)=>E_OnStatueSense?.Invoke(statue);
    }
}
