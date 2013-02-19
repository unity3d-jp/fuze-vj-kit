using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Material Property")]
public class VJMaterialPropertyModifier : VJBaseModifier {
	
	public Material targetMaterial;
	public MaterialPropertyType propertyToModify;
	public string propertyName;

	public override void VJPerformAction(GameObject go, float value) {
		if( targetMaterial ) {
			VJMaterialPropertyHelper.UpdateMaterial(targetMaterial, propertyToModify, value, propertyName );			
		}	
	}
}
