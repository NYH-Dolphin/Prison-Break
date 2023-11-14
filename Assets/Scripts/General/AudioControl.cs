using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    [SerializeField] private AudioSource swingSFX;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource throwSFX;
    [SerializeField] private AudioSource lobSFX;

    public static AudioControl Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySwing()
    {
        swingSFX.Play();
    }

    public void PlayHit()
    {
        hitSFX.Play();
    }

    public void PlayThrow()
    {
        throwSFX.Play();
    }

    public void PlayLob()
    {
        lobSFX.Play();
    }
}
