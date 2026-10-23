using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Base walking speed in units per second.")]
    [SerializeField] private float walkSpeed = 5.0f;

    [Tooltip("Sprint speed multiplier.")]
    [SerializeField] private float sprintMultiplier = 1.6f;

    [Tooltip("Smoothing time for turning towards movement direction.")]
    [SerializeField] private float rotationSmoothTime = 0.1f;

    [Header("Jump & Gravity")]
    [Tooltip("Jump height in units.")]
    [SerializeField] private float jumpHeight = 1.5f;

    [Tooltip("Downward gravity acceleration.")]
    [SerializeField] private float gravity = -19.62f;

    [Tooltip("Reference to the main camera transform for camera-relative movement.")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private Vector3 verticalVelocity;
    private float turnSmoothVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleJumpAndGravity();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Calculate target angle based on input and camera orientation
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            if (cameraTransform != null)
            {
                targetAngle += cameraTransform.eulerAngles.y;
            }

            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float currentSpeed = walkSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1.0f);

            controller.Move(moveDir.normalized * (currentSpeed * Time.deltaTime));
        }
    }

    private void HandleJumpAndGravity()
    {
        if (controller.isGrounded && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f; // Slight downward force to keep grounded
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}
