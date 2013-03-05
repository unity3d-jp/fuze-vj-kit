using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJBaseModifier))]
public class VJBaseModifierEditor : Editor 
{
	public VJBaseModifier modifier;
	public int index;
	public int index_src;

    public VJBaseModifierEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

		modifier = target as VJBaseModifier;

		if(!modifier.isModifierEnabled) {
			EditorGUILayout.HelpBox("Diabled from On-Off Switch", MessageType.None);
		}

  		VJAbstractManager[] managers = UnityEngine.Object.FindObjectsOfType(typeof(VJAbstractManager)) as VJAbstractManager[];
  		if( managers.Length > 0 ) {
			string[] options_managers = new string[managers.Length];
			for(int i = 0; i < managers.Length; ++i) {
				options_managers[i] = managers[i].gameObject.name;
				if( modifier.manager == managers[i] ) {
					index = i;
				}
			}
			
			index = EditorGUILayout.Popup(
				"Manager:",
				index, 
				options_managers);

			modifier.manager = managers[index];
            //EditorUtility.SetDirty(target);
			
			GameObject go = managers[index].gameObject;
			VJAbstractDataSource[] sources = go.GetComponents<VJAbstractDataSource>() as VJAbstractDataSource[];
			if( sources.Length > 0 ) {
				string[] options_sources = new string[sources.Length];
				for(int i = 0; i < sources.Length; ++i) {
					options_sources[i] = sources[i].sourceName;
					if( modifier.source == sources[i] ) {
						index_src = i;
					}
				}
			
				index_src = EditorGUILayout.Popup(
					"DatSource:",
					index_src, 
					options_sources);
				
				modifier.source = sources[index_src];
            	//EditorUtility.SetDirty(target);
			} else {
				EditorGUILayout.LabelField("No DataSource found");
			}			
  		} else {
  			EditorGUILayout.LabelField("No VJManager found");
  		}

        base.OnInspectorGUI();

//		modifier.multiple = EditorGUILayout.Toggle("Multiple", modifier.multiple);
//		if(modifier.multiple) {
////			Rect r = GUILayoutUtility.GetLastRect();
////			r.x += 8;
////			r.width -= 16;
////			r.y += 20;
////			r.height = 16;
//			//GUILayout.Space(72.0f);
//			EditorGUILayout.PropertyField (serializedObject.FindProperty ("targets"), GUIContent.none);
//		}

		if(GUILayout.Button("Target Children")) {
			modifier.multiple = true;
			modifier.SetVisibleChildrenAsTarget();
		}

		//EditorGUILayout.Space();
		Rect r = GUILayoutUtility.GetLastRect();
		r.x += 8;
		r.width -= 16;
		r.y += 20;
		r.height = 16;
		EditorGUILayout.BeginVertical();
		GUILayout.Space(32.0f);
		EditorGUI.ProgressBar(r, modifier.lastReturnedValue / VJAbstractDataSource.s_prog_max, "value:"+modifier.lastReturnedValue);
		EditorGUILayout.EndVertical();

        
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
