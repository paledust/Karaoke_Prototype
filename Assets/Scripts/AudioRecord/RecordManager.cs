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
    private AudioClip pendingClip;
    public AudioClip m_pendingClip => pendingClip;
    
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
            pendingClip = Microphone.Start(device, true, length, AudioSettings.outputSampleRate);
        }
        return pendingClip;
    }
    public void StopRecording()
    {
        if(isRecording)
        {
            isRecording = false;
            Microphone.End(device);
        }
    }
    public void SaveCurrentClip(string name)
    {
        SaveWav.Save(name, pendingClip);
        pendingClip = null;
    }
}
