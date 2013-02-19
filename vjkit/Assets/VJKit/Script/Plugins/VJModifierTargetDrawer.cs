using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer (typeof(VJModifierTarget))]
public class VJModifierTargetDrawer : PropertyDrawer {
    
    public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
		return base.GetPropertyHeight (prop, label);
    }    
    
    // Draw the property inside the given rect
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty (position, label, property);
        
        // Draw label
        position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
        
        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        // Calculate rects
        var goRect 		= new Rect (position.x, position.y, 200, position.height);
        var octerbRect 	= new Rect (position.x+205, position.y, position.width - (position.x + 51), position.height);
        
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        //EditorGUI.ObjectField(goRect, prop.objectReferenceValue)
        EditorGUI.PropertyField (goRect, property.FindPropertyRelative ("gameObject"), GUIContent.none);
        EditorGUI.PropertyField (octerbRect, property.FindPropertyRelative ("octerb"), GUIContent.none);
        
        // Set indent back to what it was
        EditorGUI.indentLevel = indent;
        
        EditorGUI.EndProperty ();
    }
}
