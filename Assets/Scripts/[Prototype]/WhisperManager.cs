using System;
using System.Collections.Generic;
using SimpleSaveSystem;
using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperingManager : Singleton<WhisperingManager>, ISaveable
    {
        [SerializeField, ShowOnly] private string byteGuid = Guid.NewGuid().ToString();
        [SerializeField] private WhisperWordDataCollection_SO whisperWordsCollection;
        [SerializeField] private VoiceSpectrumData_SO whisperSpectrum;
        [SerializeField] private Vector2 volumeRange;
        [SerializeField] private bool includeDeviation;
        [SerializeField] private int octave;

        private List<WhisperWordData_SO> learnedWhisperData;

        public Guid guid => new Guid(byteGuid);

        protected override void Awake()
        {
            base.Awake();
            learnedWhisperData = new List<WhisperWordData_SO>();
        }

    #region Save
        public void RestoreState(SaveData state)
        {
            var learnedWhisperKey = state.learnedWhisper;
            for(int i=0; i<learnedWhisperKey.Count; i++)
            {
                UnlockWhisper(whisperWordsCollection.GetDataByKey(learnedWhisperKey[i]));
            }
        }
        public void CaptureState(ref SaveData saveData)
        {
            saveData.learnedWhisper = new List<string>();
            for(int i=0; i<learnedWhisperData.Count; i++)
            {
                saveData.learnedWhisper.Add(learnedWhisperData[i].GetKey());
            }
        }
    #endregion
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
        public static float GetNormalizedVolumeScale(float volume) => (volume - Instance.volumeRange.x)/(Instance.volumeRange.y-Instance.volumeRange.x);
        public static WhisperWordData_SO GetWhisper(int[] note)=>Instance.whisperWordsCollection.GetWhisperByNote(note);
        public static WhisperWordData_SO GetLearnedWhisper(int index) => Instance.learnedWhisperData[index];
        public static bool HasLearnedWhisper(WhisperWordData_SO whisperData)=>Instance.learnedWhisperData.Contains(whisperData);
        public static bool IsWhisperValidAt(int index) => Instance.learnedWhisperData.Count>index && Instance.learnedWhisperData[index]!=null;
        public static void UnlockWhisper(WhisperWordData_SO whisperData)
        {
            Instance.learnedWhisperData.Add(whisperData);
            var clip = RecordManager.Instance.m_pendingClip;
            RecordManager.Instance.SaveCurrentClip($"sfx_{whisperData.GetKey()}_player");
            whisperData.AssignClipToPlayerAudioData(clip);
        }
    }
}
