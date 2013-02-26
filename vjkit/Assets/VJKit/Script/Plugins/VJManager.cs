using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/VJ Manager")]
public class VJManager : MonoBehaviour {
	
	public static int numberOfBands = 8;
	
	private static int windowIDSeed = 3000;

	public AudioSource 	source;
	public FFTWindow 	fft = FFTWindow.BlackmanHarris;
	public float[] bandLevels;

	public bool debugWindow = false;
	
	[HideInInspector]
	public bool isMicSource = false;
	private bool isMicSourceNow = false;

	private VJMicrophone mic;
	private AudioSource oldSource;

	private int spectrumSamples = 1024;
	private float intervalSec = 0.02f;
	[HideInInspector]
	private float[] rawSpectrum;

    private Rect windowRect = new Rect(160, 20, 120, 50);
	private Texture2D _debugSpectrumTex;
	private int windowId;
	
	void Awake() {
		rawSpectrum = new float[spectrumSamples];
		bandLevels = new float[numberOfBands];
		mic = VJMicrophone.GetInstance();
		
		_debugSpectrumTex = new Texture2D(16,16);

		windowRect.x = Screen.width - 220;
		windowRect.y = 20;
		windowRect.width = 200;
		windowId = windowIDSeed;
		windowIDSeed++;
	}

	IEnumerator Start() {
		
		while (true) {
			if( source ) {
				source.GetSpectrumData(rawSpectrum, 0, FFTWindow.BlackmanHarris);
				_ConvertRawSpectrumToBandLevels();
			}
			if( isMicSourceNow != isMicSource ) {
				if(mic) {
					if(isMicSource) {
						oldSource = source;
						source = mic.StartMicrophone();
					} else {
						mic.StopMicrophone();
						source = oldSource;
					}
					isMicSourceNow = isMicSource;
				}
			}
			
			yield return new WaitForSeconds(intervalSec);
		}
	}
	
	public void ToogleMicSource(bool b) {
		isMicSource = b;
	}

	private void _ConvertRawSpectrumToBandLevels() {
		float coeff = Mathf.Log(rawSpectrum.Length);
		int offs = 0;
		for (int i = 0; i < bandLevels.Length; i++) {
			float next = Mathf.Exp(coeff / bandLevels.Length * (i + 1));
			float weight = 1.0f / (next - offs);
			float sum = 0.0f;
			for (; offs < next; offs++) {
				sum += rawSpectrum[offs];
			}
			bandLevels[i] = Mathf.Sqrt(weight * sum);
		}
	}
	
	public static VJManager GetDefaultManager() {
  		VJManager[] managers = UnityEngine.Object.FindObjectsOfType(typeof(VJManager)) as VJManager[];
  		if( managers.Length > 0 ) {
  			return managers[0];
  		}
  		return null;
  	}
	
	public VJDataSource GetDefaultDataSource() {
		VJDataSource[] sources = gameObject.GetComponents<VJDataSource>() as VJDataSource[];
		if( sources.Length > 0 ) {
			return sources[0];
		}
		return null;
	}
	

    private void _DrawGUIWindow(int windowID) {
		GUILayout.BeginVertical();

		VJDataSource[] sources = gameObject.GetComponents<VJDataSource>() as VJDataSource[];

		foreach(VJDataSource src in sources) {
			GUILayout.Label(src.sourceName + ":");
			GUILayout.Space(8.0f);
			Rect r = GUILayoutUtility.GetLastRect();
			//r.y+=16;
			r.height = 2;
			r.width = Mathf.Clamp(180.0f * src.current /VJDataSource.s_prog_max, 0.0f, 180.0f);
			Color lastColor = GUI.contentColor;
			GUI.color = Color.red;
			GUI.DrawTexture(r, _debugSpectrumTex, ScaleMode.StretchToFill, false, 0.0f);
			//GUILayout.Label(_debugSpectrumTex, GUIStyle.none, GUILayout.MinWidth(100.0f), GUILayout.MaxHeight(2.0f) );
			r.y+=2;
			r.width = Mathf.Clamp(180.0f * src.previous /VJDataSource.s_prog_max, 0.0f, 180.0f);
			GUI.color = Color.blue;
			GUI.DrawTexture(r, _debugSpectrumTex, ScaleMode.StretchToFill, false, 0.0f);
			r.y+=2;
			r.width = Mathf.Clamp(180.0f * src.diff /VJDataSource.s_prog_max, 0.0f, 180.0f);
			GUI.color = Color.yellow;
			GUI.DrawTexture(r, _debugSpectrumTex, ScaleMode.StretchToFill, false, 0.0f);
			GUI.color = lastColor;
			GUI.DragWindow ();
		}				
		GUILayout.EndVertical();
    }
	
	public void DrawGUI() {
		windowRect = GUILayout.Window(windowId, windowRect, _DrawGUIWindow, name, GUILayout.Width(200));
	}


	public void OnGUI() {
		mic.DrawGUI();
		if( debugWindow ) {
			DrawGUI();
		}
	}
}
