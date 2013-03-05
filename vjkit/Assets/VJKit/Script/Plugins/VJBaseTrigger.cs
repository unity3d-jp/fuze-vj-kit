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
	
	public int semaphore = 0;
	private int _lastSemaphore = 0;
	
	private float trigerlastVal  = 0.0f;
	
	private bool gizmoTriger = false;
	Color gizmoColor = Color.white;
	
	// will be implemented by inherited classes
	public abstract void OnVJTrigger(GameObject go, float value);

	public override void Start () {
		_lastSemaphore = semaphore;
		base.Start();
	}
	
	private bool _Semaphore() {
		if(_lastSemaphore == 0) {
			_lastSemaphore = semaphore;
			return true;
		}
		_lastSemaphore -= 1;
		return false;
	}


	// do noting
	public override void VJPerformAction(GameObject go, float value) {
		if(_IsTriggered(value)) {
			if( _Semaphore() ) {
				OnVJTrigger(go, value);
			}
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
		
		gizmoTriger = trigger;
		
		return trigger;
	}
	

	void OnDrawGizmosSelected () 
	{
		if( gizmoTriger )
			gizmoColor = Color.red;
		else
			gizmoColor = Color.Lerp(gizmoColor, Color.white, 0.4f);
		
		Gizmos.color = gizmoColor;
		
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
