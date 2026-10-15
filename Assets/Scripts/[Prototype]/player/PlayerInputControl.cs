using UnityEngine;
using UnityEngine.InputSystem;
using AudioAnalysis;
using SimpleAudioSystem;
using System.Collections;

namespace WhisperPrototype
{
    public class PlayerInputControl : MonoBehaviour
    {
        [SerializeField] private InputAction recordingKey;
        [SerializeField] private InputAction movingKey;

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform characterRoot;

        [Header("Audio Analyze")]
        [SerializeField] private AudioAnalyzer analyzer;

        [Header("Recording Source")]
        [SerializeField] private float maxRecordingSec = 3;
        [SerializeField] private float recordingBreakCoolDown = 1.5f;
        [SerializeField] private float recordingCoolDown = 1;
        [SerializeField] private NoteRecorder noteRecorder;

        [Header("Animation")]
        [SerializeField] private SpineAnimator spineAnimator;

        [Header("Audio")]
        [SerializeField] private AudioData_SO sfxWhisperData;
        [SerializeField] private AudioData_SO sfxWhisperEndData;

        private bool canMove;
        private bool isRecording;
        private bool recordingCooling;
        private float recordingTimer;
        private float speed;

        public float m_recordingProgress => recordingTimer/maxRecordingSec;

    #region Animation Key
        private const string idleName = "idle";
        private const string walkName = "walk";
        private const string walkStartName = "walk_start";
        private const string walkEndName = "walk_end";
        private const string chantName = "listen";
        private const string chantStartName = "listen_start";
        private const string chantEndName = "listen_end";
        private const string tiredStartName = "tired_start";
        private const string tiredName = "tired";
        private const string tiredEndName = "sit_end";
    #endregion

        void Awake()
        {
            canMove = true;
            isRecording = false;
        }
        void OnEnable()
        {
            recordingKey.performed += StartRecording;
            recordingKey.canceled += StopRecording;
            movingKey.performed += Moving;
            movingKey.canceled += Stopping;
            recordingKey.Enable();
            movingKey.Enable();
        }
        void OnDisable()
        {
            recordingKey.performed -= StartRecording;
            recordingKey.canceled -= StopRecording;
            movingKey.performed  -= Moving;
            movingKey.canceled -= Stopping;
            recordingKey.Disable();
            movingKey.Disable();
        }
        void Update()
        {
            transform.position += speed * Vector3.right * Time.deltaTime * moveSpeed;

            if(isRecording)
            {
                recordingTimer += Time.deltaTime;
                if(recordingTimer >= maxRecordingSec)
                {
                    StopRecording(default);
                }
            }
        }

        public void LockMovement()
        {
            canMove = false;
            Stopping(default);
        }
        public void UnlockMovement()
        {
            canMove = true;
        }

        #region Movement Event
        void Moving(InputAction.CallbackContext context)
        {
            if(!canMove)
                return;
            speed = context.ReadValue<float>();
            if(speed < 0)
                characterRoot.localScale = new Vector3(-1, 1, 1);
            else
                characterRoot.localScale = Vector3.one;      
                
            spineAnimator.PlayOneShotAnimation(walkStartName, 0, walkName);   
        }
        void Stopping(InputAction.CallbackContext context)
        {
            speed = 0;
            if(spineAnimator.IsState(walkName))
                spineAnimator.PlayOneShotAnimation(walkEndName, 0, idleName);
        }
        #endregion

        #region Whisper Event
        void StartRecording(InputAction.CallbackContext context)
        {
            speed = 0;
            if(isRecording || recordingCooling)
                return;
            isRecording = true;
            //Record with Audio Source
            noteRecorder.BeginRecording(analyzer);
            //Change Animation
            spineAnimator.PlayOneShotAnimation(chantStartName, 0, chantName);

            PlayerWhisperingEvent.Call_OnPlayerStartWhispering(analyzer);
            AudioManager.Instance.PlaySFX(sfxWhisperData.name, 0.5f);

            LockMovement();
        }
        void StopRecording(InputAction.CallbackContext context)
        {
            if(!isRecording)
                return;
            isRecording = false;
            noteRecorder.EndRecording();
            //Change Animation
            spineAnimator.PlayOneShotAnimation(chantEndName, 0, idleName);

            PlayerWhisperingEvent.Call_OnPlayerStopWhispering();
            AudioManager.Instance.PlaySFX(sfxWhisperEndData.name, 0.5f);

            StartCoroutine(coroutineRecordingCoolDown(recordingTimer >= maxRecordingSec));
        }
        #endregion

        IEnumerator coroutineRecordingCoolDown(bool isTired)
        {
            recordingCooling = true;
            if(isTired)
                spineAnimator.PlayOneShotAnimation(tiredStartName, 0, tiredName);

            yield return new WaitForSeconds(isTired?recordingBreakCoolDown:recordingCoolDown);
            recordingTimer = 0;
            if(isTired)
                spineAnimator.PlayOneShotAnimation(tiredEndName, 0, idleName);

            recordingCooling = false;
            UnlockMovement();
        }   
    }
}
