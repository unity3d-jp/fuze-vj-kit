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

[AddComponentMenu("VJKit/System/VJ Manager(Native FFT-Audio Jack)")]
[RequireComponent(typeof(AudioJack))]
public class VJAudioJackManager : VJAbstractManager {

	private AudioJack m_audioJack;

	public int numberOfBands {
		get {
			switch(m_audioJack.bandType) {
			case AudioJack.BandType.FourBand:
				return 4;
			case AudioJack.BandType.EightBand:
				return 8;
			case AudioJack.BandType.TenBand:
				return 10;
			case AudioJack.BandType.TwentySixBand:
				return 26;
			}
			return 0;
		}
	}

	public float rawVolume {
		get {
			return m_audioJack.currentChannelLevel;
		}
	}
	public float[] bandLevels {
		get {
			return m_audioJack.BandLevels;
		}
	}
	public float[] rawData {
		get {
			return m_audioJack.Data;
		}
	}

	public override void Awake() {
		m_audioJack = GetComponent<AudioJack>();
	}

		
	public override void OnGUI() {
		base.OnGUI();
	}
}
