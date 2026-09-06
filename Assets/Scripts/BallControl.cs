using UnityEngine;
using UnityEngine.Serialization;

public class BallControl : MonoBehaviour
{
    [SerializeField] private float moveScale;
    [SerializeField] private float sizeScale = 10;
    [SerializeField] private Transform ballTrans;
    [FormerlySerializedAs("micInput")] [SerializeField] private RecordingControl recordingControl;

    [SerializeField] private float lerp = 1;
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = ballTrans.position;
        // pos.y = recordingControl.RecordPitch * moveScale;
        ballTrans.position = Vector2.Lerp(ballTrans.position, pos, Time.deltaTime * lerp);
        // ballTrans.localScale = Vector3.Lerp(ballTrans.localScale, Vector3.one * Mathf.Lerp(0.5f, 2, recordingControl.RecordVolume*sizeScale), Time.deltaTime * lerp);
    }
}