using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 0.08f;
    [SerializeField, Range(45f, 90f)] private float maxLookAngle = 85f;

    private CharacterController characterController;
    private float verticalVelocity;
    private float cameraPitch;
    private bool inputEnabled = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        SetCursorLocked(true);
    }

    private void Update()
    {
        HandleCursorToggle();

        if (!inputEnabled || Cursor.lockState != CursorLockMode.Locked)
            return;

        HandleMouseLook();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Keyboard.current == null)
            return;

        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1f;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1f;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1f;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1f;

        // Prevents diagonal movement from being faster.
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        bool isSprinting = Keyboard.current.leftShiftKey.isPressed;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 horizontalMovement =
            (transform.right * moveInput.x + transform.forward * moveInput.y)
            * currentSpeed;

        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMovement = horizontalMovement + Vector3.up * verticalVelocity;
        characterController.Move(finalMovement * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        if (Mouse.current == null || playerCamera == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity;

        cameraPitch -= mouseDelta.y;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        transform.Rotate(Vector3.up * mouseDelta.x);
    }

    private void HandleCursorToggle()
    {
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            SetCursorLocked(false);

        if (inputEnabled &&
            Cursor.lockState != CursorLockMode.Locked &&
            Mouse.current?.leftButton.wasPressedThisFrame == true)
        {
            SetCursorLocked(true);
        }
    }

    /// <summary>
    /// Call this when opening or closing a shop, dialogue, or haggling UI.
    /// </summary>
    public void SetInputEnabled(bool value)
    {
        inputEnabled = value;
        verticalVelocity = 0f;
        SetCursorLocked(value);
    }

    private static void SetCursorLocked(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}