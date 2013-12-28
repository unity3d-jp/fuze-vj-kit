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
public class VJMidiSequencer : MonoBehaviour
{
	public TextAsset[] midiFiles;
	public AudioClip[] audioFiles;
	public float bpm = 131.0f;
	public float startTime;
	public bool receiveStartEvent = true;
	MidiFileContainer[] songs;
	List<MidiTrackSequencer> sequencers;
	[Range(0,1)]
	public float
		volume = 1.0f;
	public bool playMasterMusic;
	public AudioClip masterMusic;
	public bool playOnAwake;
	private bool _isPlaying;

	void Start ()
	{
		songs = new MidiFileContainer[ midiFiles.Length ];
		sequencers = new List<MidiTrackSequencer> ();
		if (playMasterMusic) {
			AudioSource a = gameObject.AddComponent<AudioSource> () as AudioSource;
			a.clip = masterMusic;
			a.volume = volume;
		} else {
			for (int i=0; i < audioFiles.Length; ++i) {
				AudioSource a = gameObject.AddComponent<AudioSource> () as AudioSource;
				a.clip = audioFiles [i];
				a.volume = volume;
			}
		}
		for (int i=0; i < midiFiles.Length; ++i) {
			songs [i] = MidiFileLoader.Load (midiFiles [i].bytes);
		}
		if (playOnAwake) {
			ResetAndPlay ();
		}
	}
	
	void LateUpdate ()
	{
		if (sequencers != null) {
			foreach (MidiTrackSequencer seq in sequencers) {
				if (seq.Playing) {
					VJMidiInput.ReceiveMidiEvents (seq.Advance (Time.deltaTime));
				}
			}
			Component[] cs = GetComponents<AudioSource> ();
			if (cs != null) {
				foreach (Component c in cs) {
					AudioSource a = c as AudioSource;
					a.volume = volume;
					_isPlaying = a.isPlaying;
				}
			}
		}
	}
	
	void ResetAndPlay ()
	{
		Component[] cs = GetComponents<AudioSource> ();
		foreach (Component c in cs) {
			AudioSource a = c as AudioSource;
			a.time = startTime;
			a.Play ();
		}

		foreach (MidiFileContainer song in songs) {
			for (int i = 0; i < song.tracks.Count; ++i) {
				MidiTrackSequencer s = new MidiTrackSequencer (song.tracks [i], song.division, bpm);
				List<MidiEvent> e = s.Start (startTime);
				if (receiveStartEvent) {
					VJMidiInput .ReceiveMidiEvents (e);
				}
				sequencers.Add (s);
			}
		}
	}

	void OnGUI ()
	{
		if (!_isPlaying) {
			if (GUI.Button (new Rect (0, 0, 300, 50), "Play " + name)) {
				ResetAndPlay ();
			}
		}
	}
}

