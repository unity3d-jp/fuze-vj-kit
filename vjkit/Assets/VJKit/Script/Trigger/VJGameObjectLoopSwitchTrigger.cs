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

[AddComponentMenu("VJKit/Triggers/On-Off Looop(GameObject)")]
public class VJGameObjectLoopSwitchTrigger : VJBaseTrigger {

	public GameObject[] gameObjectToLoop;
	public bool forSlider;
	public int index;

//	public bool inverse;

	public override void Awake () {
		base.Awake();		
	}
	
	private void _ToggleEnable(float value) {
		if(forSlider) {
			int lastIndex = index;
			index = Mathf.FloorToInt(value);
			gameObjectToLoop[lastIndex].SetActive(false);
			gameObjectToLoop[index].SetActive(true);
		} else {
			int lastIndex = index;
//			index = (inverse? Mathf.Abs (index-1) : index+1) % gameObjectToLoop.Length;
			index = (index+1) % gameObjectToLoop.Length;
			gameObjectToLoop[lastIndex].SetActive(false);
			gameObjectToLoop[index].SetActive(true);
		}
	}

	public override void OnVJTrigger(GameObject go, float value) {		
		_ToggleEnable(value);
	}
}
