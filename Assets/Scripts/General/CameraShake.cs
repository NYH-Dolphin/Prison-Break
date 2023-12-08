using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private RectTransform UI;
    [SerializeField] private Vector3 posStrength;

    public float shakeCoef;

    private static event Action Shake;

    public static void Invoke()
    {
        Shake?.Invoke();
    }

    private void OnEnable() =>Shake += CameraShaker;
    private void OnDisable() =>Shake -= CameraShaker;

    private void CameraShaker()
    {
        UI.DOComplete();
        UI.DOShakeAnchorPos(0.3f, shakeCoef * posStrength);
    }

}
