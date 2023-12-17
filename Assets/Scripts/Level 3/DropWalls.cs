using System;
using Enemy;
using MyCameraEffect;
using UnityEngine;

namespace Level_3
{
    public class DropWalls : MonoBehaviour
    {
        [SerializeField] private float _fDropTime = 2f;
        private float _fTime;
        private float _fShake = 0.2f;
        private float _fShakeTime;
        private int childBricks;
        public AudioSource asDrop;

        private void Update()
        {
            _fShakeTime += Time.deltaTime;
            if (_fShakeTime > _fShake)
            {
                CameraEffect.Instance.GenerateSmallImpulse();
                _fShakeTime = 0;
            }


            _fTime += Time.deltaTime;
            
            
            if (_fTime > _fDropTime)
            {
                _fTime = 0;
                if (childBricks < transform.childCount)
                {
                    transform.GetChild(childBricks).GetChild(0).GetComponent<Animator>().Play("Brick");
                    transform.GetChild(childBricks).GetChild(0).GetComponent<WallBehaviour>().bActivate = true;
                    CameraEffect.Instance.GenerateBumpImpulse();
                    if (_fDropTime > 0.2f) 
                        _fDropTime -= 0.08f;
                    asDrop.Play();
                    childBricks++;
                }
            }
        }
    }
}