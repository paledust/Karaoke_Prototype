using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype
{
    [CreateAssetMenu(fileName = "WhisperWordData_SO", menuName = "Whisper_Word/WhisperWordData_SO")]
    public class WhisperWordData_SO : ScriptableObject
    {
        [SerializeField] private string key;
        [SerializeField] private Sprite icon;
        [SerializeField] private int[] notes;
        [SerializeField] private AudioDataClip_SO whisperClip;
        [SerializeField] private AudioDataClip_SO whisperClipPlayer;
        [SerializeField] private WhisperEffectData_SO whisperEffectData;
        public WhisperEffect GetWhisperEffect()=>whisperEffectData.GetWhisperEffect();
        public void AssignClipToPlayerAudioData(AudioClip clip) => whisperClipPlayer.AssignClip(clip);
        public string GetClipKey()=>whisperClip.name;
        public string GetClipPlayerKey()=>whisperClipPlayer.name;
        public string GetKey()=>key;
        public Sprite GetIcon()=>icon;
        public int[] GetNotes()=>notes;
    }
}