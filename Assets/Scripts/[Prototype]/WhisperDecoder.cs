using System;
using AudioAnalysis;
using DG.Tweening;
using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperDecoder : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer showingSprite;
        [SerializeField] private WhisperHandler whisperHandler;

        void Awake()
        {
            showingSprite.enabled = false;
        }
        void OnEnable()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering += OnStartWhispering;
        }
        void OnDisable()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering -= OnStartWhispering;
        }
        void OnStartWhispering(AudioAnalyzer analysis)
        {
            FadeOutWhisper();
        }
        public bool DecodeWhisper(int[] notes)
        {
            var whisper = WhisperingManager.GetWhisper(notes);

            if(whisper != null)
            {
                showingSprite.sprite = whisper.GetIcon();
                showingSprite.color = new Color(1,1,1,0);
                showingSprite.enabled = true;
                showingSprite.DOKill();
                showingSprite.DOFade(1, 1f).SetEase(Ease.OutQuad)
                             .OnComplete(()=>showingSprite.DOFade(0, 2f));
                whisperHandler.TryCastWhisper(whisper);
            }
            return whisper != null;
        }
        public void FadeOutWhisper()
        {
            showingSprite.DOKill();
            showingSprite.DOFade(0, 0.3f).OnComplete(()=>showingSprite.enabled = false);
        }
    }
}