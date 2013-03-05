using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/On-Off Switch")]
public class VJOnOffTrigger : VJBaseTrigger {

	public Behaviour componentToOnOff;	
	private bool _componentEnabled;
	private VJBaseModifier _modifierToOnOff;

	public override void Awake () {
		base.Awake();
		
		if( componentToOnOff != null ) {
			if( componentToOnOff is VJBaseModifier ) {
				_modifierToOnOff = componentToOnOff as VJBaseModifier;
				_componentEnabled = _modifierToOnOff.isModifierEnabled;
			} else {
				_componentEnabled = componentToOnOff.enabled;
			}
		}
	}
	
	private void _ToggleEnable() {
		_componentEnabled = !_componentEnabled;
		
		if(_modifierToOnOff != null) {
			_modifierToOnOff.isModifierEnabled = _componentEnabled;
		} else {
			componentToOnOff.enabled = _componentEnabled;
		}
	}

	public override void OnVJTrigger(GameObject go, float value) {		
		_ToggleEnable();
	}
}
