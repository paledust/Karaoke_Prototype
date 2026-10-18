using System.Collections;
using CoroutineUtil;
using DG.Tweening;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    public class StatueComponent_WhisperBubble : MonoBehaviour
    {
        [SerializeField] private float playerCycle;
        [SerializeField] private float bubbleRadius;
        [SerializeField] private float bubbleDuration;
        [SerializeField] private Transform whisperRoot;
        [SerializeField] private GameObject whisperBubble;
        
        private Statue statue;
        private CoroutineExcuter whisperPlayer;

        void Awake()
        {
            statue = GetComponent<Statue>();
            whisperPlayer = new CoroutineExcuter(this);
        }
        void Start()
        {
            whisperPlayer.Excute(coroutineRepeatSFX());
        }
        IEnumerator coroutineRepeatSFX()
        {
            while(true)
            {
                statue.PlayWhisper();

                var zoneInstance = Instantiate(whisperBubble);
                zoneInstance.transform.position = whisperRoot.position;
                zoneInstance.transform.localScale = Vector3.zero;
                var zoneSeq = DOTween.Sequence();
                zoneSeq.Append(zoneInstance.transform.DOScale(bubbleRadius * Vector3.one, 0.2f).SetEase(Ease.OutBack))
                        .Append(zoneInstance.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuad)
                                .SetDelay(bubbleDuration)
                                .OnComplete(()=>Destroy(zoneInstance)));

                yield return new WaitForSeconds(playerCycle);
            }
        }
    }
}
