using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Modifiers/Gravity")]
public class VJGravityModifier : VJBaseModifier {

	public enum VJGravityDirection {
		XAxis,
		YAxis,
		ZAxis
	}

	public VJGravityDirection gravityDirection;

	public bool normalizeWhenDispabled;
	public Vector3 normalValue 	 = new Vector3(0.0f, -1.0f, 0.0f);

	public override void VJPerformAction(GameObject go, float value) {	

		if(isModifierEnabled) {
			_SetGravity(value);
		} else {
			if(normalizeWhenDispabled) {
				Physics.gravity = normalValue;
			}
		}
	}
	
	private void _SetGravity(float val) {
		Vector3 g = Physics.gravity;
		
		switch(gravityDirection) {
			case VJGravityDirection.XAxis: 
				g.x = val;
				break;
			case VJGravityDirection.YAxis: 
				g.y = val;
				break;
			case VJGravityDirection.ZAxis: 
				g.z = val;
				break;
		}
		
		Physics.gravity = g;	
	}
	
	public void OnDisable() {
		if(normalizeWhenDispabled) {
			Physics.gravity = normalValue;
		}
	}
}
