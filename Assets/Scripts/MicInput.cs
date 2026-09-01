using UnityEngine;

public class MicInput : MonoBehaviour
{
    [Header("Recording Source")]
    [SerializeField] private Replayer replayer;
    
    private bool isRecording = false;
    private bool isReplaying = false;
    private string deviceName = "";
    private AudioClip recordingClip;

    public float RecordVolume { get; private set; } = 0;

    void Start()
    {
        isRecording = false;
        recordingClip = null;
        deviceName = Microphone.devices[0];
        Debug.Log($"recording device {deviceName}");
    }

    void Update()
    {
        if (recordingClip != null)
        {
            float[] samples = new float[64];
            if (!isReplaying)
            {
                int micPos = Microphone.GetPosition(deviceName);
                int startPos = micPos - samples.Length;
                if (startPos > 0)
                {
                    recordingClip.GetData(samples, startPos);
                }
            }
            else
            {
                recordingClip.GetData(samples, replayer.GetCurrentSamplePos());
            }
            
            RecordVolume = GetRMSVolume(samples);
            
        }
        else
        {
            RecordVolume = 0;
        }
    }

    float GetRMSVolume(float[] samples)
    {
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
            sum += samples[i] * samples[i];
        return Mathf.Sqrt(sum / samples.Length);
    }
    public void SwitchRecording()
    {
        if (isRecording)
            EndRecording();
        else
        {
            isRecording = true;
            recordingClip = Microphone.Start(deviceName, true, 4, AudioSettings.outputSampleRate);
            recordingClip.name = "recording";
            replayer.AssignClip(recordingClip);
        }
    }

    public void LoadClip(AudioClip clip)
    {
        EndRecording();
        recordingClip = clip;
        isReplaying = true;
    }

    void EndRecording()
    {
        isRecording = false;
        recordingClip = null;
        Microphone.End(deviceName);
    }
}