using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FuseIndicator : MonoBehaviour
{
    public static FuseIndicator Instance;
    private bool fuse = false;
    [SerializeField] private GameObject uiFuse;

    private RectTransform _rt;
    private Vector3 _vRectUpPos;
    private Vector3 _vRectDownPos;
    private void Awake()
    {
        Instance = this;
        _rt = uiFuse.GetComponent<RectTransform>();
        Vector3 rtPos = _rt.anchoredPosition;
        _vRectUpPos = new Vector3(rtPos.x, 25, rtPos.z);
        _vRectDownPos = new Vector3(rtPos.x, -30f, rtPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(fuse)
            _rt.anchoredPosition = Vector3.Lerp(_rt.anchoredPosition, _vRectUpPos, 6f * Time.deltaTime);
        else
        {
            _rt.anchoredPosition = Vector3.Lerp(_rt.anchoredPosition, _vRectDownPos, 10f * Time.deltaTime);
        }
        // if(_rt.anchoredPosition.y <= (_vRectDownPos.y+10)) 
        //     uiFuse.GetComponent<TMP_Text>().text = "";
        // else
        //     uiFuse.GetComponent<TMP_Text>().text = "Fuse";
        
        // Debug.Log(_rt.anchoredPosition.y);
    }

    public void ShowFuse()
    {
        fuse = true;
    }

    public void HideFuse()
    {
        fuse = false;
    }
}
