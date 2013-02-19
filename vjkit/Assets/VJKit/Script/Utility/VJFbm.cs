using UnityEngine;
using System.Collections;

public class VJFbm : MonoBehaviour {

	[Range(1, 4)]
	public int octave = 2;
	[Range(0.0f, 4.0f)]
	public float frequency = 0.5f;
	[Range(0.0f, 0.5f)]
	public float positionAmount = 0.15f;
	[Range(0.0f, 5.0f)]
	public float rotationAmount = 1.5f;

	public void ApplyFbmMotion() {
		float time = Time.time * frequency;
		float dx = VJPerlin.Fbm(time, octave);
		float dy = VJPerlin.Fbm(time + 10, octave);
		float dz = VJPerlin.Fbm(time + 20, octave);
		float rx = VJPerlin.Fbm(time + 30, octave);
		float ry = VJPerlin.Fbm(time + 40, octave);
		transform.localPosition = new Vector3(dx, dy, dz) * positionAmount;
		transform.localRotation =
			Quaternion.AngleAxis(rx * rotationAmount, Vector3.right) *
			Quaternion.AngleAxis(ry * rotationAmount, Vector3.up);
	}
}
