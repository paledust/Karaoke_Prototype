using System.Collections;
using AudioAnalysis;
using CoroutineUtil;
using DG.Tweening;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    public class StatueHarmonyWithPlayer : MonoBehaviour
    {
        [SerializeField] private Vector2 volumeRange;
        [Header("Audio Play")]
        [SerializeField] private AudioSource m_audio;
        [SerializeField] private AudioData_SO whisperClip;
        [SerializeField] private float playingFreq;
        [SerializeField] private ParticleSystem vfxSinging;

        [SerializeField, ShowOnly] private float sensation = 0;
        [SerializeField, ShowOnly] private bool isHarmany = false;
        [SerializeField, ShowOnly] private bool isAudioPlaying = false;
        private AudioAnalyzer playerWhisper;
        private CoroutineExcuter whisperPlayer;

        public bool IsHarmony => isHarmany;
        
        void Start()
        {
            m_audio.volume = 0;
            isHarmany = false;
            isAudioPlaying = false;
            whisperPlayer = new CoroutineExcuter(this);
        }
        void Update()
        {
            float n_volume = playerWhisper==null?0:WhisperingManager.GetNormalizedVolumeScale(playerWhisper.m_volumeLevel);
            if(isHarmany)
            {
                if(!volumeRange.IsWithinRange(n_volume))
                {
                    isHarmany = false;

                    isAudioPlaying = n_volume >= volumeRange.x;
                    m_audio.DOKill();
                    if(isAudioPlaying)
                        m_audio.DOFade(0.1f, 1);
                    else
                        m_audio.DOFade(0, 1);
                    vfxSinging.Stop();
                    return;
                }
            }
            else
            {
                if(n_volume>=volumeRange.x && !isAudioPlaying)
                {
                    isAudioPlaying = true;
                    m_audio.DOKill();
                    m_audio.DOFade(0.1f, 0.5f);
                }
                if(n_volume<volumeRange.x && isAudioPlaying)
                {
                    isAudioPlaying = false;
                    m_audio.DOKill();
                    m_audio.DOFade(0, 1);
                }

                if(volumeRange.IsWithinRange(n_volume))
                    sensation += Time.deltaTime;
                else
                    sensation -= Time.deltaTime;

                sensation = Mathf.Clamp01(sensation);

                if(sensation >= 1)
                {
                    sensation = 1;
                    isHarmany = true;
                    vfxSinging.Play(true);
                    m_audio.DOKill();
                    m_audio.DOFade(0.25f, 0.5f);
                    return;
                }
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out playerWhisper))
            {
                whisperPlayer.Excute(coroutineRepeatSFX());
            }
        }
        void OnTriggerExit(Collider other)
        {
            if(other.TryGetComponent(out playerWhisper))
            {
                isAudioPlaying = false;
                whisperPlayer.Abort();
                playerWhisper = null;
            }
        }
        IEnumerator coroutineRepeatSFX()
        {
            while(true)
            {
                AudioManager.Instance.PlaySFX(m_audio, whisperClip.name, 1);
                yield return new WaitForSeconds(1/playingFreq);
            }
        }
    }
}