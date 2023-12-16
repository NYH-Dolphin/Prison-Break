using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyCameraEffect;

public class Explode : MonoBehaviour
{

    [SerializeField] private GameObject openTrigger;
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(Explosion());
        
    }

    private IEnumerator Explosion()
    {
        
        yield return new WaitForSeconds(1);
        AudioControl.Instance.PlayExplodeDistant();
        CameraEffect.Instance.GenerateMeleeImpulseWithVelocity(new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3)));
        yield return new WaitForSeconds(.1f);
        CameraEffect.Instance.GenerateMeleeImpulseWithVelocity(new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3)));
        yield return new WaitForSeconds(.1f);
        CameraEffect.Instance.GenerateMeleeImpulseWithVelocity(new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3)));
        yield return new WaitForSeconds(.1f);
        CameraEffect.Instance.GenerateMeleeImpulseWithVelocity(new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3)));
        yield return new WaitForSeconds(.1f);
        CameraEffect.Instance.GenerateMeleeImpulseWithVelocity(new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3)));
        yield return new WaitForSeconds(.4f);
        openTrigger.SetActive(true);

    }
}
