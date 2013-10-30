using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/Midi Manager")]
public class VJMidiManager : VJAbstractManager {
	
//	Queue<MidiMessage> messageQueue;
//	
//	public bool IsEmpty {
//		get { return messageQueue.Count == 0; }
//	}
//	
//	public MidiMessage PopMessage ()
//	{
//		return messageQueue.Dequeue ();
//	}
//	
//	#if UNITY_EDITOR
//	Queue<MidiMessage> messageHistory;
//	
//	public Queue<MidiMessage> History {
//		get { return messageHistory; }
//	}
//	#endif
//	

	public override void Awake() {
		base.Awake();		
	}
//
//	void Update ()
//	{
//		while (true) {
//			var data = UnityMidiReceiver.DequeueIncomingData ();
//			if (data == 0) {
//				break;
//			}
//			
//			var message = new MidiMessage (data);
//			messageQueue.Enqueue (message);
//			#if UNITY_EDITOR
//			messageHistory.Enqueue (message);
//			#endif
//		}
//		#if UNITY_EDITOR
//		while (messageHistory.Count > 8) {
//			messageHistory.Dequeue ();
//		}
//		#endif
//	}
}
