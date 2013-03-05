using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Scene Speed")]
public class VJSceneSpeedModifier : VJBaseModifier {

	public bool normalizeWhenDispabled;
	public float normalValue = 1.0f;

	public override void VJPerformAction(GameObject go, float value) {	

		if(isModifierEnabled) {
			Time.timeScale = Mathf.Max(0.0f, value);
		} else {
			if(normalizeWhenDispabled) {
				Time.timeScale = normalValue;
			}
		}
	}
	
	public void OnDisable() {
		if(normalizeWhenDispabled) {
			Time.timeScale = normalValue;
		}
	}
}
