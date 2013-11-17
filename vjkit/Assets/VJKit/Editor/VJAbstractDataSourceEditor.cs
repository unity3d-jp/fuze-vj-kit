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

[CustomEditor(typeof(VJAbstractDataSource))]
[CanEditMultipleObjects]
public class VJAbstractDataSourceEditor : Editor 
{
    public override void OnInspectorGUI()
    {
		serializedObject.Update();

		GUI.changed = false;
		base.OnInspectorGUI();
		EditorGUILayout.Space();
		Rect r = GUILayoutUtility.GetLastRect();
		r.x += 8;
		r.width -= 16;
		r.y += 20;
		r.height = 16;

		if(!serializedObject.isEditingMultipleObjects) {
			VJAbstractDataSource src = target as VJAbstractDataSource;
			EditorGUILayout.BeginVertical();
			GUILayout.Space(72.0f);
			EditorGUI.ProgressBar(r, src.current / VJAbstractDataSource.s_prog_max, "Current:"+src.current);
			r.y += 20;
			EditorGUI.ProgressBar(r, src.previous / VJAbstractDataSource.s_prog_max, "Previous:"+src.previous);
			r.y += 20;
			EditorGUI.ProgressBar(r, src.diff / VJAbstractDataSource.s_prog_max, "Difference:"+src.diff);
			EditorGUILayout.EndVertical();
		} else {
			EditorGUILayout.BeginVertical();
			foreach(UnityEngine.Object o in targets){
				VJAbstractDataSource src = o as VJAbstractDataSource;
				GUILayout.Space(24.0f);
				EditorGUI.ProgressBar(r, src.current / VJAbstractDataSource.s_prog_max, "Current("+ src.name +"):"+src.current);
				r.y += 20;
			}
			GUILayout.Space(2.0f);
			EditorGUILayout.EndVertical();
		}

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }

		serializedObject.ApplyModifiedProperties();
	}
}
