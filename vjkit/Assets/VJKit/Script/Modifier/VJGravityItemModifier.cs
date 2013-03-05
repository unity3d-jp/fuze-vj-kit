using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Gravity Item")]
[RequireComponent(typeof(VJGravityItem))]
public class VJGravityItemModifier : VJBaseModifier {

	public override void VJPerformAction(GameObject go, float value) {	
		VJGravityItem gi = go.GetComponent<VJGravityItem>();
		if( gi != null ) {
			gi.strength = value;
		}
	}
}
