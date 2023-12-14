using System.Collections;
using UnityEngine;

namespace Effects
{
    public class LobEffect : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DestroySelf());
        }

        IEnumerator DestroySelf()
        {
            yield return new WaitForSeconds(3f);
            Destroy(this);
        }
    }
}