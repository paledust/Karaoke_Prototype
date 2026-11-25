using System.Collections;
using CoroutineUtil;
using SimpleAudioSystem;
using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource playingSource;
        [SerializeField] private AudioData_SO whisperAudio;
        [SerializeField] private Vector2 playingIntersect;
        private CoroutineExcuter player;
        void Start()
        {
            player = new CoroutineExcuter(this);
            player.Excute(coroutineCountDown());
        }

        public void StopPlaying()
        {
            player.Abort();
        }
        
        IEnumerator coroutineCountDown()
        {
            float delay = playingIntersect.GetRndValueInVector2Range();
            yield return new WaitForSeconds(delay);
            AudioManager.Instance.PlaySFX(playingSource, whisperAudio.name, 1);
            player.Excute(coroutineCountDown());
        }
    }
}