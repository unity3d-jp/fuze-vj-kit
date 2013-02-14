using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/VJ Manager")]
public class VJManager : MonoBehaviour {
	
	public static int numberOfBands = 8;

	public AudioSource 	source;
	public FFTWindow 	fft = FFTWindow.BlackmanHarris;
	public float[] bandLevels;
	
	[HideInInspector]
	public bool isMicSource = false;
	private bool isMicSourceNow = false;

	private VJMicrophone mic;
	private AudioSource oldSource;

	private int spectrumSamples = 1024;
	private float intervalSec = 0.02f;
	[HideInInspector]
	private float[] rawSpectrum;
	
	void Awake() {
		rawSpectrum = new float[spectrumSamples];
		bandLevels = new float[numberOfBands];
		mic = VJMicrophone.GetInstance();
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
}
