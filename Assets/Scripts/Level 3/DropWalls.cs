﻿using System;
using Enemy;
using MyCameraEffect;
using UnityEngine;

namespace Level_3
{
    public class DropWalls : MonoBehaviour
    {
        [SerializeField] private float _fDropTime = 2f;
        private float _fTime;
        private int childBricks;
        public AudioSource asDrop;

        private void Update()
        {
            _fTime += Time.deltaTime;
            if (_fTime > _fDropTime)
            {
                _fTime = 0;
                if (childBricks < transform.childCount)
                {
                    transform.GetChild(childBricks).GetChild(0).GetComponent<Animator>().Play("Brick");
                    transform.GetChild(childBricks).GetChild(0).GetComponent<WallBehaviour>().bActivate = true;
                    CameraEffect.Instance.GenerateBumpImpulse();
                    asDrop.Play();
                    childBricks++;
                }
            }
        }
    }
}