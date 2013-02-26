#pragma strict

static var globalTarget : GameObject;

static var globalSpeed = 0.1;
static var globalYawing = 90.0;

private var speedScale = 1.0;
private var yawingScale = 1.0;

function Start() {
    transform.rotation = Random.rotation;
    speedScale = Random.Range(0.8, 1.2);
    yawingScale = Random.Range(0.8, 1.2);
}

function Update() {
    if (!globalTarget) {
        Destroy(gameObject);
    } else {
        transform.position += transform.forward * (globalSpeed * speedScale * Time.deltaTime);
        var rotation = Quaternion.FromToRotation(transform.forward, globalTarget.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(Quaternion.identity, rotation, globalYawing * yawingScale * Time.deltaTime) * transform.rotation;
    }
}
