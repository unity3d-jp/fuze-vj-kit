using UnityEngine;
using System.Collections;

public enum ThresholdType {
	Value_LessThan,
	Value_MoreThan,
	Difference_LessThan,
	Difference_MoreThan
}

public abstract class VJBaseTrigger : MonoBehaviour {

	[HideInInspector]
	public VJManager manager;
	[HideInInspector]
	public VJDataSource source;
	public ValueSourceType 	sourceType = ValueSourceType.Current;
	public bool clampInput;
	[Range(-100.0f, 0.0f)]
	public float clampMin = 0.0f;
	[Range(0.0f, 100.0f)]
	public float clampMax = 1.0f;
	
	public ThresholdType 	thType = ThresholdType.Difference_MoreThan;
	public float threshold = 0.25f;
	private float lastVal  = 0.0f;

	public float valueMin = 0.0f;
	public float valueMax = 1.0f;

	// Use this for initialization
	void Start () {
		if(!manager) {
			manager = VJManager.GetDefaultManager();
			if(!manager) {
				Debug.LogError("[FATAL] VJ Manager not found.");
			}
		}
	
		if(!source) {
			source = manager.GetDefaultDataSource();
			if(!source) {
				Debug.LogError("[FATAL] Data source not found.");
			}
		}
	}
	
	void Update () {
		if(_IsTriggered()) {
			OnVJTrigger();
		}
	}
	
	// will be implemented by inherited classes
	public abstract void OnVJTrigger();

	protected float _GetRawValue() {
		switch(sourceType) {
			case ValueSourceType.Current:
				return source.current;
			case ValueSourceType.Previous:
				return source.previous;
			case ValueSourceType.Difference:
				return source.diff;
		}
		return source.diff;
	}
	
	// Update is called once per frame
	public float GetValue () {
		float inValue;
		if(clampInput) {
			inValue = Mathf.Clamp(_GetRawValue(), clampMin, clampMax);
			float t = (inValue - clampMin) / clampMax;
			return Mathf.Lerp(valueMin, valueMax, t);
		} else {
			return Mathf.Clamp(_GetRawValue(), valueMin, valueMax);
		}
	}
	
		// Update is called once per frame
	private bool _IsTriggered () {
		float val = GetValue();
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
			if( (val - lastVal) < threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Difference_MoreThan:
			if( (val - lastVal) > threshold ) {
				trigger = true;
			}
		break;
		}
		
		lastVal = val;
		return trigger;
	}

}
