using DG.Tweening;
using UnityEngine;

namespace WhisperPrototype
{
    public class SpriteFader
    {
        private SpriteRenderer sprite;

        public SpriteFader(SpriteRenderer targetSprite)
        {
            sprite = targetSprite;
            var clearColor = sprite.color;
            clearColor.a = 0;
            sprite.color = clearColor;
        }
        public void OnFadeInSprite()
        {
            sprite.DOKill();
            sprite.DOFade(1f, 0.2f);
        }
        public void OnFadeOutSprite()
        {
            sprite.DOKill();
            sprite.DOFade(0, 0.2f);
        }
    }
}
