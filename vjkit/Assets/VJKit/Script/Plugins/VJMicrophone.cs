using UnityEngine;
using System.Collections;

public class VJMicrophone : MonoBehaviour {

	private int sampleRate = 44100;
	private int clipLength = 4;
	private float analysisWindow = 0.2f;
	private AudioClip micClip;

	public float level = 0.0f;
	public int micSemaphor = 0;
	public string deviceName = "";

	private bool drawGui;

	private static VJMicrophone s_instance;

    public Rect windowRect = new Rect(20, 20, 120, 50);

	public static VJMicrophone GetInstance() {

		if( !s_instance ) {
			GameObject go = new GameObject();
			go.name = "VJMicrophone";
			DontDestroyOnLoad(go);
			s_instance = go.AddComponent<VJMicrophone>();
			var asObj = go.AddComponent<AudioSource>() as AudioSource;
			asObj.volume = 1.0f;
			asObj.mute = true;
			asObj.loop = true;
			asObj.playOnAwake = false;
		}

		return s_instance;
	}

	private void CheckSampleRate(string dName) {
		int minFreq = 0;
		int maxFreq = 0;
		Microphone.GetDeviceCaps(dName, out minFreq, out maxFreq);
		if (minFreq > 0 && maxFreq > 0) {
			sampleRate = Mathf.Clamp(sampleRate, minFreq, maxFreq);
		}
	}

	private IEnumerator _StartMicrophone () {
		
		deviceName = PlayerPrefs.GetString("VJMicrophone.deviceName", "");
	
		while(micSemaphor > 0) {

			string currentDeviceName = deviceName;
			CheckSampleRate(currentDeviceName);

			Debug.Log("Microphone starting: " + currentDeviceName + " sampling rate:" + sampleRate);

			micClip = Microphone.Start(currentDeviceName, true, clipLength, sampleRate);
			float[] samples = new float[(int)(analysisWindow * sampleRate)];
			audio.clip = micClip;
			audio.Play();

			while (currentDeviceName == deviceName && micSemaphor > 0) {
				yield return 0;

				int position = Microphone.GetPosition(deviceName);
				if (position < samples.Length) position += clipLength * sampleRate;
				micClip.GetData(samples, position - samples.Length);
				audio.timeSamples = position;

				float rms = 0.0f;
				foreach (float lvl in samples) {
					rms += lvl * lvl;
				}
				rms = Mathf.Sqrt(rms / samples.Length);

				level = Mathf.Clamp01( 0.5f * (2.0f + Mathf.Log10(rms)) );
			}
		
			audio.Stop();
			audio.clip = null;
			Debug.Log("Microphone stopping: " + currentDeviceName );
			Microphone.End(currentDeviceName);
		}
	}
	
	public AudioSource StartMicrophone() {
		Debug.Log("StartMicrophone: " + deviceName );
		micSemaphor += 1;
		if(micSemaphor == 1) {
			StartCoroutine(_StartMicrophone());
		}
		return gameObject.GetComponent<AudioSource>();
	}

	public void StopMicrophone() {
		Debug.Log("StopMicrophone: " + deviceName );
		if(micSemaphor > 0) {
			micSemaphor -= 1;
		}
	}
	
	public void Update() {
		 if (Input.GetKeyDown (KeyCode.Space)) {
		 	drawGui = !drawGui;
		 }
	}
	
    private void _DrawGUIWindow(int windowID) {
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Mic Device:");
		deviceName = GUILayout.TextField(deviceName, 80);
		GUILayout.EndHorizontal();
		GUILayout.Label("Available Device:");

		for (int i = 0; i < Microphone.devices.Length; i++)
		{
			GUILayout.Label("    "  + Microphone.devices[i]);
		}
		
		if(GUILayout.Button("Save")) {
			PlayerPrefs.SetString("VJMicrophone.deviceName", deviceName);
		}
		
		GUILayout.EndVertical();

		VJManager[] managers = UnityEngine.Object.FindObjectsOfType(typeof(VJManager)) as VJManager[];
		foreach(VJManager m in managers) {
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			bool newMic = GUILayout.Toggle(m.isMicSource, m.gameObject.name + " uses Microphone:");
			m.ToogleMicSource(newMic);
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
    }
	
	public void OnGUI() {
		if( drawGui ) {
        	windowRect = GUILayout.Window(0, windowRect, _DrawGUIWindow, "Microphone Setting", GUILayout.Width(100));
		}
	}
}
