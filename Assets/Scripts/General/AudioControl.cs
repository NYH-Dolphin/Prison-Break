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
    [SerializeField] private AudioSource thrustSFX;
    [SerializeField] private AudioSource slamSFX;
    [SerializeField] private AudioSource surprisedSFX;
    [SerializeField] private AudioSource boomSFX;
    [SerializeField] private AudioSource dizzySFX;
    [SerializeField] private AudioSource pickupSFX;
    [SerializeField] private AudioSource fusionSFX;

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

    public void PlayThrust()
    {
        thrustSFX.Play();
    }

    public void PlayBoom()
    {
        boomSFX.Play();
    }

    public void PlaySlam()
    {
        slamSFX.Play();
    }

    public void PlaySurprised()
    {
        surprisedSFX.Play();
    }
    
    public void PlayPickup()
    {
        pickupSFX.Play();
    }

    public void PlayDizzy()
    {
        dizzySFX.Play();
    }

    public void PlayFusion()
    {
        fusionSFX.Play();
    }
}