using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl : MonoBehaviour
{
    [SerializeField] private InputAction recordingKey;

    [Header("Recording Source")]
    [SerializeField] private AudioSource recordingSource;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private AudioClip recordingClip;
    private string device;
    private bool isRecording;

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
        recordingClip = Microphone.Start(device, true, 5, AudioSettings.outputSampleRate);
        while (!(Microphone.GetPosition(device) > 0.02f)) {}
        recordingSource.clip = recordingClip;
        recordingSource.Play();
    }
    void StopRecording(InputAction.CallbackContext context)
    {
        if(!isRecording)
            return;
        isRecording = false;
        animator.SetBool(ANIM_SINGING_BOOL, false);
        recordingSource.Stop();
        Microphone.End(device);
    }
}
