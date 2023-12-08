using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetCharge : MonoBehaviour
{
    public GameObject Explosions;
    private InputControls _inputs;
    private GameObject _player;

    private void OnEnable()
    {
        if (_inputs == null)
        {
            _inputs = new InputControls();
        }

        _inputs.Gameplay.Object.Enable();
        _inputs.Gameplay.Object.performed += OnObjectPerformed;
    }

    private void OnDisable()
    {
        _inputs.Gameplay.Object.Disable();
        _inputs.Gameplay.Object.performed -= OnObjectPerformed;
    }


    private void OnObjectPerformed(InputAction.CallbackContext value)
    {
        if (_player != null)
        {
            Explosions.GetComponent<ExplosionTrigger>().charges++;
            GetComponent<Collider>().enabled = false;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player") //CHANGE TO E INPUT IN INPUT SYSTEM
        {
            _player = col.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") //CHANGE TO E INPUT IN INPUT SYSTEM
        {
            _player = null;
        }
    }
}