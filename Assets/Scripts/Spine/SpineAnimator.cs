using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine.Unity;
using Spine;

public class SpineAnimator : MonoBehaviour
{
	[SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private SpineAnimationController controller;
	public SkeletonAnimation m_skeletonAnimation{get{return skeletonAnimation;}}
	public MeshRenderer m_meshRenderer{get{return meshRenderer;}}

	public TrackEntry AddAnimationForState(string animationName, int layerIndex, float speedMultiplier = 1){
		AnimationState stateAnimation = GetAnimationForState(animationName);
		if (stateAnimation == null){
			Debug.LogError($"No Clip {animationName} for State");
			return null;
		}

		var entry = skeletonAnimation.AnimationState.AddAnimation(layerIndex, stateAnimation.animation, true, 0f);
		entry.TimeScale = stateAnimation.speed*speedMultiplier;

		return entry;
	}
	public TrackEntry AddOneShotAnimation(string animationName, int layerIndex, float speedMultiplier = 1, float delay = 0){
		AnimationState oneshotAnimation = GetOneShot(animationName);
		if (oneshotAnimation == null){
			Debug.LogError($"No Clip {animationName} for State");
			return null;
		}

		var entry = skeletonAnimation.AnimationState.AddAnimation(layerIndex, oneshotAnimation.animation, false, delay);
		entry.TimeScale = oneshotAnimation.speed*speedMultiplier;

		return entry;
	}
	public TrackEntry PlayOneShotAnimation(string animationName, int layerIndex, float speedMultiplier = 1){
		AnimationState foundAnimation = GetOneShot(animationName);
		if (foundAnimation == null){
			Debug.LogError($"No Clip {animationName} for One Shot");
			return null;
		}

		TrackEntry entry;
		entry = skeletonAnimation.AnimationState.SetAnimation(layerIndex, foundAnimation.animation, false);
		entry.TimeScale = foundAnimation.speed*speedMultiplier;

		return entry;
	}
	public TrackEntry[] PlayOneShotAnimation(string animationName, int layerIndex, string nextStateAnimationName, float speedMultiplier = 1){
		AnimationState oneshotAnimation = GetOneShot(animationName);
		if (oneshotAnimation == null) {
			Debug.LogError($"No Clip {animationName} for One Shot");
			return null;
		}
		AnimationState stateAnimation = GetAnimationForState(nextStateAnimationName);
		if (stateAnimation == null) {
			Debug.LogError($"No Clip {nextStateAnimationName} for State");
			return null;
		}

		TrackEntry[] entry = new TrackEntry[2];
		entry[0] = skeletonAnimation.AnimationState.SetAnimation(layerIndex, oneshotAnimation.animation, false);
		entry[0].TimeScale = oneshotAnimation.speed*speedMultiplier;
		entry[1] = skeletonAnimation.AnimationState.AddAnimation(layerIndex, stateAnimation.animation, true, 0f);
		entry[1].TimeScale = stateAnimation.speed*speedMultiplier;

		return entry;
	}
	public TrackEntry PlayAnimationForState (string stateName, int layerIndex, float speedMultiplier = 1) {
		AnimationState foundAnimation = GetAnimationForState(stateName);
		if (foundAnimation == null) {
			Debug.LogError($"No Clip {stateName} for State");
			return null;
		}
		var entry = PlayAnimation(foundAnimation, true, layerIndex, speedMultiplier);
		if(foundAnimation.rndStartOffset) entry.TrackTime = Random.Range(0,1f);
		return entry;
	}
	public bool IsEmpty(int layer = 0){
		return GetCurrentAnimation(layer) == null;
	}
	public bool IsState(string stateName, int layer = 0){
		return GetCurrentAnimation(layer) == GetAnimationForState(stateName).animation.Animation;
	}
	public bool IsPlaying(string oneshotName, int layer = 0){
		return GetCurrentAnimation(layer) == GetOneShot(oneshotName).animation.Animation;
	}
	public void SetFlip (float horizontal) {
		if (horizontal != 0) {
			horizontal = (horizontal > 0) ? 1f : -1f;
			if(skeletonAnimation.skeleton.ScaleX != horizontal){
				skeletonAnimation.Skeleton.ScaleX = horizontal;
			}
		}
	}
    TrackEntry PlayAnimation(AnimationState target, bool isLoop = true, int layerIndex = 0, float speedMultiplier = 1){
		AnimationTransition transition = null;
		Spine.Animation current = null;

		current = GetCurrentAnimation(layerIndex);
		if(current == target.animation.Animation) return null;
		if (current != null)
			transition = controller.TryGetTransition(current, target.animation);

		TrackEntry entry;
		if (transition != null) {
			entry = skeletonAnimation.AnimationState.SetAnimation(layerIndex, transition.transition, false);
			entry.TimeScale = transition.speed*speedMultiplier;

			entry = skeletonAnimation.AnimationState.AddAnimation(layerIndex, target.animation, isLoop, 0f);
			entry.TimeScale = target.speed*speedMultiplier;
		} else {
			entry = skeletonAnimation.AnimationState.SetAnimation(layerIndex, target.animation, isLoop);
			entry.TimeScale = target.speed*speedMultiplier;
		}

		return entry;
    }
	Spine.Animation GetCurrentAnimation (int layerIndex) {
		TrackEntry currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
		return (currentTrackEntry != null) ? currentTrackEntry.Animation : null;
	}
	AnimationState GetAnimationForState(string stateShortName){
		return controller.TryGetAnimationForState(Animator.StringToHash(stateShortName));
	}
	AnimationState GetOneShot(string stateShortName) {
		return controller.TryGetOneShotAnimation(Animator.StringToHash(stateShortName));
	}
}