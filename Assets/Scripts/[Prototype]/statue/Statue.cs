using System;
using DG.Tweening;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    public class Statue : MonoBehaviour
    {
        [SerializeField] private AudioSource statueAudio;
        [SerializeField] private WhisperHandler whisperHandler;
        [SerializeField] private WhisperWordData_SO whisper;
        [SerializeField] private ParticleSystem vfxWhispering;
        private GameObject player;

        public void PlayWhisper()
        {
            vfxWhispering.Play();
            AudioManager.Instance.PlaySFX(statueAudio, whisper.GetClipKey(), 1);
            whisperHandler.TryCastWhisper(whisper);
            if(player!=null)
            {
                player.GetComponent<WhisperHandler>().HearingWhisper(whisper);
            }
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
                player = other.gameObject;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                player = null;
            }
        }
    }
}