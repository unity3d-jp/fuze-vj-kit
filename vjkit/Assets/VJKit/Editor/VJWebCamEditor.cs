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
        
        EditorGUIUtility.LookLikeInspector();
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
