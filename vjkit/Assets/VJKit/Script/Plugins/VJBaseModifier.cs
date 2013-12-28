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
using System.Collections.Generic;

public enum ValueSourceType {
	Current,
	Previous,
	Difference
}

[System.Serializable]
public class VJModifierTarget {
	public GameObject gameObject;
	[Range(0, 100)]
	public int octerb;
	[HideInInspector]
	public float lastValue = 0.0f;
}

public abstract class VJBaseModifier : MonoBehaviour {

	[HideInInspector]
	public VJAbstractManager manager;
	[HideInInspector]
	public VJAbstractDataSource source;
	public ValueSourceType sourceType = ValueSourceType.Current;

	[SerializeField]
	[Range(0.0f, 100.0f)]
	protected float m_boost = 1.0f;

	public bool boostByOtherSource = false;

	[HideInInspector]
	public VJAbstractManager boostManager;
	[HideInInspector]
	public VJAbstractDataSource boostSource;

	public float boost {
		get {
			return _GetBoostValue();
		}
		set {
			m_boost = value;
		}
	}

	public bool limitValueMinMax = false;		
	public float valueMin = 0.0f;
	public float valueMax = 1.0f;
	
	public float valueBaseOffset = 0.0f;		

	public bool rest = false;				
	[Range(0.0f, 1.0f)]
	public float restStrength = 0.98f;

	private float lastValue = 0.0f;			

	[HideInInspector]
	public float lastReturnedValue = 0.0f;	

	public bool negative = false;			

	public bool multiple = false;			
	public VJModifierTarget[] targets;

	[HideInInspector]
	public bool isModifierEnabled = true;

	public virtual void Awake () {
	}

	// Use this for initialization
	public virtual void Start () {
		lastValue = lastReturnedValue = valueBaseOffset;

		if(!manager) {
			manager = VJAbstractManager.GetDefaultManager();
			if(!manager) {
				Debug.LogError("[FATAL] VJ Manager not found.");
			}
		}
	
		if(!source) {
			source = manager.GetDefaultDataSource();
			if(!source) {
				Debug.LogError("[FATAL] Data source not found.");
			}
		}
		if(!boostSource) {
			boostSource = manager.GetDefaultDataSource();
			if(!source) {
				Debug.LogError("[FATAL] Data source not found.");
			}
		}
	}

	protected float _GetRawValue() {
		switch(sourceType) {
			case ValueSourceType.Current:
				return source.current;
			case ValueSourceType.Previous:
				return source.previous;
			case ValueSourceType.Difference:
				return source.diff;
		}
		return source.diff;
	}

	protected float _GetBoostValue() {
		if( boostByOtherSource ) {
			return boostSource.current;
		} else {
			return m_boost;
		}
	}
	
	// Update is called once per frame
	private float _GetValue (ref float _lastValue) {

		float v = (isModifierEnabled) ? ( _GetRawValue() ) * _GetBoostValue() : 0.0f;

		if( negative ) {
			v = -v;
		}
		
		if( limitValueMinMax ) {
			v = Mathf.Clamp(v, valueMin, valueMax);
		}

		if( rest ) {
			if(negative) {
				v = Mathf.Min(_lastValue * restStrength, v);
			} else {
				v = Mathf.Max(_lastValue * restStrength, v);
			}
		}

		_lastValue = v;

		lastReturnedValue = v + valueBaseOffset;
		return lastReturnedValue;
	}
	
	public abstract void VJPerformAction(GameObject go, float value);
	
	void Update() {
		if( float.IsNaN(lastValue) || float.IsInfinity(lastValue) ) {
			return;
		}
		if(!multiple) {
			VJPerformAction(gameObject, _GetValue(ref lastValue));
		} else if(targets != null) {			
			foreach(VJModifierTarget t in targets) {
				VJPerformAction(t.gameObject, _GetValue(ref t.lastValue));
			}
		}
	}
	
	public void SetVisibleChildrenAsTarget() {

		targets = null;
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>() as MeshRenderer[];
        
        if( meshRenderers.Length > 0 ) {
			targets = new VJModifierTarget[meshRenderers.Length];
			for (int i = 0; i < meshRenderers.Length; ++i) {
				targets[i] = new VJModifierTarget();
				targets[i].gameObject = meshRenderers[i].gameObject;
				targets[i].octerb = Random.Range(0, 100);
			}
        }

		multiple = (targets != null && targets.Length > 0);
	}
}
