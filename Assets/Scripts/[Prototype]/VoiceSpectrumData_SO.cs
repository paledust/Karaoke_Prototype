using UnityEngine;

namespace WhisperPrototype
{
    [CreateAssetMenu(fileName = "voice_spectrum", menuName = "VoiceHandling/VoiceSpectrum")]
    public class VoiceSpectrumData_SO : ScriptableObject
    {
        public Gradient spectrumColor;
    }
}
