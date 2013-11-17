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

[CustomEditor(typeof(VJMaterialLerpModifier))]
public class VJMaterialLerpModifierEditor : VJBaseModifierEditor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		VJMaterialLerpModifier ml = target as VJMaterialLerpModifier;
			
		Material[] mats = ml.renderer.sharedMaterials;

        string[] matNames = new string[mats.Length];
        int index = 0;

        for (int i = 0; i < mats.Length; i++)
        {
            matNames[i] = mats[i].name;
        }

        EditorGUILayout.BeginHorizontal();

        index = EditorGUILayout.Popup("Material to Lerp", ml.matIndex, matNames);
		ml.matIndex = index;

        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }        
    }
}
