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

[CustomEditor(typeof(VJAbstractAudioJackDataSource))]
[CanEditMultipleObjects]
public class VJAbstractAudioJackDataSourceEditor : VJAbstractDataSourceEditor 
{
	// Properties.
	SerializedProperty propDynamicRange;
	SerializedProperty propHeadroom;
	SerializedProperty propFalldown;
	SerializedProperty propLowerBound;
	SerializedProperty propPowerFactor;
	SerializedProperty propSensibility;
	SerializedProperty propShowOptions;
	
	// Texutres for drawing bars.
	Texture2D[] barTextures;
	
	// Shows the inspaector.
	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();
		
		// Show the editable properties.
		EditorGUILayout.Slider (propPowerFactor, 0.1f, 20.0f);
		EditorGUILayout.Slider (propSensibility, 0.1f, 40.0f);
		propShowOptions.boolValue = EditorGUILayout.Foldout (propShowOptions.boolValue, "Audio Input Options");
		if (propShowOptions.boolValue)
		{
			EditorGUILayout.Slider (propHeadroom, 0.0f, 20.0f, "Headroom [dB]");
			EditorGUILayout.Slider (propDynamicRange, 1.0f, 60.0f, "Dynamic Range [dB]");
			EditorGUILayout.Slider (propLowerBound, -100.0f, -10.0f, "Lower Bound [dB]");
			EditorGUILayout.Slider (propFalldown, 0.0f, 10.0f, "Falldown [dB/Sec]");
		}
		
		// Apply modifications.
		serializedObject.ApplyModifiedProperties ();
		
		// Draw the level bar on play mode.
		if (EditorApplication.isPlaying && !serializedObject.isEditingMultipleObjects)
		{
			DrawLevelBar (target as VJAbstractAudioJackDataSource);
			// Make it dirty to update the view.
			EditorUtility.SetDirty (target);
		}

		base.OnInspectorGUI();
	}
	
	// On Enable (initialization)
	void OnEnable ()
	{
		// Get references to the properties.
		propDynamicRange = serializedObject.FindProperty ("dynamicRange");
		propHeadroom = serializedObject.FindProperty ("headroom");
		propFalldown = serializedObject.FindProperty ("falldown");
		propLowerBound = serializedObject.FindProperty ("lowerBound");
		propPowerFactor = serializedObject.FindProperty ("powerFactor");
		propSensibility = serializedObject.FindProperty ("sensibility");
		propShowOptions = serializedObject.FindProperty ("showOptions");
	}
	
	// On Disable (cleanup)
	void OnDisable ()
	{
		if (barTextures != null)
		{
			// Destroy the bar textures.
			foreach (var texture in barTextures)
				DestroyImmediate (texture);
			barTextures = null;
		}
	}
	
	// Make a texture which contains only one pixel.
	Texture2D NewBarTexture (Color color)
	{
		var texture = new Texture2D (1, 1);
		texture.SetPixel (0, 0, color);
		texture.Apply ();
		return texture;
	}
	
	// Draw the input level bar.
	void DrawLevelBar (VJAbstractAudioJackDataSource ds)
	{
		if (barTextures == null)
		{
			// Make textures for drawing level bars.
			barTextures = new Texture2D[] {
				NewBarTexture (Color.red),
				NewBarTexture (Color.green),
				NewBarTexture (Color.blue),
				NewBarTexture (Color.gray)
			};
		}
		
		// Peak level label.
		EditorGUILayout.LabelField ("Peak Level", ds.Peak.ToString ("0.0") + " dB");
		
		// Get a rectangle as a text field.
		var rect = GUILayoutUtility.GetRect (18, 10, "TextField");
		var width = rect.width;
		
		// Fill the rectangle with gray.
		GUI.DrawTexture (rect, barTextures [3]);
		
		// Draw the range bar with red.
		rect.x += width * (ds.Peak - ds.lowerBound - ds.dynamicRange - ds.headroom) / (3 - ds.lowerBound);
		rect.width = width * (ds.dynamicRange + ds.headroom) / (3 - ds.lowerBound);
		GUI.DrawTexture (rect, barTextures [0]);
		
		// Draw the effective range bar with green.
		rect.width = width * (ds.dynamicRange) / (3 - ds.lowerBound);
		GUI.DrawTexture (rect, barTextures [1]);
		
		// Draw the output level bar with blue.
		rect.width = width * ds.dynamicRange * ds.Output / (3 - ds.lowerBound);
		rect.y += rect.height / 2;
		rect.height /= 2;
		GUI.DrawTexture (rect, barTextures [2]);
	}
}
