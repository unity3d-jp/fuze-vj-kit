using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJMaterialPropertyModifier2))]
public class VJMaterialPropertyModifier2Editor : VJBaseModifierEditor 
{
    public VJMaterialPropertyModifier2Editor()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

		VJMaterialPropertyModifier2 ml = target as VJMaterialPropertyModifier2;
			
		Material[] mats = ml.renderer.sharedMaterials;

        string[] matNames = new string[mats.Length];
        int index = 0;

        for (int i = 0; i < mats.Length; i++)
        {
            matNames[i] = mats[i].name;
        }

        EditorGUILayout.BeginHorizontal();

        index = EditorGUILayout.Popup("Material to Modify", ml.matIndex, matNames);
		ml.matIndex = index;

        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
