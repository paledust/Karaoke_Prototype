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
            zoneInstance = GameObject.Instantiate(zonePrefab);
            zoneInstance.transform.position = castPosition;
            zoneInstance.transform.localScale = Vector3.zero;
            var zoneSeq = DOTween.Sequence();
            zoneSeq.Append(zoneInstance.transform.DOScale(radius * Vector3.one, 0.2f).SetEase(Ease.OutBack))
                    .Append(zoneInstance.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuad)
                            .SetDelay(duration)
                            .OnComplete(()=>GameObject.Destroy(zoneInstance)));
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
