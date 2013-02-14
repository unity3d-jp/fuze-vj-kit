using UnityEngine;
using System.Collections;

/*
	カメラにアタッチして下さい
*/
[AddComponentMenu("VJKit/Utilities/Wireframe Camera")]
public class VJWireframeCamera : MonoBehaviour {

	public bool wireframeMode;

	void OnPreRender() {
		if(wireframeMode) {
			GL.wireframe = true;
		}
	}
	void OnPostRender() {
		if(wireframeMode) {
			GL.wireframe = false;
		}
	}
}
