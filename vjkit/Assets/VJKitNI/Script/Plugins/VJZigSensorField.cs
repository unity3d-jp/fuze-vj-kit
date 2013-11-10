using UnityEngine;
using System.Collections;

public class VJZigSensorField : MonoBehaviour {

	[SerializeField]
	private bool m_touching;
	[SerializeField]
	private bool m_touchUp;
	[SerializeField]
	private bool m_touchDown;
	[SerializeField]
	private float m_valueV;
	[SerializeField]
	private float m_valueH;
	[SerializeField]
	private float m_valueD;

	[SerializeField]
	private GameObject m_sensor;

	public bool Touching 	{	get { return m_touching; } }
	public bool TouchUp 	{	get { return m_touchUp; } }
	public bool TouchDown 	{	get { return m_touchDown; } }
	public float ValueVert 	{	get { return m_valueV; } }
	public float ValueHrz 	{	get { return m_valueH; } }
	public float ValueDpth 	{	get { return m_valueD; } }

	private void _updateTriggerToValue() {

		Vector3 sensorPos_local = transform.InverseTransformPoint(m_sensor.transform.position);

		Vector3 lowEnd 	= transform.InverseTransformPoint(collider.bounds.min);
		Vector3 highEnd = transform.InverseTransformPoint(collider.bounds.max);

		Debug.Log ("[Sensor] lowend:" + lowEnd + " highEnd:" + highEnd + " sensorPos:" + sensorPos_local);

		m_valueV = sensorPos_local.x / (highEnd.x - lowEnd.x) + 0.5f;
		m_valueH = sensorPos_local.y / (highEnd.y - lowEnd.y) + 0.5f;
		m_valueD = sensorPos_local.z / (highEnd.z - lowEnd.z) + 0.5f;
	}

	private bool _IsInSensor(float v, float h, float d) {
		return 	( v > 0.0f && v < 1.0f ) && 
				( h > 0.0f && h < 1.0f ) &&
				( d > 0.0f && d < 1.0f );
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		bool wasTouching = _IsInSensor(m_valueV, m_valueH, m_valueD);
		_updateTriggerToValue();

		m_touching = _IsInSensor(m_valueV, m_valueH, m_valueD);
		m_touchDown = m_touching  && !wasTouching;
		m_touchUp   = !m_touching && wasTouching;
	}
}
