using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fMovementSpeed = 10f;
    [SerializeField] private float fJumpSpeed = 10f;
    [SerializeField] private float fGroundCheckRadius = 0.3f;
    [SerializeField] private LayerMask lmGroundLayer;
    [SerializeField] private Animator animator;


    private Rigidbody _rb;
    private InputControls _inputs;
    private Vector3 _vecMove = Vector3.zero;
    private bool _bIsGrounded;


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

    private void Update()
    {
        GroundDetectUpdate();
    }


    private void FixedUpdate()
    {
        MovementUpdate();
    }


    #region Movement

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Vector2 inputMove = value.ReadValue<Vector2>();
        animator.SetFloat("Horizontal", inputMove.x);
        animator.SetFloat("Vertical", inputMove.y);
        animator.SetFloat("Speed", inputMove.magnitude);
        _vecMove.x = inputMove.x;
        _vecMove.z = inputMove.y;
        _vecMove = _vecMove.normalized;
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _vecMove = Vector3.zero;
        animator.SetFloat("Speed", 0f);
    }

    private void MovementUpdate()
    {
        if (_bIsGrounded)
        {
            _rb.velocity = _vecMove * fMovementSpeed;
        }
    }

    #endregion


    #region Jump

    public void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (_bIsGrounded)
        {
            _bIsGrounded = false;
            Vector3 velocity = _rb.velocity;
            velocity.y = 0f;
            _rb.velocity = velocity;
            _rb.AddForce(Vector3.up * fJumpSpeed, ForceMode.Impulse);
        }
    }

    public void GroundDetectUpdate()
    {
        Collider[] hitColliders = new Collider[1];
        _bIsGrounded =
            Physics.OverlapSphereNonAlloc(transform.position, fGroundCheckRadius, hitColliders, lmGroundLayer) != 0;
        _rb.drag = _bIsGrounded ? 1f : 0f;
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fGroundCheckRadius);
    }
}