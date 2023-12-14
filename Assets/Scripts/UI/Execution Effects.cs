using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExecutionEffects : MonoBehaviour
{
    public static ExecutionEffects Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Image>().color.a > 0)
        {
            var color = GetComponent<Image>().color;
            color.a -= Time.deltaTime;
            GetComponent<Image>().color = color;
            foreach(Transform child in transform)
            {
                var childColor = child.GetComponent<Image>().color;
                childColor.a -= Time.deltaTime;
                child.GetComponent<Image>().color = childColor;
            }
        }
    }

    public void Execution()
    {
        var color = GetComponent<Image>().color;
        color.a = 0.8f;
        GetComponent<Image>().color = color; 
        foreach(Transform child in transform)
        {
            child.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-300,300), Random.Range(-150,150));
            var childColor = child.GetComponent<Image>().color;
            childColor.a = 0.8f;
            child.GetComponent<Image>().color = childColor;
        }
    }
}
