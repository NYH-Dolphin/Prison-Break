using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkipTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex < 6)
        {
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Skip()
    {
        SceneManager.LoadScene(6);
    }
}
