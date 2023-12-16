using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using UnityEngine.SceneManagement;

public class ACheck : MonoBehaviour
{

    public GameObject enemy;
    public GameObject weapon;
    public GameObject message;

    // Update is called once per frame
    void Update()
    {
        if(enemy != null)
        {
            Debug.Log(enemy.GetComponent<EnemyBehaviour>().notStunned);
            if(enemy.GetComponent<EnemyBehaviour>().notStunned && weapon == null)
            {
                message.SetActive(true);
                StartCoroutine(Reload());
            }
        }
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
