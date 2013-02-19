using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Light Property")]
[RequireComponent (typeof (Light))]
public class VJLightTrigger : VJBaseTrigger {

	public LightPropertyType propertyToModify;

	public override void OnVJTrigger(GameObject go, float value) {		
		Light l = go.GetComponent<Light>();
		if(null != l) {
			VJLightPropertyHelper.UpdateLight(l, propertyToModify, value );
		}
	}
}
