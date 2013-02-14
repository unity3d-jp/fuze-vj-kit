using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Material Property")]
[RequireComponent (typeof (Light))]
public class VJMaterialPropertyModifier : VJBaseModifier {
	
	public Material targetMaterial;
	public MaterialPropertyType propertyToModify;
	public string propertyName;

	void Update () {
		if( targetMaterial ) {
			VJMaterialPropertyHelper.UpdateMaterial(targetMaterial, propertyToModify, GetValue(), propertyName );			
		}	
	}
}
