using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Light Property")]
[RequireComponent (typeof (Light))]
public class VJLightTrigger : VJBaseTrigger {

	public LightPropertyType propertyToModify;

	public override void OnVJTrigger() {		
		VJLightPropertyHelper.UpdateLight(light, propertyToModify, GetValue() );
	}
}
