using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Light Property")]
public class VJLightModifier : VJBaseModifier {

	public LightPropertyType propertyToModify;

	public override void VJPerformAction(GameObject go, float value) {

		Light l = go.GetComponent<Light>();
		if(null != l) {
			VJLightPropertyHelper.UpdateLight(l, propertyToModify, value );
		}
	}
}
