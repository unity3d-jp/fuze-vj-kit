using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/VJ Manager")]
public class VJManager : VJAbstractManager {
	
	public static int numberOfBands = 8;
	public static float analysisWindowSec = 0.2f;
	
	public AudioSource 	source;
	public FFTWindow 	fft = FFTWindow.BlackmanHarris;
	public float[] bandLevels;

	public float rawVolume;

	[HideInInspector]
	public bool isMicSource = false;
	private bool isMicSourceNow = false;

	private VJMicrophone mic;
	private AudioSource oldSource;

	private int spectrumSamples = 1024;
	private float intervalSec = 0.02f;
	[HideInInspector]
	public float[] rawSpectrum;

	[HideInInspector]
	public float[] rawData; // ch.0

	private int nSamples;

	public override void Awake() {
		base.Awake();

		rawSpectrum = new float[spectrumSamples];
		bandLevels = new float[numberOfBands];
		mic = VJMicrophone.GetInstance();
		rawData = null;
	}

	IEnumerator Start() {
		
		while (true) {
			if( source && source.clip ) {
				AudioClip ac = source.clip;
			
				if( rawData == null ) {
					nSamples = (int) (((float)ac.frequency * analysisWindowSec) * ac.channels);
					rawData = new float[nSamples];
				}

				if( rawData != null ) {
					ac.GetData(rawData, source.timeSamples);
				}
				source.GetSpectrumData(rawSpectrum, 0, FFTWindow.BlackmanHarris);
				_ConvertOutputDataToVolumeLevels();
				_ConvertRawSpectrumToBandLevels();
			}
			if( isMicSourceNow != isMicSource ) {
				if(mic) {
					if(isMicSource) {
						oldSource = source;
						source = mic.StartMicrophone();
						rawData = null;
						rawVolume = 0.0f;
					} else {
						mic.StopMicrophone();
						source = oldSource;
						rawData = null;
						rawVolume = 0.0f;
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

	private void _ConvertOutputDataToVolumeLevels() {

		if( rawData != null ) {
			float sum = 0.0f;
			for (int i = 0; i < rawData.Length; i++) {
				sum += Mathf.Abs(rawData[i]);
			}
			rawVolume = sum / (float)nSamples;
		}
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

	public override void OnGUI() {
		base.OnGUI();
		mic.DrawGUI();
	}
}
