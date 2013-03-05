using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Material Property(renderer materials)")]
[RequireComponent(typeof(Renderer))]
public class VJMaterialPropertyModifier2 : VJBaseModifier {
	
	[HideInInspector]
	public int matIndex;
	private Material _materialToModify = null;

	public MaterialPropertyType propertyToModify;
	public string propertyName;

	public override void VJPerformAction(GameObject go, float value) {

		if( !_materialToModify ) {
			_materialToModify = renderer.materials[matIndex];
		} 

		if( _materialToModify ) {
			VJMaterialPropertyHelper.UpdateMaterial(_materialToModify, propertyToModify, value, propertyName );			
		}	
	}
}
