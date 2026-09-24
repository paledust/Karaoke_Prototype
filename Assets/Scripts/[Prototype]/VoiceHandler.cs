using UnityEngine;

namespace VoicePrototype
{
    public class VoiceHandler : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float voiceScaleLimit;
        [SerializeField] private float voiceLimitScaler = 1;
        [SerializeField] private Transform limiter;
        
        void Start()
        {
            limiter.localScale = voiceScaleLimit * voiceLimitScaler * Vector3.one;
        }
    }
}