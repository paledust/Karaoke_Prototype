using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WhisperPrototype.UI
{
    public class UI_WhisperSlot : MonoBehaviour
    {
        [SerializeField] private Image iconWhisper;
        public bool isAssigned {get; private set;} = false;

        public void AssignWhisper(WhisperWordData_SO whisper)
        {
            isAssigned = true;
            iconWhisper.transform.localScale = Vector3.zero;
            iconWhisper.gameObject.SetActive(true);
            iconWhisper.sprite = whisper.GetIcon();
            iconWhisper.transform.DOKill();
            iconWhisper.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
        public void TriggerWhisper()
        {
            iconWhisper.transform.localScale = Vector3.one;
            iconWhisper.transform.DOKill();
            iconWhisper.transform.DOPunchScale(Vector3.one * 1.1f, 0.5f).SetEase(Ease.OutBack);
        }
    }
}
