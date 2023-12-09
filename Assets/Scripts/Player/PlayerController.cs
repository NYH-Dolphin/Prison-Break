using System.Collections;
using GameInputSystem;
using MyCameraEffect;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapon;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody), typeof(PlayerWeapon))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fMovementSpeed;
    [SerializeField] private float fJumpSpeed;
    [SerializeField] private float fGroundCheckRadius;
    [SerializeField] private LayerMask lmGroundLayer;
    [SerializeField] private Animator animator;

    public Vector3 VecDir { get; private set; } = new(0, 0, 1);

    private Rigidbody _rb;
    private InputControls _inputs;
    private Vector3 _vecMove = Vector3.zero; // player movement direction
    private bool _bJumpUpdate = true;
    private bool _bIsGrounded;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Swing = Animator.StringToHash("Swing");
    private static readonly int Throw = Animator.StringToHash("Throw");
    private static readonly int Slam = Animator.StringToHash("Slam");
    private static readonly int Thrust = Animator.StringToHash("Thrust");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
        Vector3 pos = transform.position;
        pos.y = 0.01f;
        transform.position = pos;
        GroundDetectUpdate();
    }


    private void FixedUpdate()
    {
        if(animator.GetBool("canMove")) {
            MovementUpdate();
            GetComponent<PlayerWeapon>().DirectionCheck(animator.GetFloat(Horizontal), animator.GetFloat(Vertical));
        }
    }


    public void OnAttackPerformed(AttackType type)
    {
        switch (type)
        {
            case AttackType.Swing:
                animator.SetTrigger(Swing);
                break;
            case AttackType.Throwable:
                animator.SetTrigger(Throw);
                break;
            case AttackType.Lob:
                animator.SetTrigger(Throw);
                // animator.SetTrigger("Lob");
                break;
            case AttackType.Slam:
                animator.SetTrigger(Slam);
                break;
            case AttackType.Thrust:
                animator.SetTrigger(Thrust);
                break;
            case AttackType.Boomerang:
                animator.SetTrigger(Throw);
                // animator.SetTrigger("Boomerang");
                break;
        }
    }

    public void OnSetAttackDir(Vector2 dir)
    {
        dir = dir.normalized;
        animator.SetFloat(Horizontal, dir.x);
        animator.SetFloat(Vertical, dir.y);
    }

    #region Movement

    private Vector2 _inputMove;


    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        _inputMove = value.ReadValue<Vector2>();
        animator.SetFloat(Horizontal, _inputMove.x);
        animator.SetFloat(Vertical, _inputMove.y);
        animator.SetFloat(Speed, _inputMove.magnitude);
        // player moving direction is based on the view of camera
        Transform cameraTransform = Camera.main.transform;
        Vector3 vecFront = cameraTransform.forward;
        vecFront.y = 0;
        vecFront = vecFront.normalized;
        Vector3 vecRight = cameraTransform.right;
        vecRight.y = 0;
        vecRight = vecRight.normalized;
        Vector3 vecIsoMove = vecRight * _inputMove.x + vecFront * _inputMove.y;
        _vecMove = vecIsoMove.normalized;
        VecDir = _vecMove;
    }

    private void OnMovementCanceled(InputAction.CallbackContext value)
    {
        _vecMove = Vector3.zero;
        animator.SetFloat(Speed, 0f);
    }

    private void MovementUpdate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Movement"))
        {
            animator.SetFloat(Horizontal, _inputMove.x);
            animator.SetFloat(Vertical, _inputMove.y);
        }
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


    public void SprintMove(Vector3 targetPos, float time)
    {
        Vector3 startPos = transform.position;
        StartCoroutine(SprintMoveUpdateCor(startPos, targetPos, time));
    }
    
    public void SetPlayerAttackPosition()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, lmGroundLayer))
            {
                // The ray hit an object on the ground layer
                Vector3 hitGround = hit.point;
                hitGround.y = 0f;
                Vector3 playerPos = transform.position;
                playerPos.y = 0f;
                Vector3 dir = (hitGround - playerPos).normalized;
                Vector2 attackDir = new Vector2(dir.x, dir.z);
                // rotate counterclockwise because this is the isometric view
                attackDir = RotateVector(attackDir, 45);
                OnSetAttackDir(attackDir);
            }
        }
    }
    
    private Vector2 RotateVector(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
    }


    IEnumerator SprintMoveUpdateCor(Vector3 startPos, Vector3 targetPos, float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.deltaTime;
            float lerpFactor = t / time;
            transform.position = Vector3.Lerp(startPos, targetPos, lerpFactor);
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fGroundCheckRadius);
    }
}