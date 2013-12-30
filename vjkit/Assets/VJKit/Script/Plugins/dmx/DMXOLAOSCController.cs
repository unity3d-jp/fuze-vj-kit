using UnityEngine;

public class DMXOLAOSCController : DMXAbstractController {
	public VJOSCClient oscClient;

	private bool blackout = false;

	public void ToggleBlackout() { 
		Debug.Log ("Blackout:" + blackout);
		blackout = !blackout; 
		oscClient.SendSimpleMessage("/dmx/blackout", blackout ? 1 : 0); 
	}

	protected override void SendChannelData(byte[] channelData) {
		oscClient.SendSimpleMessage("/dmx/universe/0", channelData);
	}
}
