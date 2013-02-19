using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Animator")]
public class VJAnimatorModifier : VJBaseModifier {

	public override void VJPerformAction(GameObject go, float value) {
		Animator animator = go.GetComponent<Animator>();
		if( animator != null ) {
			animator.speed = value;
		}
	}
}
