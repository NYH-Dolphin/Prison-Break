using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemy : MonoBehaviour
{
    [SerializeField] private bool bDev;
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
        
        // hit 
        GetComponent<Rigidbody>().velocity = dir * 30f;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<PlayerWeapon>().enabled = false;
        weaponEffect.SetActive(false);
        playerBloodyEffect.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        
        // back to normal -> restart
        Time.timeScale = 1f;
        deadBloodEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}