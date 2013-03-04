using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Material Lerp")]
[RequireComponent(typeof(Renderer))]
public class VJMaterialLerpModifier : VJBaseModifier {

	[HideInInspector]
	public int matIndex;
	private Material _materialToLerp = null;
	
	public Material from;
	public Material to;

	public override void VJPerformAction(GameObject go, float value) {		
		if( !_materialToLerp ) {
			_materialToLerp = renderer.materials[matIndex];
		} 
		if( _materialToLerp ) {
			_materialToLerp.Lerp(from, to, Mathf.Clamp01(value) );			
		}
	}
}
