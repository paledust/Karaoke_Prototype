using System.Collections;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhisperPrototype
{
    public class PlayerWhisperShortcut : MonoBehaviour
    {
        [SerializeField] private WhisperHandler whisperHandler;
        [SerializeField] private Animator playerAnime;
        [SerializeField] private AudioSource whisperSource;
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
                AudioManager.Instance.PlaySFX(whisperSource, whisper.GetClipPlayerKey(), .25f);
                playerAnime.SetTrigger(ANIM_BOOK_TRIGGER);
                StartCoroutine(coroutineCastWhisper(whisper));
                PlayerWhisperingEvent.Call_OnPlayerUseWhisperAtIndex(index);
            }
        }
        IEnumerator coroutineCastWhisper(WhisperWordData_SO whisper)
        {
            yield return new WaitForSeconds(1f);
            whisperHandler.TryCastWhisper(whisper);
        }
    }
}
