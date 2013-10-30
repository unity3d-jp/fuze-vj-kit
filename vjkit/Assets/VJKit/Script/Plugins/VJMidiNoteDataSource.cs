using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Midi/Midi Note Source")]
[RequireComponent (typeof (VJMidiManager))]
public class VJMidiNoteDataSource : VJAbstractDataSource {

//	private VJGamepadManager m_manager;
	// depends VJManager.numberOfBands
	[HideInInspector]
	public int lowerNote;
	// depends VJManager.numberOfBands
	[HideInInspector]
	public int upperNote;
	
	// Use this for initialization
	public void Awake() {
//		m_manager = GetComponent<VJGamepadManager>();
	}

	// Update is called once per frame
	public void Update () {
		float raw_current = 0.0f;
		int noteCount = 0;
		for(int i = lowerNote; i <= upperNote; ++i) {
			float value = VJMidiInput.GetKey(i);
			raw_current += value;
			if(value > 0.0f) ++noteCount;
		}
		if(noteCount > 0) {
			raw_current /= noteCount;
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
