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
