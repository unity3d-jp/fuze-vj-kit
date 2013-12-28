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

[AddComponentMenu("VJKit/Modifiers/Gravity")]
public class VJGravityModifier : VJBaseModifier {

	public enum VJGravityDirection {
		XAxis,
		YAxis,
		ZAxis
	}

	public VJGravityDirection gravityDirection;

	public bool normalizeWhenDispabled;
	public Vector3 normalValue 	 = new Vector3(0.0f, -1.0f, 0.0f);

	public override void VJPerformAction(GameObject go, float value) {	

		if(isModifierEnabled) {
			_SetGravity(value);
		} else {
			if(normalizeWhenDispabled) {
				Physics.gravity = normalValue;
			}
		}
	}
	
	private void _SetGravity(float val) {
		Vector3 g = Physics.gravity;
		
		switch(gravityDirection) {
			case VJGravityDirection.XAxis: 
				g.x = val;
				break;
			case VJGravityDirection.YAxis: 
				g.y = val;
				break;
			case VJGravityDirection.ZAxis: 
				g.z = val;
				break;
		}
		
		Physics.gravity = g;	
	}
	
	public void OnDisable() {
		if(normalizeWhenDispabled) {
			Physics.gravity = normalValue;
		}
	}
}
