using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Utilities/Destroy object in sec(s)")]
public class VJObjectDestroyer : MonoBehaviour {

	public float objectLiveSec;
	public GameObject spawnAtDeath;

	IEnumerator Start () {
		yield return new WaitForSeconds(objectLiveSec);
		if( spawnAtDeath != null ) {
			GameObject.Instantiate(spawnAtDeath, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}	
}
