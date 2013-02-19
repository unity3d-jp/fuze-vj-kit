using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Emit particle(Shuriken[new])")]
public class VJParticleSystemEmitTrigger : VJBaseTrigger {

	public override void OnVJTrigger(GameObject go, float value) {
	
		ParticleSystem p = go.GetComponent<ParticleSystem>();
		p.Emit((int)(value));
	}
}
