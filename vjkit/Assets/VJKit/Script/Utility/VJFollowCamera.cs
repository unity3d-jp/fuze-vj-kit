using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Cameras/Follow Camera")]
public class VJFollowCamera : VJFbm {

	public Transform followTarget;
	public Transform lookAtTarget;
	
	[Range(0.0f, 2.0f)]
	public float sensitiveness = 0.1f;
	public float distance = 5.0f;
	public float yOffset = 1.0f;
	//public bool fbmMotion = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if( followTarget != null ) {
			
			Quaternion rot = followTarget.rotation;
			Vector3 oldPos = followTarget.position;
			followTarget.position = new Vector3(followTarget.position.x, 0, followTarget.position.z);
			Vector3 mypos = transform.position;
			mypos.y = 0.0f;
			followTarget.LookAt(mypos);
			Vector3 targetPos = followTarget.TransformPoint(new Vector3(0.0f, yOffset, distance));
			followTarget.rotation = rot;
			followTarget.position = oldPos;
			
			transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * sensitiveness);
		}

		if( lookAtTarget != null ) {
			Vector3 lookPos = new Vector3(lookAtTarget.position.x, lookAtTarget.position.y + yOffset, lookAtTarget.position.z);
			transform.LookAt(lookPos);
		}

//		if(fbmMotion) {
//			ApplyFbmMotion();
//		}
	}
}
