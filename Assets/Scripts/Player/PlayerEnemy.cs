using System;
using System.Collections;
using Cinemachine;
using MyCameraEffect;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerEnemy))]
public class PlayerEnemy : MonoBehaviour
{
    [SerializeField] private bool bDev;
    [SerializeField] private CinemachineVirtualCameraBase mainCam;
    [SerializeField] private CinemachineVirtualCameraBase deadCam;
    [SerializeField] private GameObject playerBloodyEffect;
    [SerializeField] private GameObject deadBloodPrefab;
    [SerializeField] private GameObject weaponEffect;

    private PlayerController _pc;
    private IEnumerator _thread;


    private void Awake()
    {
        _pc = GetComponent<PlayerController>();
    }

    public void Kill(bool sniper = false)
    {
        if (bDev) return;
        if (_thread == null)
        {
            Vector3 hitDir = new Vector3(1, 1, 1);
            hitDir.y = 0f;
            hitDir.Normalize();
            _thread = OnGetKilled(hitDir, sniper);
            StartCoroutine(_thread);
        }
    }

    void OnTriggerEnter(Collider other)
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


    IEnumerator OnGetKilled(Vector3 dir, bool sniper = false)
    {
        // player dead animation
        _pc.OnSetAttackDir(new Vector2(dir.z, dir.x));
        _pc.OnPlayerDead();

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
        if (sniper) AudioControl.Instance.PlaySniper();
        AudioControl.Instance.PlayPlayerDead();
        Vector3 pos = new Vector3(transform.position.x, 0.1f, transform.position.z) + dir * 6f;
        yield return new WaitForSeconds(0.2f);
        Instantiate(deadBloodPrefab, pos, Quaternion.Euler(new Vector3(90, 0, 0)));
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}