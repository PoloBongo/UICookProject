using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    private Rigidbody rb;

    [Header("Boat Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float backwardSpeed = 5f;
    [SerializeField] private float turnSpeed = 50f;

    private float forwardInput = 0f;
    private float turnInput = 0f;
    private PlayerInputAction playerInputAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody is missing on the Player.");
        }
        playerInputAction = InputActionManager.Instance.GetPlayerInputAction();

        playerInputAction.Player.Movement.performed += OnMovePerformed;
        playerInputAction.Player.Movement.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        playerInputAction.Player.Movement.performed -= OnMovePerformed;
        playerInputAction.Player.Movement.canceled -= OnMoveCanceled;
        playerInputAction.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (this == null) return;
        Vector2 input = context.ReadValue<Vector2>();
        forwardInput = input.y;
        turnInput = input.x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (this == null) return;
        forwardInput = 0f;
        turnInput = 0f;
    }
    
    private void FixedUpdate()
    {
        // Avancer & reculer avec le bateau
        if (Mathf.Abs(forwardInput) > 0.1f)
        {
            Vector3 forwardForce = -transform.forward * ((forwardInput < 0 ? forwardSpeed : backwardSpeed) * forwardInput);
            rb.AddForce(forwardForce, ForceMode.Force);
        }

        // Tourner le bateau (ça marche en même temps que pour l'avancer donc on peut faire les deux en même temps)
        if (!(Mathf.Abs(turnInput) > 0.1f)) return;
        float turnAngle = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAngle, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}
