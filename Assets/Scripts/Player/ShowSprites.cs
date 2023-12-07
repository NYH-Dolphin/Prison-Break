using UnityEngine;

public class ShowSprites : MonoBehaviour
{
    public bool showing = true;

    void Update()
    {
        if(transform.childCount > 0)
        {
            if(showing)
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            else
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
