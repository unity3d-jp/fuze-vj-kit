using UnityEngine;
using System.Collections;

public enum ThresholdType {
	Value_LessThan,
	Value_MoreThan,
	Difference_LessThan,
	Difference_MoreThan
}

public abstract class VJBaseTrigger : VJBaseModifier {

	public ThresholdType 	thType = ThresholdType.Difference_MoreThan;
	public float threshold = 0.25f;
	private float trigerlastVal  = 0.0f;
	
	// will be implemented by inherited classes
	public abstract void OnVJTrigger(GameObject go, float value);

	// do noting
	public override void VJPerformAction(GameObject go, float value) {
		if(_IsTriggered(value)) {
			OnVJTrigger(go, value);
		}
	}

	private bool _IsTriggered (float value) {
		float val = value;
		bool trigger = false;
		
		switch( thType ) {
		case ThresholdType.Value_LessThan:
			if( val < threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Value_MoreThan:
			if( val > threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Difference_LessThan:
			if( (val - trigerlastVal) < threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Difference_MoreThan:
			if( (val - trigerlastVal) > threshold ) {
				trigger = true;
			}
		break;
		}
		
		trigerlastVal = val;
		return trigger;
	}
}
