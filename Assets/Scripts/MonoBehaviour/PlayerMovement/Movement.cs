using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float turnSpeed = 200f;

    private Vector2 moveInput;
    private Rigidbody rb;
    private InputActionManager inputActionManager;
    private PlayerInputAction playerInputAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on the Player.");
        }
        
        if (inputActionManager == null) inputActionManager = InputActionManager.Instance;
        if (playerInputAction == null) playerInputAction = inputActionManager.GetPlayerInputAction();
        playerInputAction.Player.Movement.performed += OnMovePerformed;
        playerInputAction.Player.Movement.canceled += OnMoveCanceled;
    }

    private void OnEnable()
    {
        if (playerInputAction != null)
        {
            playerInputAction.Player.Movement.performed += OnMovePerformed;
            playerInputAction.Player.Movement.canceled += OnMoveCanceled;
        }
    }

    private void OnDisable()
    {
        playerInputAction.Player.Movement.performed -= OnMovePerformed;
        playerInputAction.Player.Movement.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (moveInput.sqrMagnitude > 1)
        {
            moveInput = moveInput.normalized;
        }

        Vector3 forwardMovement = transform.forward * (moveInput.y * moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(rb.position + forwardMovement);

        float turnAmount = moveInput.x * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}