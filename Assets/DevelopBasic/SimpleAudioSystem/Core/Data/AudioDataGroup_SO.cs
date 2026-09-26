using UnityEngine;
using System;
using SimpleShuffle;

namespace SimpleAudioSystem
{
    [CreateAssetMenu(fileName = "AudioDataCollection_SO", menuName = "Audio/AudioSystem/AudioDataGroup")]
    public class AudioDataGroup_SO : AudioData_SO
    {
        [SerializeField] private AudioClip[] audioClips;
        [NonSerialized] private int playIndex = 0;

        void OnEnable()
        {
            playIndex = 0;
            ShuffleHelper.Shuffle(ref audioClips);
        }
        internal override AudioClip GetAudioClip()
        {
            var clip = audioClips[playIndex];
            playIndex ++;
            if(playIndex >= audioClips.Length)
            {
                playIndex = 0;
                ShuffleHelper.Shuffle(ref audioClips);
            }
            return clip;
        }
    }
}