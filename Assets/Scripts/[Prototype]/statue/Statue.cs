using System;
using DG.Tweening;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    public class Statue : MonoBehaviour
    {
        [SerializeField] private AudioSource statueAudio;
        [SerializeField] private WhisperWordData_SO whisper;
        [SerializeField] private ParticleSystem vfxWhispering;

        public event Action OnPlayerEnter;
        public event Action OnPlayerExit;

        public void PlayWhisper()
        {
            vfxWhispering.Play();
            AudioManager.Instance.PlaySFX(statueAudio, whisper.GetClipKey(), 1);            
        }
        public void AdjustVolume(float targetVolume, float duration)
        {
            statueAudio.DOKill();
            if(duration<=0)
            {
                statueAudio.volume = targetVolume;
                return;
            }
            statueAudio.DOFade(targetVolume, duration);
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                OnPlayerEnter?.Invoke();
            }
        }
        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                OnPlayerExit?.Invoke();
            }
        }
    }
}