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
	private bool m_inverseV;
	[SerializeField]
	private bool m_inverseH;
	[SerializeField]
	private bool m_inverseD;
	[SerializeField]
	private Color touchColor;

	private static float tx = 40.0f;
	private static float ux  =0.01f;

	[SerializeField]
	private GameObject m_sensor;

	public bool Touching 	{	get { return m_touching; } }
	public bool TouchUp 	{	get { return m_touchUp; } }
	public bool TouchDown 	{	get { return m_touchDown; } }
	public float ValueVert 	{	get { return m_valueV; } }
	public float ValueHrz 	{	get { return m_valueH; } }
	public float ValueDpth 	{	get { return m_valueD; } }

	private bool _IsInSensor(float v, float h, float d) {
		return 	( v > 0.0f && v < 1.0f ) && 
				( h > 0.0f && h < 1.0f ) &&
				( d > 0.0f && d < 1.0f );
	}

	// Use this for initialization
	void Start () {
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(collider.bounds.center,collider.bounds.size);

		if(m_touching) {
			Gizmos.color = Color.white;
			Vector3 c = collider.bounds.center;
			c.x += Mathf.Sin (Time.time * tx) * ux;
			c.y += Mathf.Cos (Time.time * tx) * ux;
			c.z += Mathf.Sin (Time.time * tx) * ux;
			Gizmos.DrawWireCube(c,collider.bounds.size);
		}
	}
	// Update is called once per frame
	void Update () {
		Vector3 sensorPos_local = transform.InverseTransformPoint(m_sensor.transform.position);

		Vector3 lowEnd 	= transform.InverseTransformPoint(collider.bounds.min);
		Vector3 highEnd = transform.InverseTransformPoint(collider.bounds.max);

		float valueV = sensorPos_local.x / (highEnd.x - lowEnd.x) + 0.5f;
		float valueH = sensorPos_local.y / (highEnd.y - lowEnd.y) + 0.5f;
		float valueD = sensorPos_local.z / (highEnd.z - lowEnd.z) + 0.5f;

		if( m_inverseV ) {
			valueV = 1.0f - valueV;
		}
		if( m_inverseH ) {
			valueH = 1.0f - valueH;
		}
		if( m_inverseD ) {
			valueD = 1.0f - valueD;
		}

		m_touching = _IsInSensor(valueV, valueH, valueD);
		bool wasTouching = _IsInSensor(m_valueV, m_valueH, m_valueD);
		m_touchDown = m_touching  && !wasTouching;
		m_touchUp   = !m_touching && wasTouching;

		if( m_touching ) {
			m_valueD = valueD;
			m_valueV = valueV;
			m_valueH = valueH;
		} else {
			m_valueD = 0.0f;
			m_valueV = 0.0f;
			m_valueH = 0.0f;
		}
	}
}
