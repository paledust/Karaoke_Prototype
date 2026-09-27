using UnityEngine;

namespace WhisperPrototype
{
    public class WhisperDecoder : MonoBehaviour
    {
        [SerializeField] private Transform showingRoot;
        private SpriteRenderer showingSprite;

        void Awake()
        {
            showingSprite = new GameObject("whisper_icon").AddComponent<SpriteRenderer>();
            showingSprite.transform.parent = showingRoot;
            showingSprite.transform.localPosition = Vector3.zero;
            showingSprite.transform.localScale = Vector3.one;

        }
        public bool DecodeWhisper(int[] notes)
        {
            var whisper = WhisperingManager.GetWhisper(notes);

            if(whisper != null)
            {
                
            }

            return whisper != null;
        }
    }
}