using System;
using UnityEngine;

public class Replayer : MonoBehaviour
{
    [SerializeField] private MicInput mic;
    private bool isPlaying;
    private AudioSource m_audio;

    private void Start()
    {
        isPlaying = false;
        m_audio = GetComponent<AudioSource>();
        m_audio.mute = true;
    }

    public void AssignClip(AudioClip clip)
    {
        m_audio.clip = clip;
    }
    public int GetCurrentSamplePos()=>m_audio.timeSamples;
    public void Play()
    {
        if (!isPlaying)
        {
            if (m_audio.clip != null)
            {
                Debug.unityLogger.Log($"Replaying {m_audio.clip.name}");
                isPlaying = true;
                m_audio.mute = false;
                m_audio.Play();
                mic.LoadClip(m_audio.clip);
            }
        }
        else
        {
            isPlaying = false;
            m_audio.Stop();
        }
    }
}
