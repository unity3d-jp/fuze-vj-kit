using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Midi/Midi Note Pitch Source")]
[RequireComponent (typeof (VJMidiManager))]
public class VJMidiNotePitchDataSource : VJAbstractDataSource {

	[Range(0,127)]
	public int baseNote;

	// Use this for initialization
	public void Awake() {
	}

	// Update is called once per frame
	public void Update () {
		float raw_current = 0.0f;
		if(VJMidiInput.IsNotePitchGiven()) {
			raw_current = VJMidiInput.GetNotePitch() - (1.0f/127 * baseNote);
		}
		raw_current = raw_current * boost;

		previous = current;
		current = raw_current;
		diff = current - previous;
	}

}
