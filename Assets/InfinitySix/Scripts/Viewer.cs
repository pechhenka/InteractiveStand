using UnityEngine;

public class Viewer : MonoBehaviour
{
public float k = 2;

public float dampTime = 0.15f;
private Vector3 velocity = Vector3.zero;
public Transform target;

[HideInInspector] public Camera cam;

private void Start()
{
cam = GetComponent<Camera>();
}

void Update()
{
Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
Vector3 destination = transform.position + delta;
transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

float tmp = Mathf.Abs(target.position.y) * k;
if (tmp > cam.orthographicSize)
cam.orthographicSize = tmp;
}

public void NewScale()
{
velocity /= 256;
transform.position /= 256;
cam.orthographicSize /= 256;
}
}
