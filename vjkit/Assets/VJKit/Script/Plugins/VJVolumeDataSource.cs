using UnityEngine;
using System.Collections;

[AddComponentMenu("VJKit/System/VJ Data source(Volume)")]
[RequireComponent (typeof (VJManager))]
public class VJVolumeDataSource : VJAbstractDataSource {

	private VJManager m_manager;

	// Use this for initialization
	public void Awake() {
		m_manager = GetComponent<VJManager>();
	}

	// Update is called once per frame
	public void Update () {

		float raw_current = m_manager.rawVolume * boost;

		previous = current;
		current = raw_current;
		diff = current - previous;
	}
}
