using System.Runtime.InteropServices;
using UnityEngine;

namespace AudioAnalysis
{
    public class AudioAnalyzer : MonoBehaviour
    {
        [SerializeField] private AudioSource targetAudio;
        [SerializeField] private float smoothDamp = 1;

        [DllImport("AudioPluginDemo")]
        private static extern float PitchDetectorGetFreq(int index);

        public int m_noteIndex { get; private set; } = 0;
        public int m_rawPitchIndex {get; private set;} = 0;
        public float m_volumeLevel { get; private set; } = 0;
        public float m_freq { get; private set; } = 0;
        public string m_note {get; private set; } = "unknown";
        public float m_smoothFreq{get; private set; } = 0;
        public int m_smoothRawPitch{get; private set;} = 0;
        private readonly static string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        
        // Update is called once per frame
        void Update()
        {
            //Volume Detection
            if (!targetAudio.isPlaying)
            {
                ClearNote();
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
            m_smoothFreq = Mathf.Lerp(m_freq, m_smoothFreq, Time.deltaTime * smoothDamp);

            if (m_freq > 0.0f)
            {
                float noteval = FreqToNoteValue(m_freq);
                int f = Mathf.FloorToInt(noteval + 0.5f);
                
                m_rawPitchIndex = f;
                m_noteIndex = m_rawPitchIndex % 12;
                int octave = Mathf.FloorToInt((noteval + 0.5f) / 12.0f);
                m_note = noteNames[m_noteIndex] + " " + octave;

                float smoothNoteVal = FreqToNoteValue(m_smoothFreq);
                m_smoothRawPitch = Mathf.FloorToInt(smoothNoteVal + 0.5f);
            }
            else
                ClearNote();
        }

        public static float FreqToNoteValue(float freq) => 57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f);
        public static Vector2 GetFreqRange(int noteLevel, int octave, int levelRange)
        {
            float refValue = Mathf.Log10(2f);
            int startNote = noteLevel + octave * 12;
            int endNote = startNote + levelRange;
            float startFreq = Mathf.Pow(10, (startNote - 57) * refValue / 12.0f) * 440f;
            float endFreq = Mathf.Pow(10, (endNote - 57) * refValue / 12.0f) * 440f;
            return new Vector2(startFreq, endFreq);
        }
        void ClearNote()
        {
            m_note = "unknown";
            m_noteIndex = 0;
            m_rawPitchIndex = 0;
            m_freq = 0;
            m_smoothRawPitch = 0;
            m_smoothFreq = 0;
        }
    }
}