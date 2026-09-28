using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperingManager : Singleton<WhisperingManager>
    {
        [SerializeField] private WhisperWordDataCollection_SO whisperWordsCollection;
        [SerializeField] private VoiceSpectrumData_SO whisperSpectrum;
        [SerializeField] private Vector2 volumeRange;
        [SerializeField] private bool includeDeviation;
        [SerializeField] private int octave;

        public static Color GetSpectrumColor(int noteIndex)
        {
            int spectrumIndex = GetValidatePitchIndex(noteIndex);
            return Instance.whisperSpectrum.GetColorFromNoteIndex(Mathf.RoundToInt(spectrumIndex));
        }
        public static int GetValidatePitchIndex(int raw_noteIndex)
        {
            int noteIndex = raw_noteIndex;
            if(noteIndex < Instance.octave*12)
                noteIndex = 0;
            else if(noteIndex >= (Instance.octave+1)*12)
                noteIndex = 11;
            else noteIndex = noteIndex % 12;
            
            if(Instance.includeDeviation)
                return noteIndex;

            switch(noteIndex)
            {
                case 0:
                case 1:
                    return 0;
                case 2:
                case 3:
                    return 1;
                case 4:
                    return 2;
                case 5:
                case 6:
                    return 3;
                case 7:
                case 8:
                    return 4;
                case 9:
                case 10:
                    return 5;
                case 11:
                    return 6;

            }
            return noteIndex;
        }
        public static float GetNormalizedVolumeScale(float volume) => Mathf.InverseLerp(Instance.volumeRange.x, Instance.volumeRange.y, volume);
        public static WhisperWordData_SO GetWhisper(int[] note)=>Instance.whisperWordsCollection.GetWhisperByNote(note);
    }
}
