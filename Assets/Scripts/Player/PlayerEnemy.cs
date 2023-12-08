using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemy : MonoBehaviour
{
    [SerializeField] private bool bDev;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && other.CompareTag("hitbox"))
        {
            if (bDev) return;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}