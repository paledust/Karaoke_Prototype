using System.Runtime.InteropServices;
using UnityEngine;

namespace AudioAnalysis
{
    public class AudioAnalyzer : MonoBehaviour
    {
        [SerializeField] private AudioSource targetAudio;

        [DllImport("AudioPluginDemo")]
        private static extern float PitchDetectorGetFreq(int index);

        public int m_noteIndex { get; private set; } = 0;
        public int m_rowPitchIndex {get; private set;} = 0;
        public float m_volumeLevel { get; private set; } = 0;
        public float m_freq { get; private set; } = 0;
        public string m_note {get; private set; } = "unknown";
        private readonly static string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        // Update is called once per frame
        void Update()
        {
            //Volume Detection
            if (!targetAudio.isPlaying)
            {
                m_note = "unknown";
                m_noteIndex = 0;
                m_rowPitchIndex = 0;
                m_freq = 0;
                m_volumeLevel = 0;
                
                return;
            }
            float[] samples = new float[1024];
            targetAudio.GetOutputData(samples, 0);
            float sum = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                sum += samples[i] * samples[i];
            }
            sum /= (0f+samples.Length);
            sum = Mathf.Sqrt(sum);
            m_volumeLevel = sum;
            
            //Pitch Detection
            m_freq = PitchDetectorGetFreq(0);

            if (m_freq > 0.0f)
            {
                float noteval = 57.0f + 12.0f * Mathf.Log10(m_freq / 440.0f) / Mathf.Log10(2.0f);
                int f = Mathf.FloorToInt(noteval + 0.5f);
                
                m_rowPitchIndex = f;
                m_noteIndex = f % 12;
                int octave = Mathf.FloorToInt((noteval + 0.5f) / 12.0f);
                m_note = noteNames[m_noteIndex] + " " + octave;
            }
            else
            {
                m_note = "unknown";
                m_noteIndex = 0;
                m_rowPitchIndex = 0;
                m_freq = 0;
            }
        }

        public static Vector2 GetFreqRange(int noteLevel, int octave, int levelRange)
        {
            float refValue = Mathf.Log10(2f);
            int startNote = noteLevel + octave * 12;
            int endNote = startNote + levelRange;
            float startFreq = Mathf.Pow(10, (startNote - 57) * refValue / 12.0f) * 440f;
            float endFreq = Mathf.Pow(10, (endNote - 57) * refValue / 12.0f) * 440f;
            return new Vector2(startFreq, endFreq);
        }
    }
}