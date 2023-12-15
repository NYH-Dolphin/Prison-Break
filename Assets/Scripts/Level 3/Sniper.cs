using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sniper : MonoBehaviour
{
    [SerializeField] private LineRenderer lr1, lr2;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float width;
    [SerializeField] private float closeTime;
    public GameObject playerHead;
    private bool load = false;
    private Vector3 offset = new Vector3(0,0,1);
    Color color1 = new Color(1,1,1,1);
    Color color2 = new Color(1,1,1,1);
    public bool active = true;

    void Update()
    {
        if(active)
        {
            if(offset.z <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            lr1.SetPosition(0, transform.GetChild(0).position + offset);
            lr2.SetPosition(0, transform.GetChild(1).position - offset);
            Vector3 direction = playerHead.transform.position - this.transform.position;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(playerHead.transform.position, transform.position), layer))
            {
                lr1.SetPosition(1, hit.point + offset);
                lr2.SetPosition(1, hit.point - offset);
                SniperRecover();
            }
            else
            {
                lr1.SetPosition(1,playerHead.transform.position + offset);
                lr2.SetPosition(1,playerHead.transform.position - offset);
                SniperClose();
            }
        }
        
    }

    void SniperRecover()
    {
        if(offset.z < 1f)
        {
            offset += new Vector3(0,0, Time.deltaTime);
            color2 += new Color(0, Time.deltaTime, Time.deltaTime, - Time.deltaTime);
            color1 += new Color(0, Time.deltaTime, Time.deltaTime, - Time.deltaTime);
            lr1.SetColors(color1, color2);
            lr2.SetColors(color1, color2);
        }
    }
    void SniperClose()
    {
        offset -= new Vector3(0,0,0.3f *Time.deltaTime);
        color2 -= new Color(0, closeTime *Time.deltaTime, closeTime *Time.deltaTime, -closeTime *Time.deltaTime);
        color1 -= new Color(0, closeTime *Time.deltaTime, closeTime *Time.deltaTime, -closeTime *Time.deltaTime);
        lr1.SetColors(color1, color2);
        lr2.SetColors(color1, color2);
    }

    
    // void Update()
    // {
    //     lr.SetPosition(0, this.transform.position);
    //     Vector3 direction = playerHead.transform.position - this.transform.position;
    //     RaycastHit hit;
    //     if(Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(playerHead.transform.position, transform.position), layer))
    //     {
    //         lr.SetPosition(1, hit.point);
    //         SniperRecover();
    //     }
    //     else
    //     {
    //         lr.SetPosition(1,playerHead.transform.position);
    //         SniperClose();
    //     }
    // }

    // void SniperClose()
    // {
    //     if(width > 0.2f)
    //     {
    //         if(width < 0.8f)
    //             lr.SetColors(new Color(1, .8f, .8f), new Color(1, .8f, .8f));
    //         else if(width < 0.6f)
    //             lr.SetColors(new Color(1, .5f, .5f), new Color(1, .5f, .5f));
    //         else if(width < 0.4f)
    //             lr.SetColors(new Color(1, .2f, .2f), new Color(1, .2f, .2f));
    //         lr.SetWidth(width, width);
    //         width -= 0.28f * Time.deltaTime;
    //     }
    //     else
    //     {
    //         lr.SetColors(Color.red, Color.red);
    //         StartCoroutine(Killer());
    //         if(load) 
    //             SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //     }
    // }

    // void SniperRecover()
    // {
    //     if(width < 1f)
    //     {
    //         load = false;
    //         lr.SetColors(Color.white, Color.white);
    //         lr.SetWidth(width, width);
    //         width += Time.deltaTime;
    //     }
    // }

    // private IEnumerator Killer()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     load = true;
    // }


}
