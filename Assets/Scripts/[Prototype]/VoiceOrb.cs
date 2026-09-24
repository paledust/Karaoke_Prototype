using UnityEngine;
using AudioAnalysis;

namespace VoicePrototype
{
    public class VoiceOrb : MonoBehaviour
    {
        [SerializeField] private Gradient pitchColor;
        [SerializeField] private int pitchRange = 32;
        [SerializeField] private Vector2 volumeScale;
        [SerializeField] private float volumeScaler = 1;
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

            float volume = targetAnalyzer.m_volumeLevel;
            int noteIndex = targetAnalyzer.m_rowPitchIndex;
            float scale = Mathf.Lerp(volumeScale.x, volumeScale.y, volume * volumeScaler);
            Color color = pitchColor.Evaluate((noteIndex+0f)/pitchRange);

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
