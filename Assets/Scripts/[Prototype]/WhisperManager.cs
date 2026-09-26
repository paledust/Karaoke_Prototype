using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperingManager : Singleton<WhisperingManager>
    {
        [SerializeField] private VoiceSpectrumData_SO whisperSpectrum;
        [SerializeField] private Vector2Int pitchRange;
        [SerializeField] private Vector2 volumeRange;
        public static Color GetSpectrumColor(float noteIndex)
        {
            float spectrumIndex = Mathf.InverseLerp(Instance.pitchRange.x, Instance.pitchRange.y, noteIndex);
            return Instance.whisperSpectrum.spectrumColor.Evaluate(spectrumIndex);
        }
        public static float GetNormalizedVolumeScale(float volume) => Mathf.InverseLerp(Instance.volumeRange.x, Instance.volumeRange.y, volume);
    }
}
