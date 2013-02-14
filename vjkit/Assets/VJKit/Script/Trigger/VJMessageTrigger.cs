using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Send message")]
public class VJMessageTrigger : VJBaseTrigger {

	public GameObject objectToSend;	
	public string message = null;

	void Start() {
		if(objectToSend == null) {
			objectToSend = gameObject;
		}
	}

	public override void OnVJTrigger() {
		if( null == message || message.Length == 0 ) {
			return;
		}		
		objectToSend.SendMessage(message, GetValue(), SendMessageOptions.DontRequireReceiver);		
	}
}
