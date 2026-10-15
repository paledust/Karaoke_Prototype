using System;
using System.Collections.Generic;
using AudioAnalysis;
using UnityEngine;

namespace WhisperPrototype
{
    public class NoteRecorder : MonoBehaviour
    {
        private enum NoteRecordingState
        {
            Pending = 0,
            Recording = 1,
            Concluding = 2,
        }
        [SerializeField] private float volumeGate;
        [SerializeField] private float volumeTolerance;
        [SerializeField] private float recordingTimeGate;
        [SerializeField, ShowOnly] private NoteRecordingState recordingState;

        [Header("Recording Source")]
        [SerializeField] private AudioSource recordingSource;

        [Header("Whisper")]
        [SerializeField] private WhisperHandler whisperHandler;
        
        private string device;
        private float stateTimer;
        private List<int> listNotes;
        private AudioAnalyzer audioAnalyzer;
        
        public event Action<WhisperWordData_SO> OnNoteMatchingWhisper;
        
        void Awake()
        {
            listNotes = new List<int>();
            recordingState = NoteRecordingState.Pending;
        }

        // Update is called once per frame
        void Update()
        {
            if(audioAnalyzer==null)
                return;
            switch(recordingState)
            {
                case NoteRecordingState.Pending:
                    if(audioAnalyzer.m_volumeLevel > volumeGate)
                    {
                        // Enter Note Recording
                        recordingState = NoteRecordingState.Recording;
                        stateTimer = 0;

                        return;
                    }
                    return;
                case NoteRecordingState.Recording:
                    stateTimer += Time.deltaTime;
                    // currentBuildingDisplayer.UpdateNote(WhisperingManager.GetSpectrumColor(audioAnalyzer.m_rawPitchIndex));

                    if(audioAnalyzer.m_volumeLevel < Mathf.Max(0, volumeGate - volumeTolerance))
                    {
                        if(stateTimer > recordingTimeGate)
                        {
                            recordingState = NoteRecordingState.Concluding;
                            stateTimer = 0;

                            listNotes.Add(WhisperingManager.GetValidatePitchIndex(audioAnalyzer.m_rawPitchIndex));
                            return;
                        }
                        else
                        {
                            // Not enough recording time, discard the note and go back to pending state
                            recordingState = NoteRecordingState.Pending;
                            stateTimer = 0;

                            return;
                        }
                    }

                    return;
                case NoteRecordingState.Concluding:
                    recordingState = NoteRecordingState.Pending;
                    stateTimer = 0;

                    return;
            }
        }
        
        #region Recording
        public void BeginRecording(AudioAnalyzer audioAnalyzer)
        {
            recordingSource.Stop();
            var recordingClip = RecordManager.Instance.BeginRecording(5);
            while (!(Microphone.GetPosition(device) > 0.02f)) {}
            recordingSource.clip = recordingClip;
            recordingSource.loop = true;
            recordingSource.Play();

            // Reset State
            this.audioAnalyzer = audioAnalyzer;
            if(recordingState != NoteRecordingState.Pending)
                recordingState = NoteRecordingState.Pending;
        }
        public void EndRecording()
        {
            recordingSource.Stop();
            RecordManager.Instance.StopRecording();

            // Reset State and Clean up
            if(recordingState != NoteRecordingState.Pending)
                recordingState = NoteRecordingState.Pending;

            if(listNotes!=null && listNotes.Count>0)
            {
                var whisper = whisperHandler.DecodeWhisperFromNote(listNotes.ToArray());
                if(whisper != null)
                {
                    whisperHandler.TryCastWhisper(whisper);
                    OnNoteMatchingWhisper?.Invoke(whisper);

                }
            }
            listNotes.Clear();
        }
        #endregion
    }
}
