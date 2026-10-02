using System.Runtime.ExceptionServices;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WhisperPrototype.Book
{
    public class BookWhisperRecorder : MonoBehaviour
    {
        [SerializeField] private ParticleSystem vfxWhispering;
        [SerializeField] private SpriteRenderer whisperRenderer;
        [SerializeField] private PerRenderWhisper perRenderWhisper;

        [Header("Input")]
        [SerializeField] private InputAction recordingKey;

        [Header("Maie Mimic")]
        [SerializeField] private Animator maieAnime;

        [Header("Recording")]
        [SerializeField] private AudioSource recordingSource;

        private AudioClip recordingClip;
        private string device;
        private bool isInputRegistered = false;
        private bool isRecording = false;

        private const string ANIM_SINGING_BOOL = "IsSinging";

        void Start()
        {
            device = Microphone.devices[0];
        }
        public void CleanUp()
        {
            vfxWhispering.gameObject.SetActive(false);
            whisperRenderer.gameObject.SetActive(false);
            isRecording = false;

            if(isInputRegistered)
            {
                isInputRegistered = false;
                recordingKey.performed -= StartRecording;
                recordingKey.canceled -= StopRecording;
                recordingKey.Disable();
            }
        }
        public void PrepareForNewWhisper(WhisperWordData_SO newWhisper)
        {
            vfxWhispering.gameObject.SetActive(true);
            whisperRenderer.gameObject.SetActive(true);
            whisperRenderer.sprite = newWhisper.GetIcon();
            perRenderWhisper.dissolveStart = 0;

            if(!isInputRegistered)
            {
                isInputRegistered = true;
                recordingKey.performed += StartRecording;
                recordingKey.canceled += StopRecording;
                recordingKey.Enable();
            }
        }
        void StartRecording(InputAction.CallbackContext context)
        {
            if(isRecording)
                return;
            isRecording = true;
            //Change Animation
            maieAnime.SetBool(ANIM_SINGING_BOOL, true);

            //Record with Audio Source
            recordingSource.Stop();
            recordingClip = Microphone.Start(device, true, 1, AudioSettings.outputSampleRate);
            while (!(Microphone.GetPosition(device) > 0.02f)) {}
            recordingSource.clip = recordingClip;
            recordingSource.loop = true;
            recordingSource.Play();
        }
        void StopRecording(InputAction.CallbackContext context)
        {
            if(!isRecording)
                return;
            isRecording = false;
            maieAnime.SetBool(ANIM_SINGING_BOOL, false);
            recordingSource.Stop();
            Microphone.End(device);
        }
    }
}