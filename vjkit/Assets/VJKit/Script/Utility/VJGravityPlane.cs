using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/GravityPlane")]
public class VJGravityPlane : VJGravityItem {

	public void FixedUpdate() {
	
		Rigidbody[] rbs = GameObject.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		Vector3 gpos = transform.position;

		for(int i=0; i < rbs.Length; ++i) {
			
			Vector3 rPos = rbs[i].transform.position;
			
			gpos.x = rPos.x;
			gpos.z = rPos.z;
			
			float d = Vector3.Distance(gpos, rPos);
			
			Vector3 vecForce = gpos - rPos;
			vecForce.Normalize();
			
			switch(gravityType) {
				case GravityType.Force:
					rbs[i].AddForce( vecForce * (strength * Mathf.Max(0.0f, 3.0f*d) ));
				break;
				case GravityType.Vecocity:
					rbs[i].velocity = (rbs[i].velocity + (0.1f * vecForce * (strength * Mathf.Max(0.0f, 3.0f*d)))) /2.0f;
				break;
			}
		}
	}

	public void OnDrawGizmos () {
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube (transform.position, new Vector3 (10.0f,0.01f,10.0f));
	}
}
