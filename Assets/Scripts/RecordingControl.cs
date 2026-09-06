using System.Runtime.InteropServices;
using UnityEngine;

public class RecordingControl : MonoBehaviour
{
    [Header("Recording Source")]
    [SerializeField] private AudioSource recordingSource;
    [SerializeField, ShowOnly] private bool isRecording = false;
    private string device = "";
    private AudioClip recordingClip;

    void Start()
    {
        isRecording = false;
        device = Microphone.devices[0];
        Debug.Log($"recording device {device}");
    }
    public void SwitchRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            //Record with Audio Source
            recordingSource.Stop();
            recordingSource.clip = Microphone.Start(device, true, 1, AudioSettings.outputSampleRate);
            recordingSource.Play();
            
            //Set delay on recording source
            int dspBufferSize, dspNumBuffers;
            AudioSettings.GetDSPBufferSize(out dspBufferSize, out dspNumBuffers);
            recordingSource.timeSamples = (Microphone.GetPosition(device) + AudioSettings.outputSampleRate - 3 * dspBufferSize * dspNumBuffers) % AudioSettings.outputSampleRate;
        }
        else
        {
            isRecording = false;
            recordingSource.Stop();
            Microphone.End(device);
        }
    }
}