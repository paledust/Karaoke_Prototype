using System;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype.Level
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private AudioData_SO ambience;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            AudioManager.Instance.PlayAmbience(ambience.name, true, 0.5f, 1);
        }
    }
}
