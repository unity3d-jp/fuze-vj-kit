//
// AudioJack - External Audio Analyzer for Unity
//
// Copyright (C) 2013 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class AudioJack : MonoBehaviour
{
    // Octave band type definition
    public enum BandType
    {
        FourBand,
        EightBand,
        TenBand,
        TwentySixBand
    }

    // Channel selection.
    public enum ChannelSelect
    {
        Discrete,
        MixStereo,
        MixAll
    }

    #region Configurations

    // FFT point number settings.
    static int[] fftPointNumberForBands = { 1024, 2048, 2048, 4096 };

    // Octave band frequency.
    static float[][] middleFrequenciesForBands = {
        new float[]{ 125.0f, 500, 1000, 2000 },
        new float[]{ 63.0f, 125, 500, 1000, 2000, 4000, 6000, 8000 },
        new float[]{ 31.5f, 63, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 },
		new float[]{ 20.0f, 25, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 }
    };

    // Octave band width.
    static float[] bandwidthForBands = {
        1.414f, // 2^(1/2)
        1.414f, // 2^(1/2)
        1.414f, // 2^(1/2)
        1.122f  // 2^(1/6)
    };
    
    #endregion

    #region Public variables and properties

    // Octave band type.
    public BandType bandType = BandType.TenBand;

	// debug
	public AudioSource internalSource;

    // Channel selection.
    public int channelToAnalyze;
    public ChannelSelect channelSelect = ChannelSelect.MixStereo;

	// Sampling: sampling length(for raw data)
	public float sampleLengthInSeconds = 0.1f;

    // Timekeeping.
    public float minimumInterval = 0.0f;

    // Internal audio mode.
    public bool internalMode = false;

	// Editor properties.
	public bool showLevels = false;
	public bool showSpectrum = false;

    // Octave band array (readonly).
    public float[] BandLevels {
        get { return bandLevels; }
    }

    // Channel level array (readonly).
    public float[] ChannelLevels {
        get { return channelLevels; }
    }

	public float currentChannelLevel {
		get { return channelLevels[Mathf.Clamp (channelToAnalyze,0, nChannels)]; }
	}

	public float[] FFTSpectrum {
		get { return fftSpectrum; }
	}

	public float[] Data {
		get { return rawData; }
 	}

    // Reference to the last created instance.
    static public AudioJack instance;

    #endregion
    
    #region Private variables and functions

    // Internal buffers.
	float[] rawData;
    float[] fftSpectrum;
    float[] bandLevels;
    float[] channelLevels;
	int nChannels;

    // Timekeeping.
    float timer;

    int FrequencyToFftIndex (float f, float sampleRate)
    {
        var points = fftSpectrum.Length;
        var index = Mathf.FloorToInt (f / sampleRate * 2.0f * points);
        return Mathf.Clamp (index, 0, points - 1);
    }

    #endregion

    #region Interface for the native part

#if UNITY_STANDALONE_OSX
    [DllImport ("AudioJackPlugin")]
    static extern int AudioJackCountChannels ();
    [DllImport ("AudioJackPlugin")]
    static extern float AudioJackGetSampleRate ();
    [DllImport ("AudioJackPlugin")]
    static extern float AudioJackGetChannelLevel (int channel);
    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackGetSpectrum (int channel, int mode, int pointNumber, float[] spectrum);
	[DllImport ("AudioJackPlugin")]
	public static extern void AudioJackGetRawData (int channel, float[] data, int samples);
#else
    static int AudioJackCountChannels ()
    {
        return 1;
    }

    static float AudioJackGetSampleRate ()
    {
        return 44100;
    }

    static float AudioJackGetChannelLevel (int channel)
    {
        return 0.0f;
    }

    static void AudioJackGetSpectrum (int channel, int mode, int pointNumber, float[] spectrum)
    {
        for (var i = 0; i < pointNumber; i++)
        {
            spectrum.elements [i] = 0.0f;
        }
    }
#endif

    #endregion

    #region MonoBehaviour functions

    void Awake ()
    {
        instance = this;

		nChannels = AudioJackCountChannels ();
        bandLevels = new float[middleFrequenciesForBands [(int)bandType].Length];
		channelLevels = new float[nChannels];
    }

    void Update ()
    {
        // Wait for the minimum interval.
        timer += Time.deltaTime;
        if (timer < minimumInterval)
            return;
        timer = 0.0f;

        // Reallocate the buffers if it needs.
        var fftNumber = fftPointNumberForBands [(int)bandType];
        if (fftSpectrum == null || fftSpectrum.Length * 2 != fftNumber)
        {
            fftSpectrum = new float[fftNumber / 2];
        }

        var bandCount = middleFrequenciesForBands [(int)bandType].Length;
        if (bandLevels == null || bandLevels.Length != bandCount)
        {
            bandLevels = new float[bandCount];
        }

		nChannels = AudioJackCountChannels ();
		if (channelLevels == null || channelLevels.Length != nChannels)
        {
			channelLevels = new float[nChannels];
        }
        
		// mute if internal source is disabled
		if(internalSource != null) {
			internalSource.mute = !internalMode;
		}
        
        // Do FFT.
        if (internalMode)
        {
			if(internalSource != null) {
				internalSource.GetSpectrumData(fftSpectrum, 0, FFTWindow.Blackman);
			} else {
            AudioListener.GetSpectrumData (fftSpectrum, 0, FFTWindow.Blackman);
        }
        }
        else
        {
			if(channelToAnalyze >= nChannels) {
				Debug.LogWarning("[AudioJack] tried to select nonexisting channel:" + channelToAnalyze + " max:" + nChannels);
				channelToAnalyze = Mathf.Clamp (channelToAnalyze, 0, nChannels-1);
			}
            AudioJackGetSpectrum (channelToAnalyze, (int)channelSelect, fftNumber, fftSpectrum);
        }
        
        // Convert the spectrum into octave bands.
        var sampleRate = internalMode ? AudioSettings.outputSampleRate : AudioJackGetSampleRate ();
        var frequencies = middleFrequenciesForBands [(int)bandType];
        var bandwidth = bandwidthForBands [(int)bandType];
		var sampleCount = Mathf.CeilToInt(sampleRate * sampleLengthInSeconds);

		if( rawData == null || rawData.Length != sampleCount ) {
			rawData = new float[sampleCount];
		}
		// get raw data
		if (internalMode)
		{
			if( internalSource != null ) {
				internalSource.GetOutputData(rawData, 0);
			} else {
				AudioListener.GetOutputData(rawData,0);
			}
		}
		else
		{
			AudioJackGetRawData (channelToAnalyze, rawData, sampleCount);
		}
		 
        
        for (var bi = 0; bi < bandCount; bi++)
        {
            // Specify the spectrum range of the band.
            int imin = FrequencyToFftIndex (frequencies [bi] / bandwidth, sampleRate);
            int imax = FrequencyToFftIndex (frequencies [bi] * bandwidth, sampleRate);
            
            // Specify the max level of the band.
            var bandMax = fftSpectrum [imin];
            for (var fi = imin + 1; fi < imax; fi++)
            {
                bandMax = Mathf.Max (bandMax, fftSpectrum [fi]);
            }

            if (internalMode)
            {
                // Convert amplitude to decibel.
                bandMax = 20.0f * Mathf.Log10 (bandMax * 2 + 1.5849e-13f);
            }

            // Store the result.
            bandLevels [bi] = bandMax;
        }

        // Read the channel levels.
		for (var i = 0; i < nChannels; i++)
        {
            channelLevels [i] = AudioJackGetChannelLevel (i);
        }
    }
    
    #endregion
}
