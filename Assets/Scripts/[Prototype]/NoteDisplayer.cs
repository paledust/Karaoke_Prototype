using DG.Tweening;
using UnityEngine;

public class NoteDisplayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer noteRender;
    [SerializeField] private float startScaleMulti = 0.5f;

    [SerializeField] private float shakingFreq = 10;
    [SerializeField] private float shakingAmp = 20;
    private Vector3 originalScale;
    private bool isLong = false;
    private bool isConfirm = false;

    public bool IsLong => isLong;
    public bool IsConfirm => isConfirm;

    void Awake()
    {
        isLong = false;
        isConfirm = false;

        originalScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originalScale * startScaleMulti, 0.2f).SetEase(Ease.OutBack);
    }
    void Update()
    {
        float angle = shakingAmp * Mathf.Sin(Time.time * Mathf.PI * 2 * shakingFreq);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0,0,angle), Time.deltaTime*10);
    }
    public void ConfirmNote()
    {
        isConfirm = true;
        transform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack);
    }
    public void UpdateNote(Color color)
    {
        noteRender.color = color;
    }
    public void SwapToLongNote(Sprite noteSprite)
    {
        isLong = true;
        noteRender.sprite = noteSprite;
        transform.DOKill();
        transform.DOPunchScale(Vector3.one * 1.2f, 0.15f, 1);
    }
    public void ReleaseNote()
    {
        this.enabled = false;
        transform.DOLocalRotate(Vector3.zero, 0.1f);
    }
}
