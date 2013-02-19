using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Material Property")]
public class VJMaterialPropertyTrigger : VJBaseTrigger {

	public Material targetMaterial;
	public MaterialPropertyType propertyToModify;
	public string propertyName;

	public override void OnVJTrigger (GameObject go, float value) {
		if( targetMaterial ) {
			VJMaterialPropertyHelper.UpdateMaterial(targetMaterial, propertyToModify, value, propertyName );			
		}	
	}
}
