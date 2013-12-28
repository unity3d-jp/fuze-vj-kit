/* 
 * fuZe vjkit
 * 
 * Copyright (C) 2013 Unity Technologies Japan, G.K.
 * 
 * Permission is hereby granted, free of charge, to any 
 * person obtaining a copy of this software and associated 
 * documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit 
 * persons to whom the Software is furnished to do so, subject 
 * to the following conditions: The above copyright notice and 
 * this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Special Thanks:
 * The original software design and architecture of fuZe vjkit 
 * is inspired by Visualizer Studio, created by Altered Reality 
 * Entertainment LLC.
 */
using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/GravitySphere")]
public class VJGravitySphere : VJGravityItem {

	public void FixedUpdate() {
	
		Rigidbody[] rbs = GameObject.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		Vector3 gpos = transform.position;

		for(int i=0; i < rbs.Length; ++i) {
			
			Vector3 rPos = rbs[i].transform.position;
			
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
		Gizmos.DrawWireSphere (transform.position, 1.0f + strength * 0.1f);
	}
}
