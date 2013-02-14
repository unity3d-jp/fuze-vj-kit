using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJBaseTrigger))]
public class VJBaseTriggerEditor : Editor 
{
	public VJBaseTrigger trigger;
	public int index;
	public int index_src;

    public VJBaseTriggerEditor()
    {
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

		trigger = target as VJBaseTrigger;

  		VJManager[] managers = UnityEngine.Object.FindObjectsOfType(typeof(VJManager)) as VJManager[];
  		if( managers.Length > 0 ) {
			string[] options_managers = new string[managers.Length];
			for(int i = 0; i < managers.Length; ++i) {
				options_managers[i] = managers[i].gameObject.name;
				if( trigger.manager == managers[i] ) {
					index = i;
				}
			}
			
			index = EditorGUILayout.Popup(
				"Manager:",
				index, 
				options_managers);

			trigger.manager = managers[index];
            //EditorUtility.SetDirty(target);
			
			GameObject go = managers[index].gameObject;
			VJDataSource[] sources = go.GetComponents<VJDataSource>() as VJDataSource[];
			if( sources.Length > 0 ) {
				string[] options_sources = new string[sources.Length];
				for(int i = 0; i < sources.Length; ++i) {
					options_sources[i] = sources[i].sourceName;
					if( trigger.source == sources[i] ) {
						index_src = i;
					}
				}
			
				index_src = EditorGUILayout.Popup(
					"DatSource:",
					index_src, 
					options_sources);
				
				trigger.source = sources[index_src];
            	//EditorUtility.SetDirty(target);
			} else {
				EditorGUILayout.LabelField("No DataSource found");
			}			
  		} else {
  			EditorGUILayout.LabelField("No VJManager found");
  		}

        base.OnInspectorGUI();

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}
