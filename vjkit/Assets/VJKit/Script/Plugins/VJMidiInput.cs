/*
fuZe vjkit

Copyright (C) 2013 Unity Technologies Japan, G.K.

Permission is hereby granted, free of charge, to any 
person obtaining a copy of this software and associated 
documentation files (the "Software"), to deal in the 
Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, 
sublicense, and/or sell copies of the Software, and to permit 
persons to whom the Software is furnished to do so, subject 
to the following conditions: The above copyright notice and 
this permission notice shall be included in all copies or 
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
OTHER DEALINGS IN THE SOFTWARE.

Special Thanks:
The original software design and architecture of fuZe vjkit 
is inspired by Visualizer Studio, created by Altered Reality 
Entertainment LLC.

VJMidiInput.cs
based on MidiInput.cs
	Unity MIDI Input plug-in / C# interface
	By Keijiro Takahashi, 2013
	https://github.com/keijiro/unity-midi-bridge

Unity MIDI Input plug-in 
License:
Copyright (C) 2013 Keijiro Takahashi

Permission is hereby granted, free of charge, to any person obtaining a 
copy of this software and associated documentation files (the "Software"), 
to deal in the Software without restriction, including without limitation 
the rights to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the Software, and to permit persons to whom the 
Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included 
in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
IN THE SOFTWARE.
  */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VJMidiInput : MonoBehaviour
{
	#region Public interface
	// Filter mode IDs.
	public enum Filter
	{
		Realtime,
		Fast,
		Slow
	}
	
	// Returns the key state (on: velocity, off: zero).
	public static float GetKey (int channel, int noteNumber)
	{
		var v = instance.channels[channel].notes [noteNumber];
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
		return GetKey (16, noteNumber);
	}
	
	// Returns true if the key was pressed down in the current frame.
	public static bool GetKeyDown (int channel, int noteNumber)
	{
		return instance.channels[channel].notes [noteNumber] > 1.0f;
	}
	public static bool GetKeyDown (int noteNumber)
	{
		return GetKeyDown (16, noteNumber);
	}
	
	// Returns true if the key was released in the current frame.
	public static bool GetKeyUp (int channel, int noteNumber)
	{
		return instance.channels[channel].notes [noteNumber] < 0.0f;
	}
	public static bool GetKeyUp (int noteNumber)
	{
		return GetKeyUp (16, noteNumber);
	}
	
	// Provides the knob list.
	public static int[] GetKnobNumbers(int channel)
	{
		var ch = instance.channels [channel];
		var numbers = new int[ch.knobs.Count];
		ch.knobs.Keys.CopyTo (numbers, 0);
		return numbers;
	}
	public static int[] GetKnobNumbers()
	{
		return GetKnobNumbers (16);
	}

	public static bool DoesKnobExist(int channel, int knobnumber)
	{
		var ch = instance.channels [channel];
		return ch.DoesKnobExist(knobnumber);
	}
	
	public static bool IsNotePitchGiven(int channel)
	{
		var ch = instance.channels [channel];
		return ch.IsNotePitchGiven();
	}

	public static float GetNotePitch(int channel)
	{
		var ch = instance.channels [channel];
		return ch.lastNotePitch;
	}

	// Get the CC (knob) value.
	public static float GetKnob (int channel, int knobNumber, Filter filter = Filter.Realtime)
	{
		var ch = instance.channels [channel];
		if (ch.knobs.ContainsKey (knobNumber)) {
			return ch.knobs [knobNumber].filteredValues [(int)filter];
		} else {
			return 0.0f;
		}
	}
	public static float GetKnob (int knobNumber, Filter filter = Filter.Realtime)
	{
		return GetKnob (16, knobNumber, filter);
	}


	// Returns true if the key was released in this frame.
	public static void ReceiveMidiEvents (List<SmfLite.MidiEvent> events)
	{
		instance._ReceiveMidiEvents (events);
	}
	#endregion
	
	#region Internal data structure
	// CC (knob) state.
	class Knob
	{
		public float[] filteredValues;
		
		public Knob (float initial)
		{
			filteredValues = new float[3];
			filteredValues [0] = filteredValues [1] = filteredValues [2] = initial;
		}
		
		public void Update (float value)
		{
			filteredValues [0] = value;
		}
		
		public void UpdateFilter (float fastFilterCoeff, float slowFilterCoeff)
		{
			filteredValues [1] = filteredValues [0] - (filteredValues [0] - filteredValues [1]) * fastFilterCoeff;
			filteredValues [2] = filteredValues [0] - (filteredValues [0] - filteredValues [2]) * slowFilterCoeff;
		}
	}
	
	// Channel status.
	class Channel
	{
		// Note status.
		// X<0    : Released on this frame.
		// X=0    : Off.
		// 0<X<=1 : On. X represents velocity.
		// 1<X<=2 : Triggered on this frame. (X-1) represents velocity.
		public float[] 	notes;
		public float	lastNotePitch;
		
		// Knob number to knob mapping.
		public Dictionary<int, Knob> knobs;
		
		public Channel ()
		{
			notes = new float[128];
			knobs = new Dictionary<int, Knob> ();
			lastNotePitch = -1.0f;
		}

		// Returns true if the key was pressed down in this frame.
		public float GetNotePitch ()
		{
			float nh = lastNotePitch;
			if( nh > 0.0f ) {
				return nh;
			} else {
				return 0.0f;
			}
		}
		
		public bool IsNotePitchGiven ()
		{
			return lastNotePitch >= 0.0f;
		}
		
		public bool DoesKnobExist (int knobnumber)
		{
			return knobs.ContainsKey (knobnumber);
		}
	}
	
	// Channel list (0ch-15ch and 16=All)
	Channel[] channels;
	#endregion
	
	#region Public properties
	public float sensibilityFast = 20.0f;
	public float sensibilitySlow = 5.0f;
	#endregion
	
	#region Monobehaviour functions
	void Awake ()
	{
		channels = new Channel[17];
		for (var i = 0; i < 17; i++) {
			channels[i] = new Channel();
		}
	}
	
	void Update ()
	{
		// Update the note state array.
		foreach (var ch in channels) {
			for (var i = 0; i < 128; i++) {
				var x = ch.notes [i];
				if (x > 1.0f) {
					// Key down -> Hold.
					ch.notes [i] = x - 1.0f;
				} else if (x < 0) {
					// Key up -> Off.
					ch.notes [i] = 0.0f;
				}
			}
		}
		
		// Calculate the filter coefficients.
		var fastFilterCoeff = Mathf.Exp (-sensibilityFast * Time.deltaTime);
		var slowFilterCoeff = Mathf.Exp (-sensibilitySlow * Time.deltaTime);
		
		// Update the filtered value.
		foreach (var ch in channels) {
			foreach (var k in ch.knobs.Values) {
				k.UpdateFilter (fastFilterCoeff, slowFilterCoeff);
			}
		}
		
		// Process the message queue.
		while (MidiBridge.instance.incomingMessageQueue.Count > 0) {
			// Pop from the queue.
			var message = MidiBridge.instance.incomingMessageQueue.Dequeue ();
			
			// Split the first byte.
			var b1hi = message.status >> 4;
			var b1lo = message.status & 0xf;
			
			// Note on message?
			if (b1hi == 9) {
				var velocity = 1.0f / 127 * message.data2 + 1.0f;
				channels [b1lo].notes [message.data1] = velocity;
				channels [16].notes [message.data1] = velocity;
			}
			
			// Note off message?
			if (b1hi == 8 || (b1hi == 9 && message.data2 == 0)) {
				channels [b1lo].notes [message.data1] = -1.0f;
				channels [16].notes [message.data1] = -1.0f;
			}
			
			// CC message?
			if (b1hi == 0xb) {
				// Normalize the value.
				var value = 1.0f / 127 * message.data2;
				
				// Update the channel if it already exists, or add a new channel.
				if (channels [b1lo].knobs.ContainsKey (message.data1)) {
					channels [b1lo].knobs [message.data1].Update (value);
				} else {
					channels [b1lo].knobs [message.data1] = new Knob (value);
				}
				
				// Do again for All-ch.
				if (channels [16].knobs.ContainsKey (message.data1)) {
					channels [16].knobs [message.data1].Update (value);
				} else {
					channels [16].knobs [message.data1] = new Knob (value);
				}
			}
		}
	}

	/*
	 * Getting midi signals from outside and process it
	 */ 
	private void _ReceiveMidiEvents (List<SmfLite.MidiEvent> events) {
		if( events == null ) {
			return;
		}

		foreach(SmfLite.MidiEvent e in events ) {
			// Parse the message.
			var message = new MidiMessage (e.status, e.data1, e.data2);
			
			// Split the first byte.
			var b1hi = message.status >> 4;
			var b1lo = message.status & 0xf;
			
			// Note on message?
			if (b1hi == 9) {
				var velocity = 1.0f / 127 * message.data2 + 1.0f;
				channels [b1lo].notes [message.data1] = velocity;
				channels [16].notes [message.data1] = velocity;
			}
			
			// Note off message?
			if (b1hi == 8 || (b1hi == 9 && message.data2 == 0)) {
				channels [b1lo].notes [message.data1] = -1.0f;
				channels [16].notes [message.data1] = -1.0f;
			}
			
			// CC message?
			if (b1hi == 0xb) {
				// Normalize the value.
				var value = 1.0f / 127 * message.data2;
				
				// Update the channel if it already exists, or add a new channel.
				if (channels [b1lo].knobs.ContainsKey (message.data1)) {
					channels [b1lo].knobs [message.data1].Update (value);
				} else {
					channels [b1lo].knobs [message.data1] = new Knob (value);
				}
				
				// Do again for All-ch.
				if (channels [16].knobs.ContainsKey (message.data1)) {
					channels [16].knobs [message.data1].Update (value);
				} else {
					channels [16].knobs [message.data1] = new Knob (value);
				}
			}
		}
	}
	#endregion
	
	#region Singleton class interface
	static VJMidiInput _instance;
	
	public static VJMidiInput instance {
		get {
			if (_instance == null) {
				var go = new GameObject ("__VJMidiInput");
				_instance = go.AddComponent<VJMidiInput> ();
				DontDestroyOnLoad (go);
				go.hideFlags = HideFlags.HideInHierarchy;
			}
			return _instance;
		}
	}
	#endregion
}
//
//
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.InteropServices;
//
//public class VJMidiInput : MonoBehaviour
//{
//    #region Static interface
//    // Common instance.
//	private static VJMidiInput s_instance;
//
//	public static VJMidiInput GetInstance() {		
//		if( !s_instance ) {
//			GameObject go = new GameObject();
//			go.name = "VJMidi";
//			DontDestroyOnLoad(go);
//			s_instance = go.AddComponent<VJMidiInput>();
//		}		
//		return s_instance;
//	}
//	
//	// Filter mode IDs.
//	public enum Filter
//	{
//		Realtime,
//		Fast,
//		Slow
//	}
//
//	// Returns the key state (on: velocity, off: zero).
//	public static float GetKey (int noteNumber)
//	{
//		var v = GetInstance().notes [noteNumber];
//		if (v > 1.0f) {
//			return v - 1.0f;
//		} else if (v > 0.0f) {
//			return v;
//		} else {
//			return 0.0f;
//		}
//	}
//	
//	// Returns true if the key was pressed down in this frame.
//	public static bool GetKeyDown (int noteNumber)
//	{
//		return GetInstance().notes [noteNumber] > 1.0f;
//	}
//	
//	// Returns true if the key was released in this frame.
//	public static bool GetKeyUp (int noteNumber)
//	{
//		return GetInstance().notes [noteNumber] < 0.0f;
//	}
//
//	// Returns true if the key was released in this frame.
//	public static void ReceiveMidiEvents (List<SmfLite.MidiEvent> events)
//	{
//		GetInstance()._ReceiveMidiEvents (events);
//	}
//
//	// Provides the channel list.
//	public static int[] KnobChannels {
//		get {
//			int[] channels = new int[GetInstance().knobs.Count];
//			GetInstance().knobs.Keys.CopyTo (channels, 0);
//			return channels;
//		}
//	}
//	
//	// Get the CC value.
//	public static float GetKnob (int channel, Filter filter = Filter.Realtime)
//	{
//		if (GetInstance().knobs.ContainsKey (channel)) {
//			return GetInstance().knobs [channel].filteredValues [(int)filter];
//		} else {
//			return 0.0f;
//		}
//	}
//
//	// Returns true if the key was pressed down in this frame.
//	public static float GetNotePitch ()
//	{
//		float nh = GetInstance().lastNotePitch;
//		if( nh > 0.0f ) {
//			return nh;
//		} else {
//			return 0.0f;
//		}
//	}
//
//	public static bool IsNotePitchGiven ()
//	{
//		return GetInstance().lastNotePitch >= 0.0f;
//	}
//
//	public static bool DoesKnobExist (int channel)
//	{
//		return GetInstance().knobs.ContainsKey (channel);
//	}
//	#endregion
//	
//	#region MIDI message sturcture
//	public struct MidiMessage
//	{
//		// MIDI source (endpoint) ID.
//		public uint source;
//		
//		// MIDI status byte.
//		public byte status;
//		
//		// MIDI data bytes.
//		public byte data1;
//		public byte data2;
//
//		public MidiMessage (SmfLite.MidiEvent e) {
//			source = 0;
//			status = e.status;
//			data1 = e.data1;
//			data2 = e.data2;
//		}
//
//		public MidiMessage (ulong data)
//		{
//			source = (uint)(data & 0xffffffffUL);
//			status = (byte)((data >> 32) & 0xff);
//			data1 = (byte)((data >> 40) & 0xff);
//			data2 = (byte)((data >> 48) & 0xff);
//		}
//		
//		public override string ToString ()
//		{
//			return string.Format ("s({0:X2}) d({1},{2}) from {3:X8}", status, data1, data2, source);
//		}
//	}
//	#endregion
//	
//	#region Internal data structure
//	// CC channel (knob) information.
//	class Knob
//	{
//		public float[] filteredValues;
//		
//		public Knob (float initial)
//		{
//			filteredValues = new float[3];
//			filteredValues [0] = filteredValues [1] = filteredValues [2] = initial;
//		}
//		
//		public void Update (float value)
//		{
//			filteredValues [0] = value;
//		}
//		
//		public void UpdateFilter (float fastFilterCoeff, float slowFilterCoeff)
//		{
//			filteredValues [1] = filteredValues [0] - (filteredValues [0] - filteredValues [1]) * fastFilterCoeff;
//			filteredValues [2] = filteredValues [0] - (filteredValues [0] - filteredValues [2]) * slowFilterCoeff;
//		}
//	}
//	
//	// Note state array.
//	// X<0    : Released on this frame.
//	// X=0    : Off.
//	// 0<X<=1 : On. X represents velocity.
//	// 1<X<=2 : Triggered on this frame. (X-1) represents velocity.
//	float[] notes;
//	float	lastNotePitch;
////	List<VJMidiSequencer> sequencers;
//	
//	// Channel number to knob mapping.
//	Dictionary<int, Knob> knobs;
//	#endregion
//	
//	#region Editor supports
//	#if UNITY_EDITOR
//	// Incoming message history.
//	Queue<MidiMessage> messageHistory;
//	public Queue<MidiMessage> History {
//		get { return messageHistory; }
//	}
//	#endif
//	#endregion
//	
//	#region Public properties
//	public float sensibilityFast = 20.0f;
//	public float sensibilitySlow = 8.0f;
//	#endregion
//	
//	#region Monobehaviour functions
//	void Awake ()
//	{
//		s_instance = this;
//		notes = new float[128];
//		knobs = new Dictionary<int, Knob> ();
//		lastNotePitch = -1.0f;
//		#if UNITY_EDITOR
//		messageHistory = new Queue<MidiMessage> ();
//		#endif
//	}
//	
//	void Update ()
//	{
//		// Update the note state array.
//		for (var i = 0; i < 128; i++) {
//			var x = notes [i];
//			if (x > 1.0f) {
//				// Key down -> Hold.
//				notes [i] = x - 1.0f;
//			} else if (x < 0) {
//				// Key up -> Off.
//				notes [i] = 0.0f;
//			}
//		}
//		
//		// Calculate the filter coefficients.
//		var fastFilterCoeff = Mathf.Exp (-sensibilityFast * Time.deltaTime);
//		var slowFilterCoeff = Mathf.Exp (-sensibilitySlow * Time.deltaTime);
//		
//		// Update the filtered value.
//		foreach (var k in knobs.Values) {
//			k.UpdateFilter (fastFilterCoeff, slowFilterCoeff);
//		}
//		
//		// Process the message queue.
//		while (true) {
//			// Pop from the queue.
//			var data = DequeueIncomingData ();
//			if (data == 0) {
//				break;
//			}
//			
//			// Parse the message.
//			var message = new MidiMessage (data);
//			
//			// Note on message?
//			if (message.status == 0x90) {
//				notes [message.data1] = 1.0f / 127 * message.data2 + 1.0f;
//				lastNotePitch = 1.0f / 127 * message.data1;
//			}
//			
//			// Note off message?
//			if (message.status == 0x80) {
//				notes [message.data1] = -1.0f;
//				lastNotePitch = -1.0f;
//			}
//			
//			// CC message?
//			if (message.status == 0xb0) {
//				// Normalize the value.
//				var value = 1.0f / 127 * message.data2;
//				
//				// Update the channel if it already exists, or add a new channel.
//				if (knobs.ContainsKey (message.data1)) {
//					knobs [message.data1].Update (value);
//				} else {
//					knobs [message.data1] = new Knob (value);
//				}
//			}
//			
//			#if UNITY_EDITOR
//			// Record the message history.
//			messageHistory.Enqueue (message);
//			#endif		
//		}
//		
//		#if UNITY_EDITOR
//		// Truncate the history.
//		while (messageHistory.Count > 8) {
//			messageHistory.Dequeue ();
//		}
//		#endif
//	}
//
//}
