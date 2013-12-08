//
// MidiInput.cs - MIDI input manager
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
using System.Collections.Generic;

public enum MidiChannel
{
    Ch1,    // 0
    Ch2,    // 1
    Ch3,
    Ch4,
    Ch5,
    Ch6,
    Ch7,
    Ch8,
    Ch9,
    Ch10,
    Ch11,
    Ch12,
    Ch13,
    Ch14,
    Ch15,
    Ch16,
    All     // 16
}

public class MidiInput : MonoBehaviour
{
    #region Public interface

    // Knob filter coefficient.
    public static float knobSensibility = 0.0f;

    // Returns the key state (on: velocity, off: zero).
    public static float GetKey (MidiChannel channel, int noteNumber)
    {
        var v = instance.channelArray [(int)channel].noteArray [noteNumber];
        if (v > 1.0f) {
            return v - 1.0f;
        } else if (v > 0.0) {
            return v;
        } else {
            return 0.0f;
        }
    }

    public static float GetKey (int noteNumber)
    {
        return GetKey (MidiChannel.All, noteNumber);
    }

    // Returns true if the key was pressed down in the current frame.
    public static bool GetKeyDown (MidiChannel channel, int noteNumber)
    {
        return instance.channelArray [(int)channel].noteArray [noteNumber] > 1.0f;
    }

    public static bool GetKeyDown (int noteNumber)
    {
        return GetKeyDown (MidiChannel.All, noteNumber);
    }

    // Returns true if the key was released in the current frame.
    public static bool GetKeyUp (MidiChannel channel, int noteNumber)
    {
        return instance.channelArray [(int)channel].noteArray [noteNumber] < 0.0f;
    }

    public static bool GetKeyUp (int noteNumber)
    {
        return GetKeyUp (MidiChannel.All, noteNumber);
    }

    // Provides the CC (knob) list.
    public static int[] GetKnobNumbers (MidiChannel channel)
    {
        var cs = instance.channelArray [(int)channel];
        var numbers = new int[cs.knobMap.Count];
        cs.knobMap.Keys.CopyTo (numbers, 0);
        return numbers;
    }

    public static int[] GetKnobNumbers ()
    {
        return GetKnobNumbers (MidiChannel.All);
    }

    // Get the CC (knob) value.
    public static float GetKnob (MidiChannel channel, int knobNumber)
    {
        var cs = instance.channelArray [(int)channel];
        if (cs.knobMap.ContainsKey (knobNumber)) {
            return cs.knobMap [knobNumber].filteredValue;
        } else {
            return 0.0f;
        }
    }

	#region vjkit extension
	// Get the CC (knob) value.
	public static bool DoesKnobExist (MidiChannel channel, int knobNumber)
	{
		var cs = instance.channelArray [(int)channel];
		return (cs.knobMap.ContainsKey (knobNumber));
	}

	public static float GetNotePitch(MidiChannel channel)
	{
		var cs = instance.channelArray [(int)channel];
		if( cs.lastNote < 0.0f || cs.noteOnCount <= 0 ) {
			return 0.0f;
		} else {
			return ( cs.lastNote * 1.0f / 127.0f );
		}
	}
	public static bool IsNotePitchGiven(MidiChannel channel)
	{
		var cs = instance.channelArray [(int)channel];
		return cs.lastNote >= 0.0f;
	}
	#endregion


    public static float GetKnob (int knobNumber)
    {
        return GetKnob (MidiChannel.All, knobNumber);
    }

    #endregion

    #region Internal data structure

    // CC (knob) state.
    protected class KnobState
    {
		public float realtimeValue;
        public float filteredValue;

        public KnobState (float initial)
        {
			realtimeValue = filteredValue = initial;
        }

        public void Update (float value)
        {
			realtimeValue = value;
        }

        public void UpdateFilter (float filterCoeff)
        {
			if (filterCoeff == 0.0f) {
				filteredValue = realtimeValue;
			} else {
				filteredValue = realtimeValue - (realtimeValue - filteredValue) * filterCoeff;
			}
        }
    }

