using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            transform.GetChild(0).GetComponent<NextScene>().FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}