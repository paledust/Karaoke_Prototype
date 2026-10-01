using System.Collections;
using CoroutineUtil;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    [RequireComponent(typeof(Statue))]
    public class StatueComponent_RepeatSinging : MonoBehaviour
    {
        [SerializeField] private float playingFreq;

        private Statue statue;
        private CoroutineExcuter whisperPlayer;

        void Awake()
        {
            statue = GetComponent<Statue>();
            whisperPlayer = new CoroutineExcuter(this);
        }
        void OnEnable()
        {
            statue.OnPlayerEnter += OnPlayerEnter;
            statue.OnPlayerExit += OnPlayerExit;
        }
        void OnDisable()
        {
            statue.OnPlayerEnter -= OnPlayerEnter;
            statue.OnPlayerExit -= OnPlayerExit;
        }
        void OnPlayerEnter()
        {
            whisperPlayer.Excute(coroutineRepeatSFX());
        }
        void OnPlayerExit()
        {
            whisperPlayer.Abort();
        }
        IEnumerator coroutineRepeatSFX()
        {
            while(true)
            {
                statue.PlayWhisper();
                yield return new WaitForSeconds(1/playingFreq);
            }
        }
    }
}