using System;
using UnityEngine;

namespace WhisperPrototype
{
    public abstract class WhisperEffectData_SO : ScriptableObject
    {
        [SerializeField] protected float duration;
        [SerializeField] protected GameObject whisper_zone_prefab;
        
        public abstract WhisperEffect GetWhisperEffect();
    }
}
