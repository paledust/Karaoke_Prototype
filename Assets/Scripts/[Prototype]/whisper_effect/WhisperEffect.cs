using DG.Tweening;
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
        protected static void CreateZone(GameObject zonePrefab, Vector3 pos, float radius, float duration)
        {
            var zoneInstance = GameObject.Instantiate(zonePrefab);
            zoneInstance.transform.position = pos;
            zoneInstance.transform.localScale = Vector3.zero;
            var zoneSeq = DOTween.Sequence();
            zoneSeq.Append(zoneInstance.transform.DOScale(radius * Vector3.one, 0.2f).SetEase(Ease.OutBack))
                    .Append(zoneInstance.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuad)
                            .SetDelay(duration)
                            .OnComplete(()=>GameObject.Destroy(zoneInstance)));
        }
    }
}
