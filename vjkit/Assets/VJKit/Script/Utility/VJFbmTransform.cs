using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Fbm Motion")]
public class VJFbmTransform : VJFbm {

	public bool fbmMotion = false;

	public void Update() {
		if(fbmMotion) {
			ApplyFbmMotion();
		}
	}
}
