using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ParticleEmitter))]
[AddComponentMenu("VJKit/Triggers/Emit particle(Old Particle system)")]
public class VJParticleEmitterEmitTrigger : VJBaseTrigger {
	public override void OnVJTrigger() {
		particleEmitter.Emit();
	}
}
