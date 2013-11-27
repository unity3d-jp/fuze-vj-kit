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

[CustomEditor(typeof(VJManager))]
[CanEditMultipleObjects]
public class VJManagerEditor : Editor 
{
	public SerializedProperty isMicSourceProperty;
	public SerializedProperty micDeviceNameProperty;
	public SerializedProperty channelProperty;
	public SerializedProperty sourceProperty;

	public void OnEnable() {
		isMicSourceProperty = serializedObject.FindProperty("isMicSource");
		micDeviceNameProperty = serializedObject.FindProperty("micDeviceName");
		channelProperty = serializedObject.FindProperty("channel");
		sourceProperty = serializedObject.FindProperty("source");
	}
	
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		serializedObject.Update();

		if( !serializedObject.isEditingMultipleObjects ) {
			AudioSource asrc = sourceProperty.objectReferenceValue as AudioSource;
			if( asrc != null && asrc.clip != null ) {
				List<string> channels = new List<string>();
				for(int i=0; i< asrc.clip.channels; ++i) {
					channels.Add ( "Ch " + i );
				}
				//channels.Add ("All");

				channelProperty.intValue = EditorGUILayout.Popup(
					"Sampling channel:",
					channelProperty.intValue, 
					channels.ToArray());
			}
		}

		bool b = EditorGUILayout.Toggle("Use Microphone", isMicSourceProperty.boolValue);
		if (GUI.changed) {
			isMicSourceProperty.boolValue = b;
		}

		if( isMicSourceProperty.boolValue ) {
			string[] micDevices = Microphone.devices;
			if( micDevices.Length > 0 ) {
				int index = 0;

				EditorGUI.showMixedValue = micDeviceNameProperty.hasMultipleDifferentValues;

				if( micDeviceNameProperty.hasMultipleDifferentValues ) {
					index = -1;
				} else {
					for(int i=0; i< micDevices.Length;++i) {
						if( micDevices[i] == micDeviceNameProperty.stringValue ) {
							index = i;
							break;
						}
					}
				}

				index = EditorGUILayout.Popup(
					"Mic Device:",
					index, 
					micDevices);

				if( index >= 0 ) {
					micDeviceNameProperty.stringValue = micDevices[index];
				}
			} else {
				EditorGUILayout.LabelField("No Microphone found");
			}
		}

		serializedObject.ApplyModifiedProperties();
    }
}
