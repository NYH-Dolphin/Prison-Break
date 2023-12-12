using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Effects
{
    public class BloodyEffect : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private void Start()
        {
            int index = Random.Range(1, 7);
            animator.Play($"Bloody{index}");
        }
    }
}