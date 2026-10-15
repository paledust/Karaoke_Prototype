using UnityEngine;
using AudioAnalysis;
using DG.Tweening;

namespace WhisperPrototype
{
    public class VoiceOrb : MonoBehaviour
    {
        [SerializeField] private Vector2 volumeScale;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animation spriteFadingAnimation;

        private AudioAnalyzer targetAnalyzer;
        private PlayerInputControl playerInput;
        private Sequence voiceOrbSeq;

        private const string DISSOLVE_OUT = "orb_dissolve_out";
        private const string DISSOLVE_IN = "orb_dissolve_in";


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
            var clearColor = sprite.color;
            clearColor.a = 0;
            sprite.color = clearColor;
            voiceOrbSeq = DOTween.Sequence();
        }
        void Update()
        {
            if(targetAnalyzer==null)
                return;

            float scale = Mathf.Lerp(volumeScale.x, volumeScale.y, playerInput.m_recordingProgress);
            Color color = WhisperingManager.GetSpectrumColor(targetAnalyzer.m_rawPitchIndex);

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * scale, Time.deltaTime * 5);
            sprite.color = Color.Lerp(sprite.color, color, Time.deltaTime * 10);
        }
        void OnPlayerStartSinging(AudioAnalyzer analyzer)
        {
            transform.localScale = Vector3.one * volumeScale.x;
            voiceOrbSeq.Kill();
            voiceOrbSeq.Join(sprite.DOFade(1, 0.2f));
            spriteFadingAnimation.Play(DISSOLVE_IN);
            playerInput = analyzer.GetComponent<PlayerInputControl>();
            targetAnalyzer = analyzer;
        }
        void OnPlayerStopSinging()
        {
            Vector3 currentScale = transform.localScale;
            voiceOrbSeq.Kill();
            voiceOrbSeq.Join(transform.DOScale(currentScale + Vector3.one * 0.5f, .5f))
                        .Join(sprite.DOFade(0, 1f));
            spriteFadingAnimation.Play(DISSOLVE_OUT);
            targetAnalyzer = null;
        }
    }
}
