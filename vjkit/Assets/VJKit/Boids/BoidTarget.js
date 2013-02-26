#pragma strict

var orbitRadius = 1.0;
var kill = false;

private var angles = Vector3.zero;
private var omega = Vector3.zero;

function Start() {
    omega.x = Random.Range(0.3, 0.6);
    omega.y = Random.Range(0.3, 0.6);
    omega.z = Random.Range(0.3, 0.6);

    while (!kill) {
        transform.localPosition = Vector3(
            Mathf.Sin(angles.x),
            Mathf.Sin(angles.y),
            Mathf.Sin(angles.z)
        ) * orbitRadius;
        angles += omega * Time.deltaTime;
        yield;
    }

    transform.localPosition = Vector3(0, 100 ,0);

    yield WaitForSeconds(5.0);

    Destroy(gameObject);
}
