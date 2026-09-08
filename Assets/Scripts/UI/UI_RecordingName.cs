using TMPro;
using UnityEngine;

public class UI_RecordingName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtDevice;
    void OnEnable()
    {
        RecordingControl.E_OnRecordingDeviceUpdate += RefreshRecordingName;
    }

    void OnDisable()
    {
        RecordingControl.E_OnRecordingDeviceUpdate -= RefreshRecordingName;
    }

    void RefreshRecordingName(string deviceName)
    {
        txtDevice.text = deviceName;
    }
}
