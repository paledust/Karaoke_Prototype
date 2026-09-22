using DG.Tweening;
using UnityEngine;
using AudioAnalysis;

public class VoiceOrb : MonoBehaviour
{
    [SerializeField] private Gradient pitchColor;
    [SerializeField] private Vector2 volumeScale;
    [SerializeField] private SpriteRenderer sprite;

    private AudioAnalyzer targetAnalyzer;

    void OnEnable()
    {
        PlayerSingingEvent.E_OnPlayerStartToSing += OnPlayerStartSinging;
        PlayerSingingEvent.E_OnPlayerStopSinging += OnPlayerStopSinging;
    }
    void OnDisable()
    {
        PlayerSingingEvent.E_OnPlayerStartToSing -= OnPlayerStartSinging;
        PlayerSingingEvent.E_OnPlayerStopSinging -= OnPlayerStopSinging;
    }
    void Start()
    {
        var clearColor = sprite.color;
        clearColor.a = 0;
        sprite.color = clearColor;
    }
    void Update()
    {
        float volume = targetAnalyzer.m_volumeLevel;
        int noteIndex = targetAnalyzer.m_noteIndex;
        float scale = Mathf.Lerp(volumeScale.x, volumeScale.y, volume);
        Color color = pitchColor.Evaluate((noteIndex+0f)/32);

        transform.localScale = Vector3.one * scale;
        sprite.color = Color.Lerp(sprite.color, color, Time.deltaTime * 10);
    }
    void OnPlayerStartSinging(AudioAnalyzer analyzer)
    {
        sprite.DOKill();
        sprite.DOFade(0.5f, 0.2f);
        targetAnalyzer = analyzer;
    }
    void OnPlayerStopSinging()
    {
        sprite.DOKill();
        sprite.DOFade(0, 0.2f);
        targetAnalyzer = null;
    }
}
