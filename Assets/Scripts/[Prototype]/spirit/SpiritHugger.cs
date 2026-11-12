using UnityEngine;
using WhisperPrototype.Villager;

namespace WhisperPrototype.Spirit
{
    public class SpiritHugger : MonoBehaviour
    {
        [SerializeField] private float followSpeed;
        [SerializeField] private GameObject spiritHugging;
        [SerializeField] private Transform renderRoot;
        [SerializeField] private Animation anime;

        private bool isDead;
        private float lifeTimer;
        private Vector3 targetPosition;
        private GameObject player;

        private static readonly Vector3 flipDir = new Vector3(-1,1,1);
        private const string KEY_DISSOLVE_IN = "spirit_dissolve_in";
        private const string KEY_DISSOLVE_OUT = "spirit_dissolve_out";

        public void Init(GameObject player, float duration)
        {
            anime.Play(KEY_DISSOLVE_IN);
            this.player = player;
            this.lifeTimer = duration;
            transform.position = player.transform.position;
            targetPosition = player.transform.position;
        }
        void Update()
        {
            // Follow Player
            if(!isDead)
                targetPosition = player.transform.position;

            float diff = targetPosition.x - transform.position.x;
            if(Mathf.Abs(diff)>0.1f)
            {
                float speedScale = Mathf.Lerp(0, followSpeed, Mathf.Abs(diff)) * (diff>0?1:-1);
                transform.position += Vector3.right * followSpeed * speedScale * Time.deltaTime;
            }
            renderRoot.localScale = diff>0?Vector3.one : flipDir;

            // Life Time
            lifeTimer-=Time.deltaTime;
            if(lifeTimer<=0 && !isDead)
            {
                isDead = true;
                anime.Play(KEY_DISSOLVE_OUT);
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out VillagerInPain villager))
            {
                Destroy(gameObject);
                var hugging = Instantiate(spiritHugging).GetComponent<HuggingSequence>();
                hugging.Init(villager);
                hugging.transform.position = villager.transform.position;
            }
        }
    }
}