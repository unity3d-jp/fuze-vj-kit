using UnityEngine;
using System.Collections;

public abstract class VJGravityItem : MonoBehaviour {

	public enum GravityType {
		Force,
		Vecocity
	}

	public GravityType gravityType;
	public float strength = 9.81f;
}
