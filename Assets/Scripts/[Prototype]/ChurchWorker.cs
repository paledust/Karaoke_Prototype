using UnityEngine;

namespace WhisperPrototype
{
    public class ChurchWorker : MonoBehaviour
    {
        [SerializeField] private WhisperWordData_SO whispering;
        void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                if(other.TryGetComponent(out WhisperHandler whisperHandler))
                {
                    whisperHandler.HearingWhisper(whispering);
                }
            }
        }
    }
}
