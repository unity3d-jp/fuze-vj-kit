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
using System.Collections.Generic;

public class VJMicrophoneManager : MonoBehaviour {
	
	[HideInInspector]
	public bool drawGui;

	[SerializeField]
	List<VJMicrophone> m_mics;

	private static VJMicrophoneManager s_instance;

	public static VJMicrophoneManager GetInstance() {

		if( !s_instance ) {
			GameObject go = new GameObject();
			go.name = "VJMicrophoneManager";
			DontDestroyOnLoad(go);
			VJMicrophoneManager mm = go.AddComponent<VJMicrophoneManager>() as VJMicrophoneManager;
			mm.m_mics = new List<VJMicrophone>();
			s_instance = mm;
		}

		return s_instance;
	}

	public AudioSource StartMicrophone(string deviceName) {

		foreach(VJMicrophone m in m_mics) {
			if( m.deviceName == deviceName ) {
				return m.StartMicrophone();
			}
		}

		foreach(string dn in Microphone.devices) {
			if( dn == deviceName ) {
				GameObject go = new GameObject();
				DontDestroyOnLoad(go);
				VJMicrophone m = go.AddComponent<VJMicrophone>() as VJMicrophone;
				m.deviceName = deviceName;
				m_mics.Add(m);
				return m.StartMicrophone();
			}
		}

		Debug.Log ("[MIC] no device found:" + deviceName);
		return null;
	}

	public void StopMicrophone(string deviceName) {
		foreach(VJMicrophone m in m_mics) {
			if( m.deviceName == deviceName ) {
				m.StopMicrophone();
				return;
			}
		}
	}
	
//	public void Update() {
//		 if (Input.GetKeyDown (KeyCode.Space)) {
//		 	drawGui = !drawGui;
//		 }
//	}
	
//    private void _DrawGUIWindow(int windowID) {
//		GUILayout.BeginVertical();
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("Mic Device:");
//		deviceName = GUILayout.TextField(deviceName, 80);
//		GUILayout.EndHorizontal();
//		GUILayout.Label("Available Device:");
//
//		for (int i = 0; i < Microphone.devices.Length; i++)
//		{
//			GUILayout.Label("    "  + Microphone.devices[i]);
//		}
//		
//		if(GUILayout.Button("Save")) {
//			PlayerPrefs.SetString("VJMicrophone.deviceName", deviceName);
//		}
//		
//		GUILayout.EndVertical();
//
//		VJManager[] managers = UnityEngine.Object.FindObjectsOfType(typeof(VJManager)) as VJManager[];
//		foreach(VJManager m in managers) {
//			GUILayout.BeginVertical();
//			GUILayout.BeginHorizontal();
//			bool newMic = GUILayout.Toggle(m.isMicSource, m.gameObject.name + " uses Microphone:");
//			m.ToogleMicSource(newMic);
//			GUILayout.EndHorizontal();
//			GUILayout.EndVertical();
//		}
//		GUI.DragWindow ();
//    }
//	
//	public void DrawGUI() {
//		if( drawGui ) {
//        	windowRect = GUILayout.Window(0, windowRect, _DrawGUIWindow, "Microphone Setting", GUILayout.Width(100));
//		}
//	}
}
