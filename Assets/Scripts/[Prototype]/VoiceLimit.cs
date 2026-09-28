using AudioAnalysis;
using UnityEngine;

namespace WhisperPrototype
{
    public class VoiceLimit : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sprite;
        private SpriteFader voiceSpriteFader;

        void OnEnable()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering += OnPlayerStartSinging;
            PlayerWhisperingEvent.E_OnPlayerStopWhispering += OnPlayerStopSinging;
        }
        void OnDisable()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering -= OnPlayerStartSinging;
            PlayerWhisperingEvent.E_OnPlayerStopWhispering -= OnPlayerStopSinging;
        }
        void Start()
        {
            voiceSpriteFader = new SpriteFader(sprite);
        }
        void OnPlayerStartSinging(AudioAnalyzer analyzer)
        {
            voiceSpriteFader.OnFadeInSprite();
        }
        void OnPlayerStopSinging()
        {
            voiceSpriteFader.OnFadeOutSprite();
        }
    }
}
