using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/Triggers/Emit particle(Old Particle system)")]
public class VJParticleEmitterEmitTrigger : VJBaseTrigger {
	public override void OnVJTrigger(GameObject go, float value) {

		ParticleEmitter p = go.GetComponent<ParticleEmitter>();
		if(p) {
			particleEmitter.Emit();
		}
	}
}
