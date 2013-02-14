using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Destroy object in sec(s)")]
public class VJObjectDestroyer : MonoBehaviour {

	public float objectLiveSec;

	IEnumerator Start () {
		yield return new WaitForSeconds(objectLiveSec);
		Destroy(gameObject);
	}	
}
