using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Material Lerp")]
[RequireComponent(typeof(Renderer))]
public class VJMaterialLerpTrigger : VJBaseTrigger {

	[HideInInspector]
	public int matIndex;
	private Material _materialToLerp = null;
	
	public Material from;
	public Material to;

	public override void OnVJTrigger (GameObject go, float value) {
		if( !_materialToLerp ) {
			_materialToLerp = renderer.materials[matIndex];
		} 
		if( _materialToLerp ) {
			_materialToLerp.Lerp(from, to, Mathf.Clamp01(value) );			
		}
	}
}
