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
	public VJManager manager;
	[HideInInspector]
	public VJDataSource source;
	public ValueSourceType sourceType = ValueSourceType.Current;

	[Range(0.0f, 100.0f)]
	public float boost = 1.0f;				///< 値の増幅値

	[Range(0.0f, 1.0f)]
	public float variationAmount = 0.0f;	///< ランダム揺らぎ量
	[Range(0.0f, 1.0f)]
	public float variationSpeed = 0.2f;		///< ランダム揺らぎ量
	[Range(0, 100)]
	public int variationOcterb;

	public bool limitMinMax = false;		///< min/maxに値を絞るかどうか
	public float valueMin = 0.0f;
	public float valueMax = 1.0f;
	
	public float middleOffset = 0.0f;		///< 中央値のオフセット

	public bool rest = false;				///< 値を中央値に徐々に戻すかどうか
	[Range(0.0f, 1.0f)]
	public float restStrength = 0.98f;

	private float lastValue = 0.0f;			///< 前回の値

	public bool negative = false;			///< 値を反転するかどうか

	public bool multiple = false;			///< 複数のオブジェクトに対する操作をするかどうか
	public VJModifierTarget[] targets;

	// Use this for initialization
	void Start () {
		if(!manager) {
			manager = VJManager.GetDefaultManager();
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
	
	private float _GetVariation(ref int vOcterb) {
		return (VJPerlin.Fbm(Time.time * variationSpeed, vOcterb)) * variationAmount;
	}
	
	// Update is called once per frame
	private float _GetValue (ref float _lastValue, ref int vOcterb) {

		// バリエーションを加えて、初期値を作成
		float v = ( _GetRawValue() + _GetVariation(ref vOcterb) ) * boost;

		// rest onなら、restの強度で値を下げる
		// 今回の値が大きいなら今回の値を使う
		if( rest ) {
			v = Mathf.Max(_lastValue * restStrength, v);
		}
		
		// 値をmin/maxの範囲内に固定する
		if( limitMinMax ) {
			v = Mathf.Clamp(v, valueMin, valueMax);
		}

		// 前回の値を保存
		_lastValue = v;

		// 反転onなら値を反転して返す
		if( negative ) {
			// 中央値オフセットの適用		
			return -v + middleOffset;
		} else {
			// 中央値オフセットの適用		
			return v + middleOffset;
		}
	}
	
	public abstract void VJPerformAction(GameObject go, float value);
	
	void Update() {
		if(!multiple) {
			VJPerformAction(gameObject, _GetValue(ref lastValue, ref variationOcterb));
		} else if(targets != null) {			
			foreach(VJModifierTarget t in targets) {
				VJPerformAction(t.gameObject, _GetValue(ref t.lastValue, ref t.octerb));
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
