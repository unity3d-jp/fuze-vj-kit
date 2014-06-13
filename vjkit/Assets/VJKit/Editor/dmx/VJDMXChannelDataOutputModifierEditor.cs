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

[CustomEditor(typeof(VJDMXChannelDataOutputModifier))]
[CanEditMultipleObjects]
public class VJDMXChannelDataOutputModifierEditor : VJBaseModifierEditor 
{
	public int dmxControllerIndex;
	
	public override void OnInspectorGUI() {
        base.OnInspectorGUI();
		
		VJDMXChannelDataOutputModifier modifier = target as VJDMXChannelDataOutputModifier;

		DMXAbstractController[] dmxControllers = UnityEngine.Object.FindObjectsOfType(typeof(DMXAbstractController)) as DMXAbstractController[];
		if( dmxControllers.Length > 0 ) {
			string[] options_dmxControllers = new string[dmxControllers.Length];
			for(int i = 0; i < dmxControllers.Length; ++i) {
				options_dmxControllers[i] = dmxControllers[i].gameObject.name;
				if( modifier.dmxController == dmxControllers[i] ) {
					dmxControllerIndex = i;
				}
			}
			
			dmxControllerIndex = EditorGUILayout.Popup(
				"DMX Controller",
				dmxControllerIndex, 
				options_dmxControllers);
			
			modifier.dmxController = dmxControllers[dmxControllerIndex];
		} else {
			EditorGUILayout.LabelField("No DMXController found");
		}

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }	
	}
}
