using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Animator Transition")]
public class VJAnimatorTransitionTrigger : VJBaseTrigger {

	public string parameterName = null;

	public override void OnVJTrigger(GameObject go, float value) {		
		Animator animator = go.GetComponent<Animator>();
		if( animator != null ) {
			animator.SetFloat(parameterName, value);
		} else {
			animator.SetFloat(parameterName, 0.0f);
		}
	}
}
