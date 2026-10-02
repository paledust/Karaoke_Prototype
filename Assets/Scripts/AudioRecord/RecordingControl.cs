using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RecordingControl : MonoBehaviour
{
    [Header("Recording Source")]
    [SerializeField] private AudioSource recordingSource;
    [SerializeField, ShowOnly] private bool isRecording = false;
    
    private AudioClip[] recordingClip;
    private string device = "";
    private int deviceIndex = 0;
    private int clipIndex = 0;

    private const string CLIP_SAVE_PATH = "Assets/MyRecording";
    private const int MAX_CLIP_LENGTH = 3;
    public static Action<string> E_OnRecordingDeviceUpdate;
    public static void Call_OnRecordingDeviceUpdate(string deviceName) => E_OnRecordingDeviceUpdate?.Invoke(deviceName);
    
    void Start()
    {
        isRecording = false;
        device = Microphone.devices[0];
        recordingClip = new AudioClip[MAX_CLIP_LENGTH];
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
    void BeginRecording()
    {
        //Record with Audio Source
        recordingSource.Stop();
        recordingClip[clipIndex] = Microphone.Start(device, true, 5, AudioSettings.outputSampleRate);
        while (!(Microphone.GetPosition(device) > 0.01f)) {}
        recordingSource.clip = recordingClip[clipIndex];
        recordingSource.Play();
    }

    public void Btn_SwitchRecording()
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
            SaveClipToAsset();

            clipIndex ++;
            clipIndex %= MAX_CLIP_LENGTH;
        }
    }
    public void Btn_BeginReplay(int index)
    {
        if(isRecording)
        {
            isRecording = false;
            Microphone.End(device);
            SaveClipToAsset();

            clipIndex ++;
            clipIndex %= MAX_CLIP_LENGTH;
        }
        if(recordingClip[index] == null)
            return;
        recordingSource.Stop();
        recordingSource.clip = recordingClip[index];
        recordingSource.Play();
    }
    void SaveClipToAsset()
    {
        // SaveWav.TrimSilence(recordingClip[clipIndex], 0.01f);
        SaveWav.Save($"{CLIP_SAVE_PATH}/recording_{clipIndex}.wav", recordingClip[clipIndex]);  

#if UNITY_EDITOR
        // Create the asset file
        AssetDatabase.ImportAsset($"{CLIP_SAVE_PATH}/recording_{clipIndex}.wav");
        // Save changes to disk and refresh the Editor
        AssetDatabase.Refresh();      
#endif
    }
}