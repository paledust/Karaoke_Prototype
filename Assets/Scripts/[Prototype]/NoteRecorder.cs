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

        [Header("Note Display")]
        [SerializeField] private Transform noteSlot;
        [SerializeField] private float noteDistance;
        [SerializeField] private GameObject showingNotePrefab;
        [SerializeField] private Sprite shortNoteSprite;
        [SerializeField] private Sprite longNoteSprite;

        [Header("VFX")]
        [SerializeField] private ParticleSystem vfxNoteOnShow;

        private AudioAnalyzer audioAnalyzer;
        private List<NoteDisplayer> listNoteDisplayer;
        private NoteDisplayer currentBuildingDisplayer;
        private float stateTimer;
        
        void Awake()
        {
            listNoteDisplayer = new List<NoteDisplayer>();
            recordingState = NoteRecordingState.Pending;
            // Clean Up all placeholder note
            foreach(Transform placeHolder in noteSlot)
            {
                Destroy(placeHolder.gameObject);
            }
        }
        void OnEnable()
        {
            PlayerSingingEvent.E_OnPlayerStartToSing += OnPlayerStartSinging;
            PlayerSingingEvent.E_OnPlayerStopSinging += OnPlayerStopSinging;
        }
        void OnDisable()
        {
            PlayerSingingEvent.E_OnPlayerStartToSing -= OnPlayerStartSinging;
            PlayerSingingEvent.E_OnPlayerStopSinging -= OnPlayerStopSinging;
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

                        // Recording Satate Enter
                        var note = Instantiate(showingNotePrefab, noteSlot);
                        note.name = $"displayer_{listNoteDisplayer.Count}";
                        currentBuildingDisplayer = note.GetComponent<NoteDisplayer>();
                        listNoteDisplayer.Add(currentBuildingDisplayer);
                        RepositionAllNote();

                        return;
                    }
                    return;
                case NoteRecordingState.Recording:
                    stateTimer += Time.deltaTime;
                    currentBuildingDisplayer.UpdateNote(WhisperingManager.GetSpectrumColor(audioAnalyzer.m_rowPitchIndex));

                    if(audioAnalyzer.m_volumeLevel < Mathf.Max(0, volumeGate - volumeTolerance))
                    {
                        if(stateTimer > recordingTimeGate)
                        {
                            recordingState = NoteRecordingState.Concluding;
                            stateTimer = 0;

                            currentBuildingDisplayer.ReleaseNote();
                            currentBuildingDisplayer = null;
                            return;
                        }
                        else
                        {
                            // Not enough recording time, discard the note and go back to pending state
                            recordingState = NoteRecordingState.Pending;
                            stateTimer = 0;

                            // Discard note on exit
                            listNoteDisplayer.Remove(currentBuildingDisplayer);
                            RepositionAllNote();
                            Destroy(currentBuildingDisplayer.gameObject);
                            currentBuildingDisplayer = null;

                            return;
                        }
                    }
                    if(!currentBuildingDisplayer.IsConfirm && stateTimer > recordingTimeGate)
                    {
                        currentBuildingDisplayer.ConfirmNote();
                        // Play VFX
                        vfxNoteOnShow.transform.position = currentBuildingDisplayer.transform.position;
                        vfxNoteOnShow.Play();
                    }
                    if(!currentBuildingDisplayer.IsLong && stateTimer > longNoteTimeGate)
                    {
                        currentBuildingDisplayer.SwapToLongNote(longNoteSprite);
                    }
                    return;
                case NoteRecordingState.Concluding:
                    recordingState = NoteRecordingState.Pending;
                    stateTimer = 0;

                    return;
            }
        }
        void RepositionAllNote()
        {
            for(int i=0; i<listNoteDisplayer.Count; i++)
            {
                listNoteDisplayer[i].transform.localPosition = Vector3.right * (noteDistance * (i - (listNoteDisplayer.Count - 1) * 0.5f));
            }
        }

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
            {
                recordingState = NoteRecordingState.Pending;
            }         
            // Discard note on exit
            foreach(var note in listNoteDisplayer)
            {
                Destroy(note.gameObject);
            }
            listNoteDisplayer.Clear();
        }
        #endregion
    }
}
