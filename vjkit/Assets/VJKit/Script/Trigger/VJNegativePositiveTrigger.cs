using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Negative-Positive Switch")]
public class VJNegativePositiveTrigger : VJBaseTrigger {

	public VJBaseModifier modifierToSwitch;	
	private bool _state;

	public override void Awake () {
		base.Awake();
		
		if( modifierToSwitch != null ) {
			_state = modifierToSwitch.negative;
		}
	}
	
	private void _ToggleEnable() {
		_state = !_state;		
		modifierToSwitch.negative = _state;
	}

	public override void OnVJTrigger(GameObject go, float value) {		
		_ToggleEnable();
	}
}
