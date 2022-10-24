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

    private Vector3 desiredMoveDir; // moveDir from player's input
    private Vector3 moveDir;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private bool isGrounded;
    private bool hasControl = true;
    private float ySpeed;


    public LedgeData LedgeData{ get; set; }
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
        velocity = Vector3.zero;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        Vector3 moveInput = (new Vector3(h, 0, v)).normalized;

        desiredMoveDir = camController.PlanerRotation * moveInput;
        moveDir = desiredMoveDir;

        if (!hasControl) return;

        GroundCheck();
        animator.SetBool("isGrounded", isGrounded);
        if (isGrounded)
        {
            velocity = desiredMoveDir * moveSpeed;
            ySpeed = -0.05f;

            IsOnLedge = envScanner.LedgeCheck(desiredMoveDir, out LedgeData ledgeData);
            if (IsOnLedge)
            {
                LedgeData = ledgeData;
                LedgeMovement();
            }
            animator.SetFloat("moveAmount", velocity.magnitude / moveSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
            velocity = transform.forward * moveSpeed / 2f;
        }

        velocity.y = ySpeed;

        charController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0 && moveDir.magnitude > .2f)
        {

            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards
            (transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

       
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

    private void LedgeMovement()
    {
        float angle = Vector3.Angle(LedgeData.surfaceHit.normal, desiredMoveDir);
        
        if(angle < 90)
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    public void SetControl(bool hasControl)
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
