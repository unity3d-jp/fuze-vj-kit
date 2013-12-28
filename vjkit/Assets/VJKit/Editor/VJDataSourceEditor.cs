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
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJDataSource))]
[CanEditMultipleObjects]
public class VJDataSourceEditor : VJAbstractDataSourceEditor 
{
	public SerializedProperty lowerBandProperty;
	public SerializedProperty upperBandProperty;

	public void OnEnable() {
		lowerBandProperty = serializedObject.FindProperty("lowerBand");
		upperBandProperty = serializedObject.FindProperty("upperBand");
	}
	
	public override void OnInspectorGUI()
    {
		base.OnInspectorGUI();
		serializedObject.Update();

		Rect r = GUILayoutUtility.GetLastRect();
		r.y -= 6;

		float lowerband = (float)lowerBandProperty.intValue;
		float upperBand = (float)upperBandProperty.intValue;
		EditorGUI.MinMaxSlider(new GUIContent("Band["+lowerband+":"+upperBand+"]"),  r, ref lowerband, ref upperBand, 0, 7 ); 
		lowerBandProperty.intValue = (int)lowerband;
		upperBandProperty.intValue = (int)upperBand;

		serializedObject.ApplyModifiedProperties();
	}
}
