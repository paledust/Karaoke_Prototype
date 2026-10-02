using UnityEngine;
using UnityEngine.UI;

namespace WhisperPrototype.UI
{
    using Book;
    
    public class UI_BookControl : MonoBehaviour
    {
        [SerializeField] private BookOfWhisper book;
        [SerializeField] private Button btnBookOpen;
        [SerializeField] private Button btnBookClose;
        private CanvasGroup canvasGroup;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            btnBookOpen.onClick.AddListener(BtnOpenBook);
            btnBookClose.onClick.AddListener(BtnCloseBook);
        }
        void OnDestroy()
        {
            btnBookOpen.onClick.RemoveListener(BtnOpenBook);
            btnBookClose.onClick.RemoveListener(BtnCloseBook);
        }
        async void BtnOpenBook()
        {
            canvasGroup.interactable = false;
            await book.OpenBook();
            btnBookOpen.gameObject.SetActive(false);
            btnBookClose.gameObject.SetActive(true);
            canvasGroup.interactable = true;
        }
        async void BtnCloseBook()
        {
            canvasGroup.interactable = false;
            await book.CloseBook();
            btnBookClose.gameObject.SetActive(false);
            btnBookOpen.gameObject.SetActive(true);
            canvasGroup.interactable = true;
        }
    }
}
