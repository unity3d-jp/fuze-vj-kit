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

[RequireComponent (typeof (VJAudioJackManager))]
public abstract class VJAbstractAudioJackDataSource : VJAbstractDataSource {

	#region [reaktor] Public properties
	
//	public int bandIndex = 1;
	[HideInInspector]
	public float dynamicRange = 20.0f;
	[HideInInspector]
	public float headroom = 6.0f;
	[HideInInspector]
	public float falldown = 0.4f;
	[HideInInspector]
	public float lowerBound = -60.0f;
	[HideInInspector]
	public float powerFactor = 1.0f;
	[HideInInspector]
	public float sensibility = 18.0f;
	[HideInInspector]
	public bool showOptions;
	
	#endregion
	
	#region [reaktor] Private variables
	
	protected VJAudioJackManager m_manager;	
	float output;
	float peak;
	
	public float Output {
		get { return output; }
	}
	
	public float Peak {
		get { return peak; }
	}
	#endregion
	
	#region [reaktor] MonoBehaviour functions

	void Awake() {
		m_manager = GetComponent<VJAudioJackManager>();
	}

	void Start ()
	{
		// Begins with the lowest level.
		peak = lowerBound + dynamicRange + headroom;
	}

	protected abstract float GetInput();
	
	void Update ()
	{
		// Audio input.
		var input = GetInput();
		
		// Check the peak level.
		peak -= Time.deltaTime * falldown;
		peak = Mathf.Max (peak, Mathf.Max (input, lowerBound + dynamicRange + headroom));
		
		// Normalize the input level.
		input = (input - peak + headroom + dynamicRange) / dynamicRange;
		input = Mathf.Pow (Mathf.Clamp01 (input), powerFactor);
		
		// Interpolation.
		output = input - (input - output) * Mathf.Exp (-sensibility * Time.deltaTime);

		float raw_current = output * boost;
		
		previous = current;
		current = raw_current;
		diff = current - previous;
	}

	#endregion
}
