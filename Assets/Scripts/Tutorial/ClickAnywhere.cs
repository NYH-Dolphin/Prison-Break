using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAnywhere : MonoBehaviour
{
    private Vector3 endPosition;
    private RectTransform rt;
    private bool vidOver = false;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        endPosition = rt.anchoredPosition;
        endPosition.y = -84f;
        StartCoroutine(VidLength());
    }
    // Start is called before the first frame update
    void Update()
    {
        if(vidOver)
            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, endPosition, 5f * Time.deltaTime);
    }

    private IEnumerator VidLength()
    {
        yield return new WaitForSeconds(7f);
        vidOver = true;
    }
}
