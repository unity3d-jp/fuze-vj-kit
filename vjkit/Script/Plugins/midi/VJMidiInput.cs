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

public class VJMidiInput : MidiInput
{
	// Debug Window	properties
	protected static int windowIDSeed = 10000;
	protected Rect windowRect = new Rect(160, 20, 120, 50);
	protected int windowId;
	public bool debugWindow = true;
	public int nMsgs = 16;

	private Queue<MidiBridge.Message> m_midimsgs;

	public virtual void Awake() {
		windowRect.x = PlayerPrefs.GetFloat("vjkit.window.pos." + gameObject.name + ".x", Screen.width - 220.0f);
		windowRect.y = PlayerPrefs.GetFloat("vjkit.window.pos." + gameObject.name + ".y", 20.0f);
		debugWindow  = 1 == PlayerPrefs.GetInt("vjkit.window." + gameObject.name + ".debug", 1);
		windowRect.width = 200;
		windowId = windowIDSeed;
		windowIDSeed++;
		m_midimsgs = new Queue<MidiBridge.Message>();
	}
	
	protected virtual void OnDrawGUIWindow(int windowID) {
		GUILayout.BeginVertical();
		foreach(MidiBridge.Message m in m_midimsgs) {
			Color lastColor = GUI.color;
			GUI.color = Color.white;

			var statusCode = m.status >> 4;
			var channelNumber = m.status & 0xf;
			
			// Note on message?
			if (statusCode == 9) {
				var velocity = 1.0f / 127 * m.data2;
				GUILayout.Label (string.Format ("CH:{0}| NOTE ON {1} {2}", channelNumber+1, m.data1, velocity));
			}
			
			// Note off message?
			if (statusCode == 8 || (statusCode == 9 && m.data2 == 0)) {
				GUILayout.Label (string.Format ("CH:{0}| NOTE OFF {1}", channelNumber+1, m.data1));
			}
			
			// CC message?
			if (statusCode == 0xb) {
				// Normalize the value.
				var value = 1.0f / 127 * m.data2;
				GUILayout.Label (string.Format ("CH:{0}| CC {1} {2}", channelNumber+1, m.data1, value));
			}

			GUI.color = lastColor;
		}				
		GUI.DragWindow ();
		GUILayout.EndVertical();
	}
	
	public virtual void OnGUI() {
		if( debugWindow ) {
			windowRect = GUILayout.Window(windowId, windowRect, OnDrawGUIWindow, name, GUILayout.Width(200));
		}
	}
	
	public virtual void OnApplicationQuit() {
		PlayerPrefs.SetFloat("vjkit.window.pos." + gameObject.name + ".x", windowRect.x);
		PlayerPrefs.SetFloat("vjkit.window.pos." + gameObject.name + ".y", windowRect.y);
		PlayerPrefs.SetInt("vjkit.window." + gameObject.name + ".debug", (debugWindow? 1:0));
	}


	public static void ReceiveMidiEvents (List<SmfLite.MidiEvent> events) {
		instance._ReceiveMidiEvents(events);
	}

	/*
	 * Getting midi signals from outside and process it
	 */ 
	private void _ReceiveMidiEvents (List<SmfLite.MidiEvent> events) {
		if( events == null ) {
			return;
		}
		while( m_midimsgs.Count > nMsgs ) {
			m_midimsgs.Dequeue();
		}

		foreach(SmfLite.MidiEvent e in events ) {
			// Parse the message.
			var message = new MidiBridge.Message (e.status, e.data1, e.data2);			
			m_midimsgs.Enqueue(message);

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
}