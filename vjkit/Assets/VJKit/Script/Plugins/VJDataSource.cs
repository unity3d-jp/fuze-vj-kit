using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/VJ Data source")]
[RequireComponent (typeof (VJManager))]
public class VJDataSource : MonoBehaviour {

	public string sourceName;
	[Range(0, 7)]	// depends VJManager.numberOfBands
	public int lowerBand;	
	[Range(0, 7)]	// depends VJManager.numberOfBands
	public int upperBand;

	[Range(0.01f, 100.0f)]
	public float boost = 1.0f;

//	[HideInInspector]
	public float current;
//	[HideInInspector]
	public float previous;
//	[HideInInspector]
	public float diff;
	
	private VJManager m_manager;

	// Use this for initialization
	void Awake() {
		if( lowerBand >= VJManager.numberOfBands ) {
			Debug.LogWarning("[FATAL]lowerBand too large.");
			lowerBand = VJManager.numberOfBands-1;
		}
		if( upperBand >= VJManager.numberOfBands ) {
			Debug.LogWarning("[FATAL]lowerBand too large.");
			upperBand = VJManager.numberOfBands-1;
		}
		if( upperBand < lowerBand ) {
			Debug.LogWarning("upperBand can not be lower than lowerBand.");
			upperBand = lowerBand;
		}
		m_manager = GetComponent<VJManager>();
	}

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float sum = 0.0f;
		for(int i = lowerBand; i <= upperBand; ++i) {
			sum += m_manager.bandLevels[i] * m_manager.bandLevels[i];
		}
		float raw_current = Mathf.Sqrt( sum / (upperBand - lowerBand + 1) ) * boost;

		previous = current;
		current = raw_current;
		diff = current - previous;
	}
}
