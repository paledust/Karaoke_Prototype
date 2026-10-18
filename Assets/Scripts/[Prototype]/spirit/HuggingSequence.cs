using System.Collections;
using UnityEngine;

namespace WhisperPrototype
{
    using Villager;
    public class HuggingSequence : MonoBehaviour
    {
        [SerializeField] private Animation huggingAnime;
        private VillagerInPain villager;
        public void Init(VillagerInPain villager)
        {
            this.villager = villager;
            StartCoroutine(coroutineHuggingSequence());
        }
        IEnumerator coroutineHuggingSequence()
        {
            huggingAnime.Play();
            yield return new WaitForSeconds(huggingAnime.clip.length + 1f);
            villager.OnComforted();
            Destroy(gameObject);
        }
    }
}
