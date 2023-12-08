using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickPlay : MonoBehaviour
{
    [SerializeField] private Sprite[] Titles;
    [SerializeField] private AudioSource[] SFX;
    private int i = 0;
    [SerializeField] private GameObject fade;
    [SerializeField] private CameraShake cam;

    private bool vidOver = false;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().sprite = Titles[i];
        i = 1;
        StartCoroutine(VidLength());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && vidOver)
        {
            if(i < 3)
            {
                CameraShake.Invoke();
                this.GetComponent<Image>().sprite = Titles[i];
                cam.shakeCoef *= 2;
                SFX[i-1].Play();
                i++;
            }
            else
            {
                CameraShake.Invoke();
                SFX[2].Play();
                this.GetComponent<Image>().sprite = Titles[3];
                fade.GetComponent<NextScene>().FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
    

    private IEnumerator VidLength()
    {
        yield return new WaitForSeconds(7f);
        vidOver = true;
    }
}
