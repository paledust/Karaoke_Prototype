using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperingManager : Singleton<WhisperingManager>
    {
        [SerializeField] private WhisperWordDataCollection_SO whisperWordsCollection;
        [SerializeField] private VoiceSpectrumData_SO whisperSpectrum;
        [SerializeField] private int octave;
        [SerializeField] private Vector2 volumeRange;
        public static Color GetSpectrumColor(int noteIndex)
        {
            int spectrumIndex = Mathf.RoundToInt(GetValidatePitchIndex(noteIndex));
            return Instance.whisperSpectrum.GetColorFromNoteIndex(Mathf.RoundToInt(spectrumIndex));
        }
        public static int GetValidatePitchIndex(int raw_noteIndex)
        {
            if(raw_noteIndex < Instance.octave*12)
            {
                return 0;
            }
            else if(raw_noteIndex >= (Instance.octave+1)*12)
            {
                return 11;
            }
            else return raw_noteIndex % 12;
        }
        public static float GetNormalizedVolumeScale(float volume) => Mathf.InverseLerp(Instance.volumeRange.x, Instance.volumeRange.y, volume);
        public static WhisperWordData_SO GetWhisper(int[] note)=>Instance.whisperWordsCollection.GetWhisperByNote(note);
    }
}
