using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperEffect
    {
        protected float duration;
        public WhisperEffect(float duration)
        {
            this.duration = duration;
        }
        internal virtual void InitializeWhisper(WhisperHandler caster, Vector3 castPosition, float radius)
        {
            
        }
        internal virtual void OnWhisperBegin()
        {
            
        }
        internal virtual void OnWhisperEnd()
        {
            
        }
    }
}
