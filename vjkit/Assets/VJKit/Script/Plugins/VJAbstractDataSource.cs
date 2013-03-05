using UnityEngine;
using System.Collections;

[RequireComponent (typeof (VJAbstractManager))]
public abstract class VJAbstractDataSource : MonoBehaviour {

	public static float s_prog_max = 10.0f;

	public string sourceName;

	[Range(0.01f, 100.0f)]
	public float boost = 1.0f;

	[HideInInspector]
	public float current;
	[HideInInspector]
	public float previous;
	[HideInInspector]
	public float diff;
}
