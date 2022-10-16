using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f; // the degrees by which to rotate
    [Header("Ground Check Settings")]
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private LayerMask groundLayer;

    private Quaternion targetRotation;
    private bool isGrounded;
    private bool hasControl = true;
    private float ySpeed;

    public bool IsOnLedge { get; set; }

    private CameraController camController;
    private Animator animator;
    private CharacterController charController;
    private EnvironmentScanner envScanner;

    private void Awake()
    {
        camController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        envScanner = GetComponent<EnvironmentScanner>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        Vector3 moveInput = (new Vector3(h, 0, v)).normalized;

        Vector3 moveDir = camController.PlanerRotation * moveInput;

        if (!hasControl) return;

        GroundCheck();
        if (isGrounded)
        {
            ySpeed = -0.05f;

            IsOnLedge = envScanner.LedgeCheck(moveDir);
            if (IsOnLedge)
            {
                Debug.Log("on LEDGE BABY");
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        charController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {

            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards
            (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? new Color(0, 1, 0, .5f) : new Color(1, 0, 0, .75f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    public void HasControl(bool hasControl)
    {
        this.hasControl = hasControl;
        this.charController.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("moveAmount", 0f);
            targetRotation = transform.rotation;
        }
    }

    public float RotationSpeed => rotationSpeed;
}
