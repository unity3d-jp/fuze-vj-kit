using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Freeze Position")]
public class VJPositionFreeze : MonoBehaviour {

	public bool freezePosition = true;
	public bool freezeRotation = false;
	public bool freezeScale = false;

	private Vector3 savedPosition;
	private Quaternion savedRotation;
	private Vector3 savedScale;

	// Use this for initialization
	void Start () {
		savedPosition = transform.position;
		savedRotation = transform.rotation;
		savedScale    = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(freezePosition)
			transform.position = savedPosition;
		if(freezeRotation)
			transform.rotation = savedRotation;
		if(freezeScale)
			transform.localScale = savedScale;
	}
}
