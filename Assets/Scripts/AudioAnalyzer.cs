using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAnalyzer : MonoBehaviour
{
    [SerializeField] private string frequency = "detected frequency";
    [SerializeField] private string note = "detected note";
    [SerializeField] private float volumeGate = 0.01f;
    private AudioSource _audio;
    
    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);

    public int m_noteIndex { get; private set; } = 0;
    public float m_volumeLevel { get; private set; } = 0;
    public float m_freq { get; private set; } = 0;
    
    private readonly static string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    void Start()
    {
        _audio = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //Volume Detection
        if (!_audio.isPlaying)
        {
            note = "unknown";
            m_noteIndex = 0;
            m_freq = 0;
            m_volumeLevel = 0;
            
            return;
        }
        float[] samples = new float[1024];
        _audio.GetOutputData(samples, 0);
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        sum /= samples.Length;
        sum = Mathf.Sqrt(sum);
        m_volumeLevel = sum;
        
        //Pitch Detection
        if (m_volumeLevel < volumeGate)
        {
            note = "unknown";
            m_noteIndex = 0;
            m_freq = 0;
            return;
        }
        float deviation = 0.0f;
        float freq = PitchDetectorGetFreq(0);
        m_freq = freq;
        frequency = freq.ToString() + " Hz";

        if (freq > 0.0f)
        {
            float noteval = 57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f);
            float f = Mathf.Floor(noteval + 0.5f);
            
            m_noteIndex = (int)f % 12;
            int octave = (int)Mathf.Floor((noteval + 0.5f) / 12.0f);
            note = noteNames[m_noteIndex] + " " + octave;
        }
        else
        {
            note = "unknown";
            m_noteIndex = 0;
            m_freq = 0;
        }
    }
}