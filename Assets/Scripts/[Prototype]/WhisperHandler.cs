using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperHandler : MonoBehaviour
    {
        [SerializeField] private Transform castRoot;
        [SerializeField] private float castRadius;
        [SerializeField] private bool unlockAllWhisper = false;

        public WhisperWordData_SO DecodeWhisperFromNote(int[] notes)
        {
            var whisper = WhisperingManager.GetWhisper(notes);
            return whisper;
        }
        public bool TryCastWhisper(WhisperWordData_SO whisper)
        {
            if(unlockAllWhisper || WhisperingManager.HasLearnedWhisper(whisper))
            {
                var effect = whisper.GetWhisperEffect();
                effect.InitializeWhisper(this, castRoot.position, castRadius);

                return true;
            }
            return false;
        }
        public void HearingWhisper(WhisperWordData_SO whisper)
        {
            if(!WhisperingManager.HasLearnedWhisper(whisper))
            {
                PlayerWhisperingEvent.Call_OnPlayerHearingNewWhisper(whisper);
            }
        }
    }
}