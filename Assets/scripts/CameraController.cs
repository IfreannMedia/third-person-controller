using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private float distance = 5;

    private float rotationY;

    private void Update()
    {
        rotationY += Input.GetAxis("Mouse X");
        Quaternion targetRotation = Quaternion.Euler(0, rotationY, 0);
        transform.position = target.position - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }
}
