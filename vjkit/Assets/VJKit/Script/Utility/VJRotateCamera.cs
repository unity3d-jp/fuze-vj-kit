using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Cameras/Rotate Camera")]
public class VJRotateCamera : MonoBehaviour {

	public Transform rotateAroundTarget;
	public Transform lookAtTarget;
	
	[Range(0.0f, 720.0f)]
	public float rotateAngleSec = 0.1f;
	
	public bool smooth;
	public float damping = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {		
		if( rotateAroundTarget != null ) {			
			transform.RotateAround (rotateAroundTarget.position, Vector3.up, rotateAngleSec * Time.deltaTime);			
		}

		if( lookAtTarget != null ) {
			if (smooth)
			{
				// Look at and dampen the rotation
				var rotation = Quaternion.LookRotation(lookAtTarget.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
			}
			else
			{
				// Just lookat
				transform.LookAt(lookAtTarget);
			}
		}
	}
}
