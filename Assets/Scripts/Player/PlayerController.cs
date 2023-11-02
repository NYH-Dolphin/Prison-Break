using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fMovementSpeed = 10f;
    [SerializeField] private float fJumpSpeed = 10f;

    private Rigidbody _rb;
    private InputControls _inputs;
    private Vector3 _vecMove = Vector3.zero;
    private bool _bOnGround;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    #region EventRegisteration

    private void OnEnable()
    {
        if (_inputs == null)
        {
            _inputs = new InputControls();
        }

        _inputs.Gameplay.Enable();
        _inputs.Gameplay.Movement.performed += OnMovementPerformed;
        _inputs.Gameplay.Movement.canceled += OnMovementCanceled;
        _inputs.Gameplay.Jump.performed += OnJumpPerformed;
    }

    public void OnDisable()
    {
        _inputs.Gameplay.Disable();
        _inputs.Gameplay.Movement.performed -= OnMovementPerformed;
        _inputs.Gameplay.Movement.canceled -= OnMovementCanceled;
        _inputs.Gameplay.Jump.performed -= OnJumpPerformed;
    }

    #endregion


    private void FixedUpdate()
    {
        MovementUpdate();
    }


    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Vector2 inputMove = value.ReadValue<Vector2>();
        _vecMove.x = inputMove.x;
        _vecMove.z = inputMove.y;
        _vecMove = _vecMove.normalized;
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _vecMove = Vector3.zero;
    }

    private void MovementUpdate()
    {
        _rb.velocity = _vecMove * fMovementSpeed;
    }


    public void OnJumpPerformed(InputAction.CallbackContext value)
    {
        _rb.drag = 0;
        _rb.AddForce(Vector3.up * fJumpSpeed, ForceMode.Impulse);
    }

    public void OnJumpCanceled()
    {
    }

    public void OnWeapon(InputAction.CallbackContext context)
    {
    }
}