using System;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    public static class StatueEvent
    {
        public static event Action<Statue> E_OnStatueSense;
        public static void Call_OnStatueSense(Statue statue)=>E_OnStatueSense?.Invoke(statue);
    }
}
