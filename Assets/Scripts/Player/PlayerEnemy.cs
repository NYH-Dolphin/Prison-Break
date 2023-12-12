using UnityEngine;
using UnityEngine.SceneManagement;
using Weapon;
using Enemy;
using Player;

public class PlayerEnemy : MonoBehaviour
{
    [SerializeField] private bool bDev;
    [SerializeField] private Animator anim;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.CompareTag("hitbox"))
        {
            if (bDev) return;
            GameObject detected = GetComponent<PlayerWeapon>().EnemyDetected;
            if(anim.GetBool("attacking") && detected != null) 
            {
                Debug.Log("heyyo");
                if(detected == other.gameObject)
                    return;
            }
 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
    }
}