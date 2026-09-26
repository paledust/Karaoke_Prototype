using UnityEngine;
using AudioAnalysis;

namespace WhisperPrototype
{
    public class VoiceOrb : MonoBehaviour
    {
        [SerializeField] private Vector2 volumeScale;
        [SerializeField] private SpriteRenderer sprite;

        private AudioAnalyzer targetAnalyzer;
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
        void Update()
        {
            if(targetAnalyzer==null)
                return;

            float scale = Mathf.Lerp(volumeScale.x, volumeScale.y, WhisperingManager.GetNormalizedVolumeScale(targetAnalyzer.m_volumeLevel));
            Color color = WhisperingManager.GetSpectrumColor(targetAnalyzer.m_rowPitchIndex);

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scale, Time.deltaTime * 5);
            sprite.color = Color.Lerp(sprite.color, color, Time.deltaTime * 10);
        }
        void OnPlayerStartSinging(AudioAnalyzer analyzer)
        {
            transform.localScale = Vector3.zero;
            voiceSpriteFader.OnFadeInSprite();
            targetAnalyzer = analyzer;
        }
        void OnPlayerStopSinging()
        {
            voiceSpriteFader.OnFadeOutSprite();
            targetAnalyzer = null;
        }
    }
}
