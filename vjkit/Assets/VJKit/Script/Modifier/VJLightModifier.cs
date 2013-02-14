using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Light Property")]
[RequireComponent (typeof (Light))]
public class VJLightModifier : VJBaseModifier {

	public LightPropertyType propertyToModify;

	void Update () {
		VJLightPropertyHelper.UpdateLight(light, propertyToModify, GetValue() );
	}
}
