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
        [SerializeField] private float longNoteTimeGate;
        [SerializeField, ShowOnly] private NoteRecordingState recordingState;

        // [Header("Note Display")]
        // [SerializeField] private Transform noteSlot;
        // [SerializeField] private float noteDistance;
        // [SerializeField] private GameObject showingNotePrefab;
        // [SerializeField] private Sprite shortNoteSprite;
        // [SerializeField] private Sprite longNoteSprite;

        [Header("Whisper")]
        [SerializeField] private WhisperHandler whisperHandler;

        private AudioAnalyzer audioAnalyzer;
        private List<NoteDisplayer> listNoteDisplayer;
        private List<int> listNotes;
        // private NoteDisplayer currentBuildingDisplayer;
        private float stateTimer;
        
        void Awake()
        {
            listNoteDisplayer = new List<NoteDisplayer>();
            listNotes = new List<int>();
            recordingState = NoteRecordingState.Pending;
            // // Clean Up all placeholder note
            // foreach(Transform placeHolder in noteSlot)
            // {
            //     Destroy(placeHolder.gameObject);
            // }
        }
        void OnEnable()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering += OnPlayerStartSinging;
            PlayerWhisperingEvent.E_OnPlayerStopWhispering += OnPlayerStopSinging;
        }
        void OnDisable()
        {
            PlayerWhisperingEvent.E_OnPlayerStartWhispering -= OnPlayerStartSinging;
            PlayerWhisperingEvent.E_OnPlayerStopWhispering -= OnPlayerStopSinging;
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

                        // // Recording Satate Enter
                        // var note = Instantiate(showingNotePrefab, noteSlot);
                        // note.name = $"displayer_{listNoteDisplayer.Count}";
                        // currentBuildingDisplayer = note.GetComponent<NoteDisplayer>();
                        // listNoteDisplayer.Add(currentBuildingDisplayer);
                        // RepositionAllNote();

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

                            // if(!currentBuildingDisplayer.IsConfirm)
                                // currentBuildingDisplayer.ConfirmNote();
                            listNotes.Add(WhisperingManager.GetValidatePitchIndex(audioAnalyzer.m_rawPitchIndex));
                            // currentBuildingDisplayer.ReleaseNote();
                            // currentBuildingDisplayer = null;
                            return;
                        }
                        else
                        {
                            // Not enough recording time, discard the note and go back to pending state
                            recordingState = NoteRecordingState.Pending;
                            stateTimer = 0;

                            // Discard note on exit
                            // listNoteDisplayer.Remove(currentBuildingDisplayer);
                            // RepositionAllNote();
                            // Destroy(currentBuildingDisplayer.gameObject);
                            // currentBuildingDisplayer = null;

                            return;
                        }
                    }
                    // if(!currentBuildingDisplayer.IsConfirm && stateTimer > recordingTimeGate)
                    // {
                    //     currentBuildingDisplayer.ConfirmNote();
                    //     // Play VFX
                    //     vfxNoteOnShow.transform.position = currentBuildingDisplayer.transform.position;
                    //     vfxNoteOnShow.Play();
                    // }
                    // if(!currentBuildingDisplayer.IsLong && stateTimer > longNoteTimeGate)
                    //     currentBuildingDisplayer.SwapToLongNote(longNoteSprite);
                    return;
                case NoteRecordingState.Concluding:
                    recordingState = NoteRecordingState.Pending;
                    stateTimer = 0;

                    return;
            }
        }
        // void RepositionAllNote()
        // {
        //     for(int i=0; i<listNoteDisplayer.Count; i++)
        //     {
        //         listNoteDisplayer[i].transform.localPosition = Vector3.right * (noteDistance * (i - (listNoteDisplayer.Count - 1) * 0.5f));
        //     }
        // }

        #region Singing Event Handling
        void OnPlayerStartSinging(AudioAnalyzer audioAnalyzer)
        {
            this.audioAnalyzer = audioAnalyzer;
            if(recordingState != NoteRecordingState.Pending)
                recordingState = NoteRecordingState.Pending;
        }
        void OnPlayerStopSinging()
        {
            if(recordingState != NoteRecordingState.Pending)
                recordingState = NoteRecordingState.Pending;

            // // Discard Current Note
            // if(currentBuildingDisplayer != null)
            // {
            //     currentBuildingDisplayer.PopNote();
            //     currentBuildingDisplayer = null;
            // }
            // Confirm the whispering
            var noteDisplayers = listNoteDisplayer.ToArray();
            if(listNotes!=null && listNotes.Count>0)
            {
                if(whisperHandler.DecodeWhisperFromNote(listNotes.ToArray()))
                {
                    // Discard note on exit
                    foreach(var note in noteDisplayers)
                        note.AbsorbNote();
                }
                else
                {
                    // Discard note on exit
                    foreach(var note in noteDisplayers)
                        note.PopNote();
                }
            }
            listNoteDisplayer.Clear();
            listNotes.Clear();
        }
        #endregion
    }
}
