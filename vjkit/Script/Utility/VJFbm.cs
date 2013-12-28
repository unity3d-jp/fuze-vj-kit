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

public class VJFbm : MonoBehaviour {

	[Range(1, 4)]
	public int octave = 2;
	[Range(0.0f, 4.0f)]
	public float frequency = 0.5f;
	[Range(0.0f, 0.5f)]
	public float positionAmount = 0.15f;
	[Range(0.0f, 5.0f)]
	public float rotationAmount = 1.5f;

	public void ApplyFbmMotion() {
		float time = Time.time * frequency;
		float dx = VJPerlin.Fbm(time, octave);
		float dy = VJPerlin.Fbm(time + 10, octave);
		float dz = VJPerlin.Fbm(time + 20, octave);
		float rx = VJPerlin.Fbm(time + 30, octave);
		float ry = VJPerlin.Fbm(time + 40, octave);
		transform.localPosition = new Vector3(dx, dy, dz) * positionAmount;
		transform.localRotation =
			Quaternion.AngleAxis(rx * rotationAmount, Vector3.right) *
			Quaternion.AngleAxis(ry * rotationAmount, Vector3.up);
	}
}
