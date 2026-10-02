using UnityEngine;
using UnityEngine.UI;

namespace WhisperPrototype.UI
{
    using Book;
    
    public class UI_BookControl : MonoBehaviour
    {
        [Header("Hint")]
        [SerializeField] private Image hint;

        [Header("Book Control")]
        [SerializeField] private BookOfWhisper book;
        [SerializeField] private Button btnBookOpen;
        [SerializeField] private Button btnBookClose;
        private CanvasGroup canvasGroup;
        private WhisperWordData_SO pendingWhisper;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            btnBookOpen.onClick.AddListener(BtnOpenBook);
            btnBookClose.onClick.AddListener(BtnCloseBook);

            PlayerWhisperingEvent.E_OnPlayerHearingNewWhisper += HandleNewWhisper;
        }
        void OnDestroy()
        {
            btnBookOpen.onClick.RemoveListener(BtnOpenBook);
            btnBookClose.onClick.RemoveListener(BtnCloseBook);

            PlayerWhisperingEvent.E_OnPlayerHearingNewWhisper -= HandleNewWhisper;
        }
        void HandleNewWhisper(WhisperWordData_SO whisper)
        {
            hint.gameObject.SetActive(true);
            pendingWhisper = whisper;
        }
        async void BtnOpenBook()
        {
            canvasGroup.interactable = false;
            await book.OpenBook(pendingWhisper);
            pendingWhisper = null;
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
