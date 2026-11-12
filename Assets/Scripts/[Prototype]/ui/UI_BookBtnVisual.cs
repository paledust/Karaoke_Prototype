using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WhisperPrototype.UI
{
    public class UI_BookBtnVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image bookOutline;
        [SerializeField] private Image symbolGlow;
        private Sequence sequence;

        void Start()
        {
            Color bookColor = bookOutline.color, symbolColor = symbolGlow.color;
            bookColor.a = 0;
            symbolColor.a = 0;

            bookOutline.color = bookColor;
            symbolGlow.color = symbolColor;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(sequence!=null)
                sequence.Kill();
            sequence.Append(bookOutline.DOFade(1, 0.2f))
                    .Join(symbolGlow.DOFade(1, 0.2f));

        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if(sequence!=null)
                sequence.Kill();
            sequence.Append(bookOutline.DOFade(0, 0.2f))
                    .Join(symbolGlow.DOFade(0, 0.2f));
        }
    }
}
