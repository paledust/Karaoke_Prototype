using UnityEngine;
using WhisperPrototype.Villager;

namespace WhisperPrototype.Spirit
{
    public class SpiritHugger : MonoBehaviour
    {
        [SerializeField] private float followSpeed;
        [SerializeField] private GameObject spiritHugging;
        private GameObject player;

        public void Init(GameObject player)
        {
            this.player = player;    
        }
        void Update()
        {
            Vector3 playerPos = player.transform.position;
            float diff = playerPos.x - transform.position.x;
            float speedScale = Mathf.Lerp(0, followSpeed, Mathf.Abs(diff)) * (diff>0?1:-1);
            transform.position += Vector3.right * followSpeed * speedScale * Time.deltaTime;
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out VillagerInPain villager))
            {
                Destroy(gameObject);
                var hugging = Instantiate(spiritHugging);
                hugging.transform.position = villager.transform.position;
                Destroy(hugging, 6);
            }
        }
    }
}