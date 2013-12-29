using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

public abstract class DMXAbstractController : MonoBehaviour {
	private bool showSliders;
	
	// Debug Window	properties
	protected static int windowIDSeed = 20000;
	protected Rect windowRect = new Rect(160, 20, 120, 50);
	protected int windowId;
	public bool debugWindow = true;
	
	protected int page = 0;
	
	private byte[] currentChannelData = new byte[512];
    private byte[] oldChannelData = new byte[512];

	public virtual void Awake() {
		windowRect.x = PlayerPrefs.GetFloat("dmx.window.pos." + gameObject.name + ".x", Screen.width - 400.0f);
		windowRect.y = PlayerPrefs.GetFloat("dmx.window.pos." + gameObject.name + ".y", 20.0f);
		debugWindow = 1 == PlayerPrefs.GetInt("dmx.window." + gameObject.name + ".debug", 1);
		showSliders = 1 == PlayerPrefs.GetInt("dmx.window." + gameObject.name + ".showSliders", 0);
		windowRect.width = 400;
		windowId = windowIDSeed;
		windowIDSeed++;
	}
	
	protected virtual void OnDrawGUIWindow(int windowID) {
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		
		showSliders = GUILayout.Toggle(showSliders, "Show sliders");

		GUILayout.EndHorizontal();
		
		if (showSliders) {
			string[] pages= new string[8];
			for (int i = 0; i < pages.Length; i++) {
				pages[i] = string.Format("{0}-{1}", i * 64 + 1, i * 64 + 64);
			}
			page = GUILayout.SelectionGrid(page, pages, pages.Length);
			
			for (int i = 0; i < 2; i++) {
				GUILayout.BeginHorizontal();
				
				for (int j = 0; j < 32; j++) {
					float num = currentChannelData[page * 64 + i * 32 + j] / 255.0f;
					num = GUILayout.VerticalSlider(num, 1.0f, 0.0f);
					currentChannelData[page * 64 + i * 32 + j] = (byte)(num * 255.0f);
				}
	
				GUILayout.EndHorizontal();
			}
		}

		GUI.DragWindow();
		GUILayout.EndVertical();
	}
	
	public virtual void OnGUI() {
		if(debugWindow) {
			windowRect = GUILayout.Window(windowId, windowRect, OnDrawGUIWindow, name, GUILayout.Width(200));
		}
	}
	
	public virtual void OnApplicationQuit() {
		PlayerPrefs.SetFloat("dmx.window.pos." + gameObject.name + ".x", windowRect.x);
		PlayerPrefs.SetFloat("dmx.window.pos." + gameObject.name + ".y", windowRect.y);
		PlayerPrefs.SetInt("dmx.window." + gameObject.name + ".debug", (debugWindow ? 1 : 0));
		PlayerPrefs.SetInt("dmx.window." + gameObject.name + ".showSliders", (showSliders ? 1 : 0));
	}

	void LateUpdate() {
		if (enabled && !currentChannelData.SequenceEqual(oldChannelData)) {
			SendChannelData(currentChannelData);

			Buffer.BlockCopy(currentChannelData, 0, oldChannelData, 0, 512);
		}
	}
	
	public void SetChannelData(int channel, byte data) {
		currentChannelData[Mathf.Clamp(channel, 1, 512) - 1] = data;
	}

	protected abstract void SendChannelData(byte[] channelData);
}
