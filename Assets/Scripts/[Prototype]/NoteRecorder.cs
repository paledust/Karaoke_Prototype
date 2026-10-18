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
        }
        [SerializeField] private float volumeGate;
        [SerializeField] private float volumeTolerance;
        [SerializeField] private float noteSampleMinGate;
        [SerializeField] private float silenceGate = 0.5f;
        [SerializeField, ShowOnly] private NoteRecordingState recordingState;

        [Header("Recording Source")]
        [SerializeField] private AudioSource recordingSource;

        [Header("Whisper")]
        [SerializeField] private WhisperHandler whisperHandler;
        
        private string device;
        private float noteMaintainTimer;
        private float silenceTimer;
        private int lastRecordedNote;
        private int chantingNote;
        [SerializeField, ShowOnly] private List<int> listNotes;
        private AudioAnalyzer audioAnalyzer;
        
        public event Action<WhisperWordData_SO> OnNoteMatchingWhisper;
        
        void Awake()
        {
            listNotes = new List<int>();
            recordingState = NoteRecordingState.Pending;
            chantingNote = -1;
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
                        noteMaintainTimer = 0;
                        chantingNote = -1;
                        lastRecordedNote = -1;

                        return;
                    }
                    return;
                case NoteRecordingState.Recording:
                    if(audioAnalyzer == null)
                    {
                        recordingState = NoteRecordingState.Pending;
                        noteMaintainTimer = 0;
                        chantingNote = -1;
                        lastRecordedNote = -1;

                        return;
                    }
                    if(chantingNote!=PitchToNote())
                    {
                        noteMaintainTimer = 0;
                        chantingNote = PitchToNote();
                    }
                    else
                    {
                        noteMaintainTimer += Time.deltaTime;
                        if(noteMaintainTimer >= noteSampleMinGate && lastRecordedNote!=chantingNote)
                        {
                            lastRecordedNote = chantingNote;
                            listNotes.Add(chantingNote);
                        }
                    }

                    if(audioAnalyzer.m_volumeLevel < Mathf.Max(0, volumeGate - volumeTolerance))
                    {
                        silenceTimer += Time.deltaTime;
                        chantingNote = -1;
                        if(silenceTimer > silenceGate)
                        {
                            recordingState = NoteRecordingState.Pending;
                            chantingNote = -1;
                            return;
                        }
                    }
                    else
                    {
                        silenceTimer = 0;
                    }

                    return;
            }
        }

        #region Recording
        int PitchToNote() => WhisperingManager.GetValidatePitchIndex(audioAnalyzer.m_smoothRawPitch);
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

            this.audioAnalyzer = null;

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
