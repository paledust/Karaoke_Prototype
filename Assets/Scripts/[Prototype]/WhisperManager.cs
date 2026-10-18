using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleSaveSystem;
using UnityEngine;
using UnityEngine.Networking;

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
        private Dictionary<string, AudioClip> dictWhisperClip;

        public Guid guid => new Guid(byteGuid);

        protected override void Awake()
        {
            base.Awake();
            dictWhisperClip = new Dictionary<string, AudioClip>();
            learnedWhisperData = new List<WhisperWordData_SO>();
        }

    #region Save
        public void RestoreState(SaveData state)
        {
            var learnedWhisperKey = state.learnedWhisper;
            for(int i=0; i<learnedWhisperKey.Count; i++)
            {
                var data = whisperWordsCollection.GetDataByKey(learnedWhisperKey[i]);
                StartCoroutine(coroutineLoadWhisperClip(data.GetKey()));
                learnedWhisperData.Add(data);
                PlayerWhisperingEvent.Call_OnPlayerLearnedNewWhisper(data);
            }
        }
        public void CaptureState(ref SaveData saveData)
        {
            var saveWhisper = saveData.learnedWhisper;
            if(saveWhisper==null)
                saveWhisper = new List<string>();
            for(int i=0; i<learnedWhisperData.Count; i++)
            {
                if(!saveWhisper.Contains(learnedWhisperData[i].GetKey()))
                    saveWhisper.Add(learnedWhisperData[i].GetKey());
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
            PlayerWhisperingEvent.Call_OnPlayerLearnedNewWhisper(whisperData);

            var clip = RecordManager.Instance.m_pendingClip;

            //Save Clip to Dictionary
            Instance.dictWhisperClip[whisperData.GetKey()] = clip;
            whisperData.AssignClipToPlayerAudioData(clip);

            //Save Clip as .wav file
            SaveWav.Save(GetPlayerWhisperClipPath(whisperData.GetKey()), clip);

        }
        IEnumerator coroutineLoadWhisperClip(string whisperKey)
        {
            var request = SaveWav.Load(GetPlayerWhisperClipPath(whisperKey));
            yield return request.SendWebRequest();

            if(request.result == UnityWebRequest.Result.ConnectionError)
                Debug.LogError("WhisperManager: Audio Request Error:" + request.error);
            else if(request.isDone && request.error == null)
            {
                Debug.LogWarning("Load Clip");
                var clip = DownloadHandlerAudioClip.GetContent(request);
                dictWhisperClip[whisperKey] = clip;
                whisperWordsCollection.GetDataByKey(whisperKey).AssignClipToPlayerAudioData(clip);
            }
            else
            {
                Debug.LogWarning(">>>>");
            }
        }
        static string GetPlayerWhisperClipPath(string whisperKey)
        {
            Debug.LogWarning(whisperKey);
            string fileName = $"sfx_{whisperKey}_player.wav";
        #if UNITY_EDITOR
            return Path.Combine(Application.dataPath + "/AddressableLocal/whisper_clip/", fileName);
        #else
            return Path.Combine(Application.persistentDataPath, fileName);
        #endif
        }
    }
}
