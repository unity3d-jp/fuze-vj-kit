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

[AddComponentMenu("VJKit/MIDI Source/Midi Knob Source")]
[RequireComponent (typeof (VJMidiManager))]
public class VJMidiKnobDataSource : VJAbstractDataSource {

	private VJMidiManager m_manager;

	[Range(0, 127)]
	public int knobNumber;

	[Range(-1.0f, 0.0f)]
	public float middleOffset;

	[Range(0.0f, 1.0f)]
	public float initialValue;

	public bool digital;
	public bool inverse;

	// Use this for initialization
	public void Awake() {
		m_manager = GetComponent<VJMidiManager>();
	}

	// Update is called once per frame
	public void Update () {
		float raw_current = 0.0f;

		if( VJMidiInput.DoesKnobExist(m_manager.midiChannel, knobNumber) ) {
			raw_current = VJMidiInput.GetKnob(m_manager.midiChannel, knobNumber) + middleOffset;

			if(inverse) {
				raw_current = 1.0f - raw_current;
			}
		} else {
			raw_current = initialValue;
		}
			
		if(digital) {
			raw_current = Mathf.Floor(raw_current);
		}
			
		raw_current = raw_current * boost;

		previous = current;
		current = raw_current;
		diff = current - previous;
	}

	//----------------------------------------------

//	public static int LearntChannel {
//		get { return instance.learnt; }
//	}
//	
//	public static float GetController (int channel)
//	{
//		if (instance.controllers.ContainsKey (channel)) {
//			return instance.controllers [channel];
//		} else {
//			return -1.0f;
//		}
//	}
//	
//	public static void StartLearn ()
//	{
//		instance.learnt = -1;
//		instance.toLearn = true;
//	}
//	
//	void Awake ()
//	{
//		instance = this;
//		learnt = -1;
//	}
//	
//	void Start ()
//	{
//		receiver = FindObjectOfType (typeof(MidiReceiver)) as MidiReceiver;
//		controllers = new Dictionary<int, float> ();
//	}
//	
//	void Update ()
//	{
//		while (!receiver.IsEmpty) {
//			var message = receiver.PopMessage ();
//			if (message.status == 0xb0) {
//				controllers [message.data1] = 1.0f / 127 * message.data2;
//				if (toLearn) {
//					learnt = message.data1;
//					toLearn = false;
//				}
//			}
//		}
//	}
}

/*

        while (true) {
            MidiInput.StartLearn ();
            while (MidiInput.LearntChannel < 0) {
                yield return null;
            }
            if (MidiInput.LearntChannel != rackets [0].channel) {
                break;
            }
            yield return null;
        }

        rackets [1].channel = MidiInput.LearntChannel;
        rackets [1].gameObject.SetActive (true);



 */
