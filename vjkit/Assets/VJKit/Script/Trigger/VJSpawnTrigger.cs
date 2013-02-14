using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Spawn Object")]
public class VJSpawnTrigger : VJBaseTrigger {

	public GameObject spawnObject;	
	public bool destroySpawnedObject = false;
	public float objectLiveSec = 1.0f;
	public Vector3 randomPositionOffset;

	public override void OnVJTrigger() {
		
		if( !spawnObject ) {
			return;
		}
		
		float x_offset = Random.Range(-randomPositionOffset.x, randomPositionOffset.x);
		float y_offset = Random.Range(-randomPositionOffset.y, randomPositionOffset.y);
		float z_offset = Random.Range(-randomPositionOffset.z, randomPositionOffset.z);
															
		Vector3 spawnPos = transform.position + new Vector3(x_offset, y_offset, z_offset);
		
		GameObject go = (GameObject)GameObject.Instantiate(spawnObject, spawnPos, transform.rotation);
		if(destroySpawnedObject) {
			VJObjectDestroyer d = (VJObjectDestroyer)go.AddComponent<VJObjectDestroyer>();
			d.objectLiveSec = objectLiveSec;
		}
	}
}
