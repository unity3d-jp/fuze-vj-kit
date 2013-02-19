using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer (typeof(TextureSizeAttribute))]
public class TextureSizeDrawer : PropertyDrawer {
    
    public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return base.GetPropertyHeight (prop, label);
    }    
    
    // Draw the property inside the given rect
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

		string[] labels = new string[]{"64", "128", "256", "512", "1024", "2048", "4096"};
		int[] values = new int[]{64, 128, 256, 512, 1024, 2048, 4096};
        
        // First get the attribute since it contains the range for the slider
        //var range : RangeAttribute = attribute as RangeAttribute;
        
        int index = 0;
        
        for(;index < values.Length; ++index) {
        	if(values[index] == property.intValue) {
        		break;
        	}
        }
        
        if(index == values.Length) {
        	index = 0;
        }
        
		index = EditorGUILayout.Popup("Size", index, labels);
		property.intValue = values[index];        
    }
}
