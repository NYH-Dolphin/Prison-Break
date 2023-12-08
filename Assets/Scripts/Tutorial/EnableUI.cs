using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableUI : MonoBehaviour
{
    [SerializeField] private GameObject rawimage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(VidLength());
    }


    private IEnumerator VidLength()
    {
        yield return new WaitForSeconds(7f);
        rawimage.SetActive(false);
    }
}
