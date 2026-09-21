using UnityEngine;
using UnityEngine.Serialization;

public class BallControl : MonoBehaviour
{
    public enum BaseNote
    {
        C = 0,
        D = 1,
        E = 2,
        F = 3,
        G = 4,
        A = 5,
        B = 6,
    }
    [Header("Pitch Level")]
    [SerializeField] private BaseNote baseNote = BaseNote.A;
    [SerializeField] private int baseOctave = 2;
    [SerializeField] private int pitchRange = 14;
    
    [Header("Transform")]
    [SerializeField] private Transform ballTrans;
    [SerializeField] private float lerp = 1;
    [Header("Move Control")]
    [SerializeField] private Vector2 moveRange;
    [Header("Size Control")]
    [SerializeField] private float size;
    [SerializeField] private float sizeScale = 10;
    [Header("Audio Analyzer")]
    [SerializeField] private AudioAnalyzer audioAnalyzer;
    
    // Update is called once per frame
    void Update()
    {
        Vector2 pitchRange = AudioAnalyzer.GetFreqRange((int)baseNote, baseOctave, this.pitchRange);
        float normalizedPitch = Mathf.InverseLerp(pitchRange.x, pitchRange.y, audioAnalyzer.m_freq);
        
        Vector2 pos = ballTrans.position;
        pos.y = Mathf.Lerp(moveRange.x, moveRange.y, normalizedPitch);
        ballTrans.position = Vector2.Lerp(ballTrans.position, pos, Time.deltaTime * lerp);
        ballTrans.localScale = Vector3.Lerp(ballTrans.localScale, Vector3.one * Mathf.Lerp(0.5f, 2, audioAnalyzer.m_volumeLevel*sizeScale), Time.deltaTime * lerp);
    }
}