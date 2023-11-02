using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fMovementSpeed = 10f;
    [SerializeField] private float fJumpSpeed = 10f;
    
    private Rigidbody _rb;
    
    
    
    
    // private InputControls _controls;
    // private Vector2 _inputMovement;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // if (_controls == null)
        // {
        //     _controls = new InputControls();
        // }
        //
        // _controls.gameplay.Enable();
    }

    public void OnDisable()
    {
        // _controls.gameplay.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // OnMove();
        // OnJump();
    }

    public void OnMove()
    {
        // Vector2 movementInput = _controls.gameplay.Move.ReadValue<Vector2>().normalized;
        // Vector3 movementVelocity = new Vector3(movementInput.x, 0f, movementInput.y) * fMovementSpeed;
        // _rb.AddForce(movementVelocity);
    }

    public void OnJump()
    {
        // if (_controls.gameplay.Jump.IsPressed())
        // {
        //     _rb.AddForce(Vector3.up * fJumpSpeed, ForceMode.Impulse);
        // }
    }

    public void OnWeapon(InputAction.CallbackContext context)
    {
    }
}