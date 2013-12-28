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
