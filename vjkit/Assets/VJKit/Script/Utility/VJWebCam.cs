using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Web Camera")]
public class VJWebCam : MonoBehaviour {

	[HideInInspector]
	public WebCamTexture webcam;

// デバイスリストへのアクセスを試みると2回目以降のプレビューで
// デバイスが見つからなくなるので、アクセスするのをやめる
//	[HideInInspector]
//	public string deviceName;

	[TextureSize]
	public int width = 512;
	[TextureSize]
	public int height = 512;
	[Range (1, 120)]
	public int fps = 60;
	public Material targetMaterial;
	public string texturePropertyName;

    //private Rect windowRect = new Rect(120, 20, 120, 50);
	//private bool drawGui;

	// Use this for initialization
	void Start () {		
//		deviceName = PlayerPrefs.GetString("VJWebCam.deviceName", "");
		SetWebCamTexture(width, height, fps);
	}
	
	public void SetWebCamTexture(int w, int h, int f) {

		width = w;
		height = h;
		fps = f;

		if(Application.isPlaying){

			if( null != webcam ) {
				webcam.Stop();	
				Destroy(webcam);
			}

			webcam = new WebCamTexture(w, h, f);
			
			if(targetMaterial == null) {
				targetMaterial = renderer.material;
			}
			
			if(texturePropertyName == null || texturePropertyName.Length == 0) {
				targetMaterial.mainTexture = webcam;
			} else {
				targetMaterial.SetTexture(texturePropertyName, webcam);
			}
			
			webcam.Play();
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
//		GUILayout.Label("Webcam Device:");
//		deviceName = GUILayout.TextField(deviceName, 80);
//		GUILayout.EndHorizontal();
//		GUILayout.Label("Available Device:");
//		
//		WebCamDevice[] devs = WebCamTexture.devices;
//
//		for (int i = 0; i < devs.Length; i++)
//		{
//			GUILayout.Label("    "  + devs[i].name);
//		}
//		
//		if(GUILayout.Button("Save")) {
//			PlayerPrefs.SetString("VJWebCam.deviceName", deviceName);
//		}
//		
//		GUILayout.EndVertical();
//    }
//	
//	public void OnGUI() {
//		if( drawGui ) {
//        	windowRect = GUILayout.Window(1, windowRect, _DrawGUIWindow, "WebCam Setting", GUILayout.Width(100));
//		}
//	}

}
