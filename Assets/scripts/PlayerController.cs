using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f; // the degrees by which to rotate

    private Quaternion targetRotation;

    private CameraController camController;

    private void Awake()
    {
        camController = Camera.main.GetComponent<CameraController>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);

        Vector3 moveInput = (new Vector3(h, 0, v)).normalized;

        Vector3 moveDir = camController.PlanerRotation * moveInput;

        if (moveAmount > 0)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards
            (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
}
