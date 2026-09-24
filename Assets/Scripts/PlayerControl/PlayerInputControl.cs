using UnityEngine;
using UnityEngine.InputSystem;
using AudioAnalysis;

public class PlayerInputControl : MonoBehaviour
{
    [SerializeField] private InputAction recordingKey;

    [Header("Audio Source")]
    [SerializeField] private AudioAnalyzer analyzer;

    [Header("Recording Source")]
    [SerializeField] private AudioSource recordingSource;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private bool isRecording;
    private string device;
    private AudioClip recordingClip;

    private const string ANIM_SINGING_BOOL = "IsSinging";

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
        recordingKey.Enable();
    }
    void OnDisable()
    {
        recordingKey.performed -= StartRecording;
        recordingKey.canceled -= StopRecording;
        recordingKey.Disable();
    }
    void StartRecording(InputAction.CallbackContext context)
    {
        if(isRecording)
            return;
        isRecording = true;
        //Change Animation
        animator.SetBool(ANIM_SINGING_BOOL, true);
        //Record with Audio Source
        recordingSource.Stop();
        recordingClip = Microphone.Start(device, true, 1, AudioSettings.outputSampleRate);
        while (!(Microphone.GetPosition(device) > 0.02f)) {}
        recordingSource.clip = recordingClip;
        recordingSource.loop = true;
        recordingSource.Play();

        PlayerSingingEvent.Call_OnPlayerStartToSing(analyzer);
    }
    void StopRecording(InputAction.CallbackContext context)
    {
        if(!isRecording)
            return;
        isRecording = false;
        animator.SetBool(ANIM_SINGING_BOOL, false);
        recordingSource.Stop();
        Microphone.End(device);

        PlayerSingingEvent.Call_OnPlayerStopSinging();
    }
}
