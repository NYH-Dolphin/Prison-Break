using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoNotDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string tag;
    Scene awakeScene;
    void Awake()
    {
        awakeScene = SceneManager.GetActiveScene();
        GameObject[] Obj = GameObject.FindGameObjectsWithTag(tag);
        if(Obj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene() != awakeScene)
        {
            Destroy(this.gameObject);
        }
    }
}
