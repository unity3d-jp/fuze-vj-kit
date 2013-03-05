using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Material Property")]
public class VJMaterialPropertyTrigger : VJBaseTrigger {

	public Material targetMaterial;
	public MaterialPropertyType propertyToModify;
	public string propertyName;
	
	private float _initialValue;

	public override void Start () {
		if( targetMaterial ) {
			_initialValue = VJMaterialPropertyHelper.GetMaterialValue(targetMaterial, propertyToModify, propertyName );
		}	
		base.Start();
	}

	public override void OnVJTrigger (GameObject go, float value) {
		if( targetMaterial ) {
			VJMaterialPropertyHelper.UpdateMaterial(targetMaterial, propertyToModify, value, propertyName );			
		}	
	}

	public void OnApplicationQuit () {
		if( targetMaterial ) {
			VJMaterialPropertyHelper.UpdateMaterial(targetMaterial, propertyToModify, _initialValue, propertyName );			
		}	
	}
}
