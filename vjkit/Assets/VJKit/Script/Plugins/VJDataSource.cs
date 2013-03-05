using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/VJ Data source(Spectrum)")]
[RequireComponent (typeof (VJManager))]
public class VJDataSource : VJAbstractDataSource {

	[Range(0, 7)]	// depends VJManager.numberOfBands
	public int lowerBand;	
	[Range(0, 7)]	// depends VJManager.numberOfBands
	public int upperBand;
	
	private VJManager m_manager;

	// Use this for initialization
	public void Awake() {
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

	// Update is called once per frame
	public void Update () {
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
