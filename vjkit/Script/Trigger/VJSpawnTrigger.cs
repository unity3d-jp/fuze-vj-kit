/* 
 * fuZe vjkit
 * 
 * Copyright (C) 2013 Unity Technologies Japan, G.K.
 * 
 * Permission is hereby granted, free of charge, to any 
 * person obtaining a copy of this software and associated 
 * documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, 
 * sublicense, and/or sell copies of the Software, and to permit 
 * persons to whom the Software is furnished to do so, subject 
 * to the following conditions: The above copyright notice and 
 * this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
 * OTHER DEALINGS IN THE SOFTWARE.
 * 
 * Special Thanks:
 * The original software design and architecture of fuZe vjkit 
 * is inspired by Visualizer Studio, created by Altered Reality 
 * Entertainment LLC.
 */
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
