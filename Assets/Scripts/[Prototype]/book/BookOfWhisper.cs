using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WhisperPrototype.Book
{
    public class BookOfWhisper : MonoBehaviour
    {
        [SerializeField] private Animation animeBook;
        private const string ANIME_OPEN_KEY = "book_open";
        private const string ANIME_CLOSE_KEY = "book_close";

        public async UniTask OpenBook()
        {
            gameObject.SetActive(true);
            animeBook.Play(ANIME_OPEN_KEY);
            await UniTask.WaitForSeconds(animeBook[ANIME_OPEN_KEY].length);
        }
        public async UniTask CloseBook()
        {
            animeBook.Play(ANIME_CLOSE_KEY);
            await UniTask.WaitForSeconds(animeBook[ANIME_CLOSE_KEY].length);
            gameObject.SetActive(false);
        }
    }
}
