using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;

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

	[Range(0.0f, 100.0f)]
	public float boost = 1.0f;				///< 値の増幅値

	public bool limitMinMax = false;		///< min/maxに値を絞るかどうか
	public float valueMin = 0.0f;
	public float valueMax = 1.0f;
	
	public float middleOffset = 0.0f;		///< 中央値のオフセット

	public bool rest = false;				///< 値を中央値に徐々に戻すかどうか
	[Range(0.0f, 1.0f)]
	public float restStrength = 0.98f;

	private float lastValue = 0.0f;			///< 前回の値

	[HideInInspector]
	public float lastReturnedValue = 0.0f;	///< 前回の値（実際の使用値）

	public bool negative = false;			///< 値を反転するかどうか

	public bool multiple = false;			///< 複数のオブジェクトに対する操作をするかどうか
	public VJModifierTarget[] targets;

	[HideInInspector]
	public bool isModifierEnabled = true;

	public virtual void Awake () {
	}

	// Use this for initialization
	public virtual void Start () {
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
	
	// Update is called once per frame
	private float _GetValue (ref float _lastValue) {

		// バリエーションを加えて、初期値を作成
		float v = (isModifierEnabled) ? ( _GetRawValue() ) * boost : 0.0f;

		if( negative ) {
			v = -v;
		}
		
		// 値をmin/maxの範囲内に固定する
		if( limitMinMax ) {
			v = Mathf.Clamp(v, valueMin, valueMax);
		}

		// rest onなら、restの強度で値を下げる
		// 今回の値が大きいなら今回の値を使う
		if( rest ) {
			if(negative) {
				v = Mathf.Min(_lastValue * restStrength, v);
			} else {
				v = Mathf.Max(_lastValue * restStrength, v);
			}
		}

		// 前回の値を保存
		_lastValue = v;

		// 中央値オフセットの適用		
		lastReturnedValue = v + middleOffset;
		return lastReturnedValue;
	}
	
	public abstract void VJPerformAction(GameObject go, float value);
	
	void Update() {
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
