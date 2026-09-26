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
            PlayerSingingEvent.E_OnPlayerStartToSing += OnPlayerStartSinging;
            PlayerSingingEvent.E_OnPlayerStopSinging += OnPlayerStopSinging;
        }
        void OnDisable()
        {
            PlayerSingingEvent.E_OnPlayerStartToSing -= OnPlayerStartSinging;
            PlayerSingingEvent.E_OnPlayerStopSinging -= OnPlayerStopSinging;
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
