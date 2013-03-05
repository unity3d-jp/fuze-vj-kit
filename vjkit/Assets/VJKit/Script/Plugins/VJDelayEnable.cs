using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Enable Component in sec(s)")]
public class VJDelayEnable : MonoBehaviour {

	public float toggleSec;
	public Behaviour behaviourToEnable;

	IEnumerator Start () {
		yield return new WaitForSeconds(toggleSec);
		behaviourToEnable.enabled = !behaviourToEnable.enabled;
	}	
}
