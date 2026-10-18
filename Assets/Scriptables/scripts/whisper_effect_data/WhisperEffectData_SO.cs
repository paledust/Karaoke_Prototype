using System;
using UnityEngine;

namespace WhisperPrototype
{
    public abstract class WhisperEffectData_SO : ScriptableObject
    {
        [SerializeField] protected float duration;
        
        public abstract WhisperEffect GetWhisperEffect();
    }
}
