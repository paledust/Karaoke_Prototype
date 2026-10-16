using DG.Tweening;
using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperSilence:WhisperEffect
    {
        protected GameObject zonePrefab;
        protected GameObject zoneInstance;
        protected Sequence zoneSeq;
        
        public WhisperSilence(float duration, GameObject zonePrefab):base(duration)
        {
            this.zonePrefab = zonePrefab;
        }
        internal override void InitializeWhisper(WhisperHandler caster, Vector3 castPosition, float radius)
        {
            CreateZone(zonePrefab, castPosition, radius, duration);
        }
    }   
    [CreateAssetMenu(fileName = "WhisperEffectSilent", menuName = "Whisper_Word/Effect/WhisperEffectSilent")]
    public class WhisperEffectSilent : WhisperEffectData_SO
    {
        public override WhisperEffect GetWhisperEffect()
        {
            return new WhisperSilence(duration, whisper_zone_prefab);
        }
    }
}
