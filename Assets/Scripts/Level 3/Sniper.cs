using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sniper : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float width;
    public GameObject playerHead;
    private bool load = false;
    

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }
    
    void Update()
    {
        lr.SetPosition(0, this.transform.position);
        Vector3 direction = playerHead.transform.position - this.transform.position;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(playerHead.transform.position, transform.position), layer))
        {
            lr.SetPosition(1, hit.point);
            SniperRecover();
        }
        else
        {
            lr.SetPosition(1,playerHead.transform.position);
            SniperClose();
        }
    }

    void SniperClose()
    {
        if(width > 0.2f)
        {
            lr.SetWidth(width, width);
            width -= 0.35f * Time.deltaTime;
        }
        else
        {
            lr.SetColors(Color.red, Color.red);
            StartCoroutine(Killer());
            if(load) 
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void SniperRecover()
    {
        if(width < 1f)
        {
            load = false;
            lr.SetColors(Color.white, Color.white);
            lr.SetWidth(width, width);
            width += Time.deltaTime;
        }
    }

    private IEnumerator Killer()
    {
        yield return new WaitForSeconds(0.5f);
        load = true;
    }


}
