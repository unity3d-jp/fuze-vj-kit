using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utility/Fbm Motion")]
public class VJFbmTransform : VJFbm {

	public bool fbmMotion = false;

	public void Update() {
		if(fbmMotion) {
			ApplyFbmMotion();
		}
	}
}
