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


[AddComponentMenu("VJKit/System/ZigFu/Collision Datasource")]
[RequireComponent (typeof (VJZigManager))]
public class VJZigSensorFieldDataSource : VJAbstractDataSource {

	enum Axis {
		Horizontal,
		Vertical,
		Depth,
		Touching,
		TouchUp,
		TouchDown
	}

	[SerializeField]
	private VJZigSensorField m_sensorField;

	[SerializeField]
	private Axis m_axis;

	// Use this for initialization
	public void Awake() {
	}
	
	// Update is called once per frame
	public void Update () {
		previous = current;

		//float vMax = Mathf.Max (m_sensorField.ValueHrz, Mathf.Max (m_sensorField.ValueVert, m_sensorField.ValueDpth));

		switch(m_axis) {
		case Axis.Horizontal:
			current = m_sensorField.ValueHrz * boost;
			break;
		case Axis.Vertical:
			current = m_sensorField.ValueVert * boost;
			break;
		case Axis.Depth:
			current = m_sensorField.ValueDpth * boost;
			break;
		case Axis.Touching:
			current = (m_sensorField.Touching?1.0f:0.0f) * boost;
			break;
		case Axis.TouchUp:
			current = (m_sensorField.TouchUp?1.0f:0.0f) * boost;
			break;
		case Axis.TouchDown:
			current = (m_sensorField.TouchDown?1.0f:0.0f) * boost;
			break;
		}

		diff = current - previous;
	}
}
