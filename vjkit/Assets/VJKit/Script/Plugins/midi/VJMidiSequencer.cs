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
using System.Collections.Generic;
using SmfLite;

[AddComponentMenu("VJKit/System/Midi Sequencer")]
[RequireComponent(typeof(AudioSource))]
public class VJMidiSequencer : MonoBehaviour
{
	public TextAsset midiFile;
	public float bpm = 131.0f;
	public float startTime;
	MidiFileContainer song;
	MidiTrackSequencer[] sequencers;
	
	void Start ()
	{
		song = MidiFileLoader.Load (midiFile.bytes);
		ResetAndPlay ();
	}
	
	void LateUpdate ()
	{
		if( sequencers != null ) {
			foreach(MidiTrackSequencer seq in sequencers) {
				if(seq.Playing) {
					VJMidiInput.ReceiveMidiEvents(seq.Advance (Time.deltaTime));
				}
			}
		}
	}
	
	void ResetAndPlay ()
	{
		audio.Play ();
		sequencers = new MidiTrackSequencer[song.tracks.Count];
		for(int i = 0; i < song.tracks.Count; ++i) {
			sequencers[i] = new MidiTrackSequencer (song.tracks [i], song.division, bpm);
			VJMidiInput.ReceiveMidiEvents(sequencers[i].Start (startTime));
		}
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (0, 0, 300, 50), "Reset")) {
			ResetAndPlay ();
		}
	}
}

