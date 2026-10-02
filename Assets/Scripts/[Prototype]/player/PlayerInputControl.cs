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

        [Header("Audio Source")]
        [SerializeField] private AudioAnalyzer analyzer;

        [Header("Recording Source")]
        [SerializeField] private float maxRecordingSec = 3;
        [SerializeField] private float recordingCoolDown = 1;
        [SerializeField] private AudioSource recordingSource;

        [Header("Animation")]
        [SerializeField] private Animator animator;

        [Header("Audio")]
        [SerializeField] private AudioData_SO sfxWhisperData;
        [SerializeField] private AudioData_SO sfxWhisperEndData;

        private bool isRecording;
        private bool recordingCooling;
        private float recordingTimer;
        private float speed;
        private string device;
        private AudioClip recordingClip;

        private const string ANIM_SINGING_BOOL = "IsSinging";
        private const string TIRING_STATE = "tiring";
        private const string ANIM_TIRED_BOOL = "IsTired";

        void Awake()
        {
            isRecording = false;
        }
        void Start()
        {
            device = Microphone.devices[0];
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

        #region Movement Event
        void Moving(InputAction.CallbackContext context)
        {
            speed = context.ReadValue<float>();
            if(speed < 0)
                characterRoot.localScale = new Vector3(-1, 1, 1);
            else
                characterRoot.localScale = Vector3.one;         
        }
        void Stopping(InputAction.CallbackContext context)
        {
            speed = 0;
        }
        #endregion

        #region Whisper Event
        void StartRecording(InputAction.CallbackContext context)
        {
            if(isRecording || recordingCooling)
                return;
            isRecording = true;
            //Change Animation
            animator.SetBool(ANIM_SINGING_BOOL, true);
            //Record with Audio Source
            recordingSource.Stop();
            recordingClip = Microphone.Start(device, false, 5, AudioSettings.outputSampleRate);
            while (!(Microphone.GetPosition(device) > 0.02f)) {}
            recordingSource.clip = recordingClip;
            recordingSource.loop = true;
            recordingSource.Play();

            PlayerWhisperingEvent.Call_OnPlayerStartWhispering(analyzer);
            AudioManager.Instance.PlaySFX(sfxWhisperData.name, 0.5f);
        }
        void StopRecording(InputAction.CallbackContext context)
        {
            if(!isRecording)
                return;
            isRecording = false;
            animator.SetBool(ANIM_SINGING_BOOL, false);
            recordingSource.Stop();
            Microphone.End(device);

            PlayerWhisperingEvent.Call_OnPlayerStopWhispering();
            AudioManager.Instance.PlaySFX(sfxWhisperEndData.name, 0.5f);

            StartCoroutine(coroutineRecordingCoolDown(recordingTimer >= maxRecordingSec));
        }
        #endregion

        IEnumerator coroutineRecordingCoolDown(bool isTired)
        {
            recordingCooling = true;
            if(isTired)
            {
                animator.SetBool(ANIM_TIRED_BOOL, true);
                animator.Play(TIRING_STATE);
            }
            yield return new WaitForSeconds(isTired?recordingCoolDown:0.75f);
            recordingTimer = 0;
            if(isTired)
            {
                animator.SetBool(ANIM_TIRED_BOOL, false);
            }
            recordingCooling = false;
        }   
    }
}
