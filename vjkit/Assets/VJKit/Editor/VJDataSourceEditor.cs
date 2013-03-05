using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(VJDataSource))]
public class VJDataSourceEditor : VJAbstractDataSourceEditor 
{

    public VJDataSourceEditor()
    {
    }

    public override void OnInspectorGUI()
    {
		VJDataSource src = target as VJDataSource;

        base.OnInspectorGUI();

		Rect r = GUILayoutUtility.GetLastRect();
		r.y -= 6;
		
		float lowerband = (float)src.lowerBand, upperBand = (float)src.upperBand;
		EditorGUI.MinMaxSlider(new GUIContent("Band"),  r, ref lowerband, ref upperBand, 0, 7 ); 
		src.lowerBand = (int)lowerband;
		src.upperBand = (int)upperBand;
		
    }
}
