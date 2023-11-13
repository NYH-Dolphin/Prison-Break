using System.Collections;
using GameInputSystem;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;


[RequireComponent(typeof(Rigidbody), typeof(PlayerWeapon))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fMovementSpeed = 10f;
    [SerializeField] private float fJumpSpeed = 10f;
    [SerializeField] private float fGroundCheckRadius = 0.3f;
    [SerializeField] private LayerMask lmGroundLayer;
    [SerializeField] private Animator animator;

    public Vector3 vecDir => _vecDir;

    private Rigidbody _rb;
    private PlayerWeapon _pw;
    private InputControls _inputs;
    private Vector3 _vecMove = Vector3.zero; // player movement direction
    private Vector3 _vecDir = new Vector3(0, 0, 1); // player facing direction
    private bool _bJumpUpdate = true;
    private bool _bIsGrounded;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _pw = GetComponent<PlayerWeapon>();
    }


    #region EventRegisteration

    // TODO Currently Jump is Removed
    private void OnEnable()
    {
        if (_inputs == null)
        {
            _inputs = new InputControls();
        }

        _inputs.Gameplay.Movement.Enable();
        _inputs.Gameplay.Jump.Enable();
        _inputs.Gameplay.Movement.performed += OnMovementPerformed;
        _inputs.Gameplay.Movement.canceled += OnMovementCanceled;
        // _inputs.Gameplay.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _inputs.Gameplay.Movement.Disable();
        _inputs.Gameplay.Jump.Disable();
        _inputs.Gameplay.Movement.performed -= OnMovementPerformed;
        _inputs.Gameplay.Movement.canceled -= OnMovementCanceled;
        // _inputs.Gameplay.Jump.performed -= OnJumpPerformed;
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


    public void OnAttackPerformed(AttackType type)
    {
        switch (type)
        {
            case AttackType.Swing:
                animator.SetTrigger("Swing");
                break;
            case AttackType.Throwable:
                
                animator.SetTrigger("Throw");
                break;
        }
    }
    
    public void OnSetAttackDir(Vector3 dir)
    {
        dir = dir.normalized;
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
    }

    #region Movement

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        Vector2 inputMove = value.ReadValue<Vector2>();
        animator.SetFloat("Horizontal", inputMove.x);
        animator.SetFloat("Vertical", inputMove.y);
        animator.SetFloat("Speed", inputMove.magnitude);

        // player moving direction is based on the view of camera
        Transform cameraTransform = Camera.main.transform;
        Vector3 vecFront = cameraTransform.forward;
        vecFront.y = 0;
        vecFront = vecFront.normalized;
        Vector3 vecRight = cameraTransform.right;
        vecRight.y = 0;
        vecRight = vecRight.normalized;
        Vector3 vecIsoMove = vecRight * inputMove.x + vecFront * inputMove.y;
        _vecMove = vecIsoMove.normalized;
        _vecDir = _vecMove;
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _vecMove = Vector3.zero;
        animator.SetFloat("Speed", 0f);
    }

    private void MovementUpdate()
    {
        Vector3 moveVelocity = _vecMove * fMovementSpeed;
        moveVelocity.y = _rb.velocity.y;
        _rb.velocity = moveVelocity;
    }

    #endregion


    #region Jump

    public void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (_bIsGrounded)
        {
            _bIsGrounded = false;
            animator.SetBool("Grounded", false);
            animator.SetTrigger("Jump");
            StartCoroutine(JumpCountDown(0.1f)); // wait a really time for jump detection
            Vector3 velocity = _rb.velocity;
            velocity.y = 0f;
            _rb.velocity = velocity;
            _rb.AddForce(Vector3.up * fJumpSpeed, ForceMode.Impulse);
        }
    }


    IEnumerator JumpCountDown(float time)
    {
        _bJumpUpdate = false;
        yield return new WaitForSeconds(time);
        _bJumpUpdate = true;
    }

    public void GroundDetectUpdate()
    {
        if (!_bJumpUpdate) return;
        Collider[] hitColliders = new Collider[1];
        _bIsGrounded =
            Physics.OverlapSphereNonAlloc(transform.position, fGroundCheckRadius, hitColliders, lmGroundLayer) != 0;
        _rb.drag = _bIsGrounded ? 1f : 0f;
        animator.SetBool("Grounded", _bIsGrounded);
    }

    #endregion


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fGroundCheckRadius);
    }
}