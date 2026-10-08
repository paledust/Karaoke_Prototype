using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine.Unity;

[System.Serializable]
public class AnimationState{
    public string stateName;
    public AnimationReferenceAsset animation;
	public float speed = 1;
	public bool rndStartOffset = false;
}
[System.Serializable]
public class AnimationTransition {
	public AnimationReferenceAsset from;
	public AnimationReferenceAsset to;
	public AnimationReferenceAsset transition;
	public float speed = 1;
}
[CreateAssetMenu(menuName = "Spines/SpineAnimationController")]
public class SpineAnimationController : ScriptableObject
{
    [SerializeField] private List<AnimationState> states;
	[SerializeField] private List<AnimationState> oneShots;
    [SerializeField] private List<AnimationTransition> transitions;
    // public List<StateNameToAnimationReference> m_states{get{return states;}}
    // public List<AnimationTransition> m_transitions{get{return transitions;}}
	public AnimationTransition TryGetTransition (Spine.Animation from, Spine.Animation to) {
		foreach (AnimationTransition transition in transitions) {
			if (transition.from.Animation == from && transition.to.Animation == to) {
				return transition;
			}
		}
		return null;
	}
	public AnimationState TryGetAnimation(string animationName){
		AnimationState foundState = oneShots.Find(x => x.animation.name == animationName);
		return foundState;
	}
	public AnimationState TryGetOneShotAnimation (string stateName) {
		return TryGetOneShotAnimation(Animator.StringToHash(stateName));
	}
	public AnimationState TryGetOneShotAnimation (int nameHash) {
		AnimationState foundState = oneShots.Find(x => Animator.StringToHash(x.stateName) == nameHash);
		return foundState;
	}
	public AnimationState TryGetAnimationForState (string stateName) {
		return TryGetAnimationForState(Animator.StringToHash(stateName));
	}
	public AnimationState TryGetAnimationForState (int nameHash) {
		AnimationState foundState = states.Find(x => Animator.StringToHash(x.stateName) == nameHash);
		return foundState;
	}
}