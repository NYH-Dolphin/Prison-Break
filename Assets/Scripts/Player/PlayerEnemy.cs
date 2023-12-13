using System;
using System.Collections;
using Cinemachine;
using MyCameraEffect;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerEnemy : MonoBehaviour
{
    [SerializeField] private bool bDev;
    [SerializeField] private CinemachineVirtualCameraBase mainCam;
    [SerializeField] private CinemachineVirtualCameraBase deadCam;
    [SerializeField] private GameObject playerBloodyEffect;
    [SerializeField] private GameObject deadBloodEffect;
    [SerializeField] private GameObject weaponEffect;


    private IEnumerator _thread;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.CompareTag("hitbox"))
        {
            if (bDev) return;
            if (_thread == null)
            {
                Vector3 hitDir = transform.position - other.gameObject.transform.position;
                hitDir.y = 0f;
                hitDir.Normalize();
                _thread = OnGetKilled(hitDir);
                StartCoroutine(_thread);
            }
        }
    }


    IEnumerator OnGetKilled(Vector3 dir)
    {
        // slow motion
        Time.timeScale = 0.1f;

        // camera effect
        if (mainCam != null && deadCam != null)
        {
            mainCam.VirtualCameraGameObject.SetActive(false);
            deadCam.VirtualCameraGameObject.SetActive(true);
        }

        if (DeadCameraEffect.Instance)
        {
            DeadCameraEffect.Instance.GetComponent<DeadCameraEffect>().OnTriggerDeadEffect();

        }
        

        // hit 
        GetComponent<Rigidbody>().velocity = dir * 30f;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerWeapon>().enabled = false;
        weaponEffect.SetActive(false);
        playerBloodyEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        // back to normal -> restart
        Time.timeScale = 1f;
        deadBloodEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}