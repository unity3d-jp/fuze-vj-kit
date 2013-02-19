using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Spawn Object")]
public class VJSpawnTrigger : VJBaseTrigger {

	public GameObject spawnObject;	
	public bool destroySpawnedObject = false;
	public float objectLiveSec = 1.0f;
	public Vector3 randomPositionOffset;

	public override void OnVJTrigger(GameObject go, float value) {
		
		if( !spawnObject ) {
			return;
		}
		
		float x_offset = Random.Range(-randomPositionOffset.x, randomPositionOffset.x);
		float y_offset = Random.Range(-randomPositionOffset.y, randomPositionOffset.y);
		float z_offset = Random.Range(-randomPositionOffset.z, randomPositionOffset.z);
															
		Vector3 spawnPos = go.transform.position + new Vector3(x_offset, y_offset, z_offset);
		
		GameObject spawn = (GameObject)GameObject.Instantiate(spawnObject, spawnPos, go.transform.rotation);
		if(destroySpawnedObject) {
			VJObjectDestroyer d = (VJObjectDestroyer)spawn.AddComponent<VJObjectDestroyer>();
			d.objectLiveSec = objectLiveSec;
		}
	}
}
