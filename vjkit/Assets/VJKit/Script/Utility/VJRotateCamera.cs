using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Cameras/Rotate Camera")]
public class VJRotateCamera : VJFbm {

	public Transform rotateAroundTarget;
	public Transform lookAtTarget;
	
	[Range(0.0f, 720.0f)]
	public float rotateAngleSec = 0.1f;

//	public bool fbmMotion = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		if( rotateAroundTarget != null ) {			
			transform.RotateAround (rotateAroundTarget.position, Vector3.up, rotateAngleSec * Time.deltaTime);			
		}

		if( lookAtTarget != null ) {
			transform.LookAt(lookAtTarget.position);
		}

//		if(fbmMotion) {
//			ApplyFbmMotion();
//		}
	}
}
