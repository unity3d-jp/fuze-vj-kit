using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Animator")]
public class VJAnimatorTrigger : VJBaseTrigger {

	public override void OnVJTrigger(GameObject go, float value) {		
		Animator animator = go.GetComponent<Animator>();
		if( animator != null ) {
			animator.speed = value;
		}
	}
}
