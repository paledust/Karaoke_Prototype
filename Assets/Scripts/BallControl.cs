using UnityEngine;
using UnityEngine.Serialization;

public class BallControl : MonoBehaviour
{
    [SerializeField] private Vector2 pitchRange;
    [SerializeField] private float moveScale;
    [SerializeField] private float sizeScale = 10;
    [SerializeField] private Transform ballTrans;
    [SerializeField] private AudioAnalyzer audioAnalyzer;
    [SerializeField] private float lerp = 1;
    
    [SerializeField] private float demo;
    // Update is called once per frame
    void Update()
    {
        Vector2 pos = ballTrans.position;
        float normalizedPitch = Mathf.InverseLerp(pitchRange.x, pitchRange.y, audioAnalyzer.m_freq);
        pos.y = normalizedPitch * moveScale;
        ballTrans.position = Vector2.Lerp(ballTrans.position, pos, Time.deltaTime * lerp);
        ballTrans.localScale = Vector3.Lerp(ballTrans.localScale, Vector3.one * Mathf.Lerp(0.5f, 2, audioAnalyzer.m_volumeLevel*sizeScale), Time.deltaTime * lerp);
    }
}