using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class RecordingControl : MonoBehaviour
{
    [Header("Recording Source")]
    [SerializeField] private AudioSource recordingSource;
    [SerializeField, ShowOnly] private bool isRecording = false;
    
    private AudioClip recordingClip;
    private string device = "";
    private int deviceIndex = 0;

    public static Action<string> E_OnRecordingDeviceUpdate;
    public static void Call_OnRecordingDeviceUpdate(string deviceName) => E_OnRecordingDeviceUpdate?.Invoke(deviceName);
    
    void Start()
    {
        isRecording = false;
        device = Microphone.devices[0];
        Debug.Log($"recording device {device}");
        E_OnRecordingDeviceUpdate?.Invoke(device);
    }
    public void NextDevice()
    {
        Microphone.End(device);
        deviceIndex ++;
        if (deviceIndex >= Microphone.devices.Length)
            deviceIndex = 0;
        device = Microphone.devices[deviceIndex];
        if(isRecording)
            BeginRecording();
        
        E_OnRecordingDeviceUpdate?.Invoke(device);
    }
    public void PreviousDevice()
    {
        Microphone.End(device);
        deviceIndex --;
        if (deviceIndex < 0)
            deviceIndex = Microphone.devices.Length - 1;
        device = Microphone.devices[deviceIndex];
        if (isRecording)
            BeginRecording();
        
        E_OnRecordingDeviceUpdate?.Invoke(device);
    }
    public void SwitchRecording()
    {
        if (!isRecording)
        {
            isRecording = true;
            BeginRecording();
        }
        else
        {
            isRecording = false;
            recordingSource.Stop();
            Microphone.End(device);
        }
    }

    void BeginRecording()
    {
        //Record with Audio Source
        recordingSource.Stop();
        recordingSource.clip = Microphone.Start(device, true, 1, AudioSettings.outputSampleRate);
        recordingSource.Play();
            
        //Set delay on recording source
        int dspBufferSize, dspNumBuffers;
        AudioSettings.GetDSPBufferSize(out dspBufferSize, out dspNumBuffers);
        recordingSource.timeSamples = (Microphone.GetPosition(device) + AudioSettings.outputSampleRate - 3 * dspBufferSize * dspNumBuffers) % AudioSettings.outputSampleRate;
    }
}