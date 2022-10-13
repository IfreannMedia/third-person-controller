using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5;
    [SerializeField] private float rotationSpeed = 3;
    [SerializeField] private float minVerticalAngle = -60;
    [SerializeField] private float maxVerticalAngle = 60;
    [SerializeField] private Vector2 framingOffset = new Vector2(0, 1);

    private float rotationX;
    private float rotationY;

    private void Update()
    {
        rotationX += Input.GetAxis("Mouse Y") * rotationSpeed;
        rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

        Vector3 focusPos = target.position + new Vector3(framingOffset.x, framingOffset.y, 0);
        transform.position = focusPos - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }
}
