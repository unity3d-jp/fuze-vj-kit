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
