using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperHandler : MonoBehaviour
    {
        [SerializeField] private Transform castRoot;
        [SerializeField] private float castRadius;
        [SerializeField] private bool unlockAllWhisper = false;

        public bool DecodeWhisperFromNote(int[] notes)
        {
            var whisper = WhisperingManager.GetWhisper(notes);

            if(whisper != null)
            {
                TryCastWhisper(whisper);
            }
            return whisper != null;
        }
        public bool TryCastWhisper(WhisperWordData_SO whisper)
        {
            if(unlockAllWhisper || WhisperingManager.HasLearnedWhisper(whisper.GetKey()))
            {
                var effect = whisper.GetWhisperEffect();
                effect.InitializeWhisper(this, castRoot.position, castRadius);

                return true;
            }
            return false;
        }
        public void HearingWhisper(WhisperWordData_SO whisper)
        {
            if(!WhisperingManager.HasLearnedWhisper(whisper.GetKey()))
            {
                PlayerWhisperingEvent.Call_OnPlayerHearingNewWhisper(whisper);
            }
        }
    }
}