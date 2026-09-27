using UnityEngine;

namespace WhisperPrototype
{
    [CreateAssetMenu(fileName = "voice_spectrum", menuName = "VoiceHandling/VoiceSpectrum")]
    public class VoiceSpectrumData_SO : ScriptableObject
    {
        [SerializeField] private Color[] spectrumColor;
        public Color GetColorFromNoteIndex(int noteIndex) => spectrumColor[noteIndex];
        
    }
}
