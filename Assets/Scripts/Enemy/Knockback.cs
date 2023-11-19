using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    public Rigidbody rb;
    private float strength = 40, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;
    
    public void PlayFeedback(Vector3 direction)
    {
        StopAllCoroutines();
        OnBegin?.Invoke();
        //Vector3 direction = new Vector3(1f,0,1f);//(transform.position - sender.transform.position). normalized;
        rb.AddForce(direction * strength, ForceMode.Impulse);
        StartCoroutine(Reset());
    }
    
    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        OnDone?.Invoke();
    }
}
