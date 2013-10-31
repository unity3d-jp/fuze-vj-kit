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
	public SerializedProperty datasourceProperty;
	public SerializedProperty boostDatasourceProperty;
	public SerializedProperty boostByOtherSourceProperty;
	public SerializedProperty multipleProperty;
	public SerializedProperty isModifierEnabledProperty;

    public VJBaseModifierEditor()
    {
    }

	public void OnEnable() {
		managerProperty = serializedObject.FindProperty("manager");
		datasourceProperty = serializedObject.FindProperty("source");
		boostDatasourceProperty = serializedObject.FindProperty("boostSource");
		boostByOtherSourceProperty = serializedObject.FindProperty("boostByOtherSource");
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

				index_src = EditorGUILayout.Popup(
					"DataSource:",
					(datasourceProperty.hasMultipleDifferentValues?-1:index_src), 
					options_sources);
			
				//modifier.source = sources[index_src];
				if( index_src >= 0 ) {
					datasourceProperty.objectReferenceValue = sources[index_src];
				}

				if(boostByOtherSourceProperty.boolValue) {
					index_src = 0;
					for(int i = 0; i < sources.Length; ++i) {
						if( boostDatasourceProperty.objectReferenceValue == sources[i] ) {
							index_src = i;
							break;
						}
					}
					
					index_src = EditorGUILayout.Popup(
						"BoostSource:",
						(boostDatasourceProperty.hasMultipleDifferentValues?-1:index_src), 
						options_sources);
					
					//modifier.boostSource = sources[index_src];
					if( index_src >= 0 ) {
						boostDatasourceProperty.objectReferenceValue = sources[index_src];
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
