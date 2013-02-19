using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VJMicrophone))]
public class VJMicrophoneEditor : Editor 
{
    public VJMicrophoneEditor()
    {
    }

    public override void OnInspectorGUI()
    {
    	VJMicrophone mic = target as VJMicrophone;
    
        GUI.changed = false;

        base.OnInspectorGUI();
        
        string[] deviceNames = new string[Microphone.devices.Length];
        deviceNames[0] = "";
        int index = 0;

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            deviceNames[i] = Microphone.devices[i];
            if(Microphone.devices[i] == mic.deviceName) {
            	index = i;
            }
        }

		if( mic.deviceName == null || mic.deviceName.Length == 0 ) {
			index = 0;
		}

        EditorGUILayout.BeginHorizontal();

        
        index = EditorGUILayout.Popup("Mic Device", index, deviceNames);
		mic.deviceName = deviceNames[index];
		PlayerPrefs.SetString("VJMicrophone.deviceName", mic.deviceName);

        EditorGUILayout.EndHorizontal();

        if (GUI.changed) {
            EditorUtility.SetDirty(target);
        }
    }
}
