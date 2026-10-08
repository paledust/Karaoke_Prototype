using UnityEngine;

namespace WhisperPrototype.UI
{
    public class UI_WhisperShortcut : MonoBehaviour
    {
        [SerializeField] private UI_WhisperSlot[] whisperSlots;
        void Start()
        {
            PlayerWhisperingEvent.E_OnPlayerEndLeanredNewWhisper += LearnNewWhisperHandler;
            PlayerWhisperingEvent.E_OnPlayerUseWhisperAtIndex += UseWhisperHandler;
        }
        void OnDestroy()
        {
            PlayerWhisperingEvent.E_OnPlayerEndLeanredNewWhisper -= LearnNewWhisperHandler;
            PlayerWhisperingEvent.E_OnPlayerUseWhisperAtIndex -= UseWhisperHandler;
        }
        void UseWhisperHandler(int index)
        {
            whisperSlots[index].TriggerWhisper();
        }
        void LearnNewWhisperHandler(WhisperWordData_SO whisper)
        {
            for(int i=0; i<whisperSlots.Length; i++)
            {
                if(whisperSlots[i].isAssigned)
                    continue;
                whisperSlots[i].AssignWhisper(whisper);
                return;
            }
        }
    }
}
