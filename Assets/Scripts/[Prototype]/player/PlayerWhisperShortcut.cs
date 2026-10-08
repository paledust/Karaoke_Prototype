using System;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using WhisperPrototype.UI;

namespace WhisperPrototype
{
    public class PlayerWhisperShortcut : MonoBehaviour
    {
        [SerializeField] private WhisperHandler whisperHandler;
        [SerializeField] private Animator playerAnime;
        private const string ANIM_BOOK_TRIGGER = "UseBook";
        
        void Update()
        {
            if(Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                CastWhisperShortcut(0);
                return;
            }
            if(Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                CastWhisperShortcut(1);
                return;
            }
            if(Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                CastWhisperShortcut(2);
                return;
            }
        }
        void CastWhisperShortcut(int index)
        {
            if(WhisperingManager.IsWhisperValidAt(index))
            {
                var whisper = WhisperingManager.GetLearnedWhisper(index);
                AudioManager.Instance.PlaySFX(whisper.GetClipPlayerKey(), 1);
                whisperHandler.TryCastWhisper(whisper);
                playerAnime.SetTrigger(ANIM_BOOK_TRIGGER);
                PlayerWhisperingEvent.Call_OnPlayerUseWhisperAtIndex(index);
            }
        }
    }
}
