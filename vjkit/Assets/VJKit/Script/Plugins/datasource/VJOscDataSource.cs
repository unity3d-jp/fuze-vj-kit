using UnityEngine;

[RequireComponent (typeof(VJOscManager))]
public class VJOscDataSource : VJAbstractDataSource {
	public string address;
	public float value = 0.0f;

	public void Update () {
		float raw_current = value * boost;

		previous = current;
		current = raw_current;
		diff = current - previous;
	}
}
