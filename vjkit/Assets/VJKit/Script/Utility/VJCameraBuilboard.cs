using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Always look at camera")]
[ExecuteInEditMode]
public class VJCameraBuilboard : MonoBehaviour {

	public Camera cam;

	void Start() {
		if(!cam) {
			cam = Camera.main;
		}
	}
	
	void Update() {
		gameObject.transform.LookAt(cam.transform);
	}
}
