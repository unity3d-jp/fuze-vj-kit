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

public enum ThresholdType {
	Value_LessThan,
	Value_MoreThan,
	Difference_LessThan,
	Difference_MoreThan,
	Difference_MoreThanAbs,
}

public abstract class VJBaseTrigger : VJBaseModifier {

	public ThresholdType 	thType = ThresholdType.Difference_MoreThan;
	public float threshold = 0.25f;
	
	public int semaphore = 0;
	private int _lastSemaphore = 0;
	public float trigerDelaySec  = 0.0f;
	
	private float trigerlastVal  = 0.0f;
	
	private bool gizmoTriger = false;
	Color gizmoColor = Color.white;
	
	// will be implemented by inherited classes
	public abstract void OnVJTrigger(GameObject go, float value);

	public override void Start () {
		_lastSemaphore = semaphore;
		base.Start();
	}
	
	private bool _Semaphore() {
		if(_lastSemaphore == 0) {
			_lastSemaphore = semaphore;
			return true;
		}
		_lastSemaphore -= 1;
		return false;
	}

	private IEnumerator _DelayedTrigger(GameObject go, float value) {
		yield return new WaitForSeconds(trigerDelaySec);
		OnVJTrigger(go, value);
	}	


	// do noting
	public override void VJPerformAction(GameObject go, float value) {
		if(_IsTriggered(value)) {
			if( _Semaphore() ) {
				if( trigerDelaySec > 0.0f ) {
					StartCoroutine(_DelayedTrigger(go, value));
				} else {
					OnVJTrigger(go, value);
				}
			}
		}
	}

	private bool _IsTriggered (float value) {
		float val = value;
		bool trigger = false;
		
		switch( thType ) {
		case ThresholdType.Value_LessThan:
			if( val < threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Value_MoreThan:
			if( val > threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Difference_LessThan:
			if( (val - trigerlastVal) < threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Difference_MoreThan:
			if( (val - trigerlastVal) > threshold ) {
				trigger = true;
			}
		break;
		case ThresholdType.Difference_MoreThanAbs: 
			if( Mathf.Abs(val - trigerlastVal) > threshold ) {
				trigger = true;
			}
		break;
		}
		
		trigerlastVal = val;
		
		gizmoTriger = trigger;
		
		return trigger;
	}
	

	void OnDrawGizmosSelected () 
	{
		if( gizmoTriger )
			gizmoColor = Color.red;
		else
			gizmoColor = Color.Lerp(gizmoColor, Color.white, 0.4f);
		
		Gizmos.color = gizmoColor;
		
		Gizmos.DrawWireSphere(transform.position, 0.5f);
	}
}
