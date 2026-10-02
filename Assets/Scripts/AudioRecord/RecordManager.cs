using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RecordManager : Singleton<RecordManager>
{
    [Header("Recording Source")]
    [SerializeField, ShowOnly] private bool isRecording = false;
    
    private string device = "";
    private int deviceIndex = 0;
    private AudioClip currentClip;

    public AudioClip m_currentClip => currentClip;
    
    void Start()
    {
        isRecording = false;
        device = Microphone.devices[0];
    }
    public void NextDevice()
    {
        StopRecording();
        deviceIndex ++;
        if (deviceIndex >= Microphone.devices.Length)
            deviceIndex = 0;
        device = Microphone.devices[deviceIndex];
    }
    public void PreviousDevice()
    {
        StopRecording();
        deviceIndex --;
        if (deviceIndex < 0)
            deviceIndex = Microphone.devices.Length - 1;
        device = Microphone.devices[deviceIndex];
    }
    public AudioClip BeginRecording(int length)
    {
        if(!isRecording)
        {
            isRecording = true;
            currentClip = Microphone.Start(device, true, length, AudioSettings.outputSampleRate);
        }
        return currentClip;
    }
    public void StopRecording()
    {
        if(isRecording)
        {
            isRecording = false;
            Microphone.End(device);
        }
    }
}
