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

[CustomEditor(typeof(VJWebCam))]
public class VJWebCamEditor : Editor 
{
	private static string[] deviceNames = null;

	/*
		WebCamTexture.devices へのアクセスは遅いので、1回以上行わない
		ようにする
	*/
	private static string[] _GetDeviceNames() {
		if(deviceNames == null) {
			WebCamDevice[] devices = WebCamTexture.devices;
			deviceNames = new string[devices.Length];
			
			for (int i = 0; i < devices.Length; i++)
			{
				deviceNames[i] = devices[i].name;
			}
		}
		return deviceNames;
	}

    public VJWebCamEditor()
    {
    }

    public override void OnInspectorGUI()
    {
    	VJWebCam cam = target as VJWebCam;
    
        GUI.changed = false;
        
        base.OnInspectorGUI();
//        EditorGUILayout.BeginHorizontal();
//        
//        string[] names = _GetDeviceNames();
//
//        if( names.Length > 0 ) {
//			int index = 0;
//
//			for (int i = 0; i < names.Length; i++)
//			{
//				if(names[i] == cam.deviceName) {
//					index = i;
//				}
//			}
//			
//			if( cam.deviceName == null || cam.deviceName.Length == 0 ) {
//				index = 0;
//			}
//			
//			index = EditorGUILayout.Popup("WebCam Device", index, names);
//			if( cam.deviceName != names[index] ) {
//				cam.deviceName = names[index];
//				PlayerPrefs.SetString("VJWebCam.deviceName", cam.deviceName);	
//			}
//			
//        } else {
//        	EditorGUILayout.LabelField("No Webcam device found.");
//        }        
//
//        EditorGUILayout.EndHorizontal();

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
			cam.SetWebCamTexture(cam.width, cam.height, cam.fps);
        }
    }
}
