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

[CustomEditor(typeof(VJBaseModifier))]
[CanEditMultipleObjects]
public class VJBaseModifierEditor : Editor 
{
	public int index;
	public int index_src;

	public SerializedProperty managerProperty;
	public SerializedProperty boostManagerProperty;
	public SerializedProperty datasourceProperty;
	public SerializedProperty boostDatasourceProperty;
	public SerializedProperty boostByOtherSourceProperty;
	public SerializedProperty multipleProperty;
	public SerializedProperty isModifierEnabledProperty;

	public void OnEnable() {
		managerProperty = serializedObject.FindProperty("manager");
		datasourceProperty = serializedObject.FindProperty("source");
		boostDatasourceProperty = serializedObject.FindProperty("boostSource");
		boostByOtherSourceProperty = serializedObject.FindProperty("boostByOtherSource");
		boostManagerProperty = serializedObject.FindProperty("boostManager");
		multipleProperty = serializedObject.FindProperty("multiple");
		isModifierEnabledProperty = serializedObject.FindProperty("isModifierEnabled");
	}

    public override void OnInspectorGUI()
    {
		// Setup the SerializedProperties
		serializedObject.Update();
        GUI.changed = false;

		// modifier = target as VJBaseModifier;

		if(!serializedObject.isEditingMultipleObjects) {
			if(!isModifierEnabledProperty.boolValue) {
				EditorGUILayout.HelpBox("Diabled from On-Off Switch", MessageType.None);
			}
		}

		VJAbstractManager[] managers = UnityEngine.Object.FindObjectsOfType(typeof(VJAbstractManager)) as VJAbstractManager[];
		if( managers.Length > 0 ) {
			string[] options_managers = new string[managers.Length];
			for(int i = 0; i < managers.Length; ++i) {
				options_managers[i] = managers[i].gameObject.name;
				if( managerProperty.objectReferenceValue == managers[i] ) {
					index = i;
				}
			}
			if( managerProperty.objectReferenceValue == null ) {
				EditorGUILayout.HelpBox ("Manager is currently null. please select and set value.", MessageType.Warning);
			}
			
			index = EditorGUILayout.Popup(
				"Manager:",
				index, 
				options_managers);
			
			managerProperty.objectReferenceValue = managers[index];

			GameObject go = managers[index].gameObject;
			VJAbstractDataSource[] sources = go.GetComponents<VJAbstractDataSource>() as VJAbstractDataSource[];
			if( sources.Length > 0 ) {
				string[] options_sources = new string[sources.Length];
				for(int i = 0; i < sources.Length; ++i) {
					options_sources[i] = sources[i].sourceName;
					if( datasourceProperty.objectReferenceValue == sources[i] ) {
						index_src = i;
					}
				}

				EditorGUI.showMixedValue = datasourceProperty.hasMultipleDifferentValues;
				index_src = EditorGUILayout.Popup(
					"DataSource:",
					(datasourceProperty.hasMultipleDifferentValues?-1:index_src), 
					options_sources);
				EditorGUI.showMixedValue = false;

				//modifier.source = sources[index_src];
				if( index_src >= sources.Length ) {
					index_src = 0;
				}
				if( index_src >= 0 ) {
					datasourceProperty.objectReferenceValue = sources[index_src];
				}
				if( datasourceProperty.objectReferenceValue == null ) {
					EditorGUILayout.HelpBox ("DataSource is currently null. please select and set value.", MessageType.Warning);
				}

				if(boostByOtherSourceProperty.boolValue) {
					int index_boostMgrs = 0;
					for(int i = 0; i < managers.Length; ++i) {
						if( boostManagerProperty.objectReferenceValue == managers[i] ) {
							index_boostMgrs = i;
						}
					}
					EditorGUI.showMixedValue = boostManagerProperty.hasMultipleDifferentValues;
					index_boostMgrs = EditorGUILayout.Popup(
						"BoostManager:",
						index_boostMgrs, 
						options_managers);
					EditorGUI.showMixedValue = false;
					boostManagerProperty.objectReferenceValue = managers[index_boostMgrs];

					go = managers[index_boostMgrs].gameObject;
					VJAbstractDataSource[] boostSources = go.GetComponents<VJAbstractDataSource>() as VJAbstractDataSource[];
					index_src = 0;
					if( boostSources.Length > 0 ) {
						string[] options_boostSources = new string[boostSources.Length];
						for(int i = 0; i < boostSources.Length; ++i) {
							options_boostSources[i] = boostSources[i].sourceName;
							if( boostDatasourceProperty.objectReferenceValue == boostSources[i] ) {
								index_src = i;
							}
						}
						
						EditorGUI.showMixedValue = boostDatasourceProperty.hasMultipleDifferentValues;
						index_src = EditorGUILayout.Popup(
							"DataSource:",
							(boostDatasourceProperty.hasMultipleDifferentValues?-1:index_src), 
							options_boostSources);
						EditorGUI.showMixedValue = false;
						
						if( index_src >= 0 ) {
							boostDatasourceProperty.objectReferenceValue = boostSources[index_src];
						}
					}
				}
				//EditorUtility.SetDirty(target);
			} else {
				EditorGUILayout.LabelField("No DataSource found");
			}			
		} else {
			EditorGUILayout.LabelField("No VJManager found");
		}

		serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();

		if(GUILayout.Button("Target Children")) {
			if( serializedObject.isEditingMultipleObjects )
			{
				foreach(UnityEngine.Object o in targets) {
					VJBaseModifier m = o as VJBaseModifier;
					m.multiple = true;
					m.SetVisibleChildrenAsTarget();
				}
			} else {
				VJBaseModifier m = target as VJBaseModifier;
				m.multiple = true;
				m.SetVisibleChildrenAsTarget();
			}
		}

		Rect r = GUILayoutUtility.GetLastRect();
		r.width -= 16;
		r.x += 8;
		if( serializedObject.isEditingMultipleObjects ) 
		{
			foreach(UnityEngine.Object o in targets) {
				//EditorGUILayout.Space();
				VJBaseModifier m = o as VJBaseModifier;
				r.y += 20;
				r.height = 16;
				EditorGUILayout.BeginVertical();
				GUILayout.Space(20.0f);
				EditorGUI.ProgressBar(r, m.lastReturnedValue / VJAbstractDataSource.s_prog_max, m.gameObject.name + " value:"+m.lastReturnedValue);
				EditorGUILayout.EndVertical();
			}
		} else {
			VJBaseModifier m = target as VJBaseModifier;
			r.y += 20;
			r.height = 16;
			EditorGUILayout.BeginVertical();
			GUILayout.Space(20.0f);
			EditorGUI.ProgressBar(r, m.lastReturnedValue / VJAbstractDataSource.s_prog_max, "value:"+m.lastReturnedValue);
			EditorGUILayout.EndVertical();
		}

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }	
	}
}
