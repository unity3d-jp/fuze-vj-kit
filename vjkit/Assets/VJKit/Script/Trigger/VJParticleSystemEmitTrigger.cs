using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ParticleSystem))]
[AddComponentMenu("VJKit/Triggers/Emit particle(Shuriken[new])")]
public class VJParticleSystemEmitTrigger : VJBaseTrigger {

	public float boost = 1.0f;

	public override void OnVJTrigger() {
		particleSystem.Emit((int)(boost * GetValue()));
	}
}
