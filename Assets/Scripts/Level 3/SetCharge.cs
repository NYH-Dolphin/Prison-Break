using GameInputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetCharge : MonoBehaviour
{
    public GameObject Explosions;
    [SerializeField] private GameObject[] message;
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
            if(Explosions.GetComponent<ExplosionTrigger>().charges == 0)
            {
                message[1].SetActive(true);
            }
            else
            {
                message[2].SetActive(true);
            }
            message[0].SetActive(false);
                

            Explosions.GetComponent<ExplosionTrigger>().charges++;
            GetComponent<Collider>().enabled = false;
            _player = null;
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