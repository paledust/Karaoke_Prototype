using System;
using DG.Tweening;
using UnityEngine;

public class VoiceOrb : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
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
    void OnPlayerStartSinging()
    {
        sprite.DOKill();
        sprite.DOFade(0.5f, 0.2f);
    }
    void OnPlayerStopSinging()
    {
        sprite.DOKill();
        sprite.DOFade(0, 0.2f);
    }
}
