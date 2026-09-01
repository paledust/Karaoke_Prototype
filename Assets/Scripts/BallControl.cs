using UnityEngine;

public class BallControl : MonoBehaviour
{
    [SerializeField] private float moveScale;
    [SerializeField] private Transform ballTrans;
    [SerializeField] private MicInput micInput;
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = ballTrans.position;
        pos.y = micInput.RecordVolume * moveScale;
        ballTrans.position = Vector2.Lerp(ballTrans.position, pos, Time.deltaTime * 2);        
    }
}
