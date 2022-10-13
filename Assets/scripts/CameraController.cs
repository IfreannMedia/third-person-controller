using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] float distance = 5;

    private void Update()
    {
        transform.position = target.position - new Vector3(0, 0, distance);
    }
}