    // Channel State.
    protected class ChannelState
    {
        // Note state array.
        // X<0    : Released on this frame.
        // X=0    : Off.
        // 0<X<=1 : On. X represents velocity.
        // 1<X<=2 : Triggered on this frame. (X-1) represents velocity.
        public float[] noteArray;
		public float lastNote;
		public int noteOnCount;
        
        // Knob number to knob mapping.
        public Dictionary<int, KnobState> knobMap;

        public ChannelState ()
        {
            noteArray = new float[128];
            knobMap = new Dictionary<int, KnobState> ();
			lastNote = -1.0f;
			noteOnCount = 0;
        }
    }

    // Channel state array.
    protected ChannelState[] channelArray;

    #endregion
    
    #region Monobehaviour functions

	void Initialize() {
		if( channelArray == null ) {
        channelArray = new ChannelState[17];
        for (var i = 0; i < 17; i++) {
            channelArray [i] = new ChannelState ();
        }
    }
	}

    void Awake ()
    {
		Initialize();
    }

    void Update ()
    {
        // Update the note state array.
        foreach (var cs in channelArray) {
            for (var i = 0; i < 128; i++) {
                var x = cs.noteArray [i];
                if (x > 1.0f) {
                    // Key down -> Hold.
                    cs.noteArray [i] = x - 1.0f;
					cs.lastNote = i;
					cs.noteOnCount+= 1;
                } else if (x < 0) {
                    // Key up -> Off.
                    cs.noteArray [i] = 0.0f;
					cs.lastNote = i;
					cs.noteOnCount-= 1;
				}
            }
        }

        // Calculate the filter coefficient.
        var filterCoeff = (knobSensibility > 0.0f) ? Mathf.Exp (-knobSensibility * Time.deltaTime) : 0.0f;

        // Update the filtered value.
        foreach (var cs in channelArray) {
            foreach (var k in cs.knobMap.Values) {
                k.UpdateFilter (filterCoeff);
            }
        }

        // Process the message queue.
        while (MidiBridge.instance.incomingMessageQueue.Count > 0) {
            // Pop from the queue.
            var message = MidiBridge.instance.incomingMessageQueue.Dequeue ();

            // Split the first byte.
            var statusCode = message.status >> 4;
            var channelNumber = message.status & 0xf;

            // Note on message?
            if (statusCode == 9) {
                var velocity = 1.0f / 127 * message.data2 + 1.0f;
                channelArray [channelNumber].noteArray [message.data1] = velocity;
                channelArray [(int)MidiChannel.All].noteArray [message.data1] = velocity;
            }

            // Note off message?
            if (statusCode == 8 || (statusCode == 9 && message.data2 == 0)) {
                channelArray [channelNumber].noteArray [message.data1] = -1.0f;
                channelArray [(int)MidiChannel.All].noteArray [message.data1] = -1.0f;
            }

            // CC message?
            if (statusCode == 0xb) {
                // Normalize the value.
                var value = 1.0f / 127 * message.data2;

                // Update the channel if it already exists, or add a new channel.
                if (channelArray [channelNumber].knobMap.ContainsKey (message.data1)) {
                    channelArray [channelNumber].knobMap [message.data1].Update (value);
                } else {
                    channelArray [channelNumber].knobMap [message.data1] = new KnobState (value);
                }

                // Do again for All-ch.
                if (channelArray [(int)MidiChannel.All].knobMap.ContainsKey (message.data1)) {
                    channelArray [(int)MidiChannel.All].knobMap [message.data1].Update (value);
                } else {
                    channelArray [(int)MidiChannel.All].knobMap [message.data1] = new KnobState (value);
                }
            }
        }
    }

    #endregion

    #region Singleton class handling

	static VJMidiInput _instance;

    public static VJMidiInput instance {
        get {
            if (_instance == null) {
                var previous = FindObjectOfType (typeof(MidiInput));
                if (previous) {
                    Debug.LogWarning ("Initialized twice. Don't use MidiInput in the scene hierarchy.");
					_instance = (VJMidiInput)previous;
                } else {
                    var go = new GameObject ("__MidiInput");
                    _instance = go.AddComponent<VJMidiInput> ();
					_instance.Initialize();
                    DontDestroyOnLoad (go);
                    //go.hideFlags = HideFlags.HideInHierarchy;
                }
            }
            return _instance;
        }
    }

    #endregion
}
