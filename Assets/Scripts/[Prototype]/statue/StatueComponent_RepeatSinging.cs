using System.Collections;
using CoroutineUtil;
using UnityEngine;

namespace WhisperPrototype.Statue
{
    [RequireComponent(typeof(Statue))]
    public class StatueComponent_RepeatSinging : MonoBehaviour
    {
        [SerializeField] private float playerCycle;

        private Statue statue;
        private CoroutineExcuter whisperPlayer;

        void Awake()
        {
            statue = GetComponent<Statue>();
            whisperPlayer = new CoroutineExcuter(this);
        }
        void Start()
        {
            whisperPlayer.Excute(coroutineRepeatSFX());
        }
        IEnumerator coroutineRepeatSFX()
        {
            while(true)
            {
                statue.PlayWhisper();
                yield return new WaitForSeconds(playerCycle);
            }
        }
    }
}