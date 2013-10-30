// Unity MIDI Input plug-in / Inspector
// By Keijiro Takahashi, 2013
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(VJMidiInput))]
class VJMidiInputEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		var midi = target as VJMidiInput;
		
		// Filter sensibility settings.
		midi.sensibilitySlow = EditorGUILayout.Slider ("CC Sensibility (slow)", midi.sensibilitySlow, 1.0f, 25.0f);
		midi.sensibilityFast = EditorGUILayout.Slider ("CC Sensibility (fast)", midi.sensibilityFast, 1.0f, 25.0f);
		
		// Only shows the details on Play Mode.
		if (EditorApplication.isPlaying) {
			var endpointCount = VJMidiInput.CountEndpoints ();
			
			// Endpoints.
			var temp = "Detected MIDI endpoints:";
			for (var i = 0; i < endpointCount; i++) {
				var id = VJMidiInput.GetEndpointIdAtIndex (i);
				var name = VJMidiInput.GetEndpointName (id);
				temp += "\n" + id.ToString ("X8") + ": " + name;
			}
			EditorGUILayout.HelpBox (temp, MessageType.None);
			
			// Incomming messages.
			temp = "Incoming MIDI messages:";
			foreach (var message in (target as VJMidiInput).History) {
				temp += "\n" + message.ToString ();
			}
			EditorGUILayout.HelpBox (temp, MessageType.None);
			
			// Make itself dirty to update on every time.
			EditorUtility.SetDirty (target);
		} else {
			EditorGUILayout.HelpBox ("You can view the sutatus on Play Mode.", MessageType.Info);
		}
	}
}
