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

[AddComponentMenu("VJKit/System/VJ Data source(Spectrum)")]
[RequireComponent (typeof (VJManager))]
public class VJDataSource : VJAbstractDataSource {

	// depends VJManager.numberOfBands
	[HideInInspector]
	public int lowerBand;	
	// depends VJManager.numberOfBands
	[HideInInspector]
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
