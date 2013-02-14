using UnityEngine;
using System.Collections;

public enum ValueSourceType {
	Current,
	Previous,
	Difference
}

public abstract class VJBaseModifier : MonoBehaviour {

	[HideInInspector]
	public VJManager manager;
	[HideInInspector]
	public VJDataSource source;
	public ValueSourceType sourceType = ValueSourceType.Current;
	public bool clampInput;
	[Range(-100.0f, 0.0f)]
	public float clampMin = 0.0f;
	[Range(0.0f, 100.0f)]
	public float clampMax = 1.0f;

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
}
