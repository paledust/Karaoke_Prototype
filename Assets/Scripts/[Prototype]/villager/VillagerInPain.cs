using System;
using UnityEngine;

namespace WhisperPrototype.Villager
{
    public class VillagerInPain : MonoBehaviour
    {
        [SerializeField] private SpineAnimator spineAnimator;
        [SerializeField] private ParticleSystem vfx_happy;
        public void OnComforted()
        {
            spineAnimator.PlayAnimationForState("praise", 0, 1);
            vfx_happy.Play();
        }
    }
}
