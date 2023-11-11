using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCameraEffect
{
    /// <summary>
    /// Require object in tag [Wall] and contains a [Collider] for detection
    /// Detect the desire object's child GameObject with SpriteRenderer with [2d Sprite] material
    /// </summary>
    public class TransparencyFadeout : MonoBehaviour
    {
        [SerializeField] private Transform tPlayer;
        [SerializeField] [Range(0, 1)] private float fFadeAmount;
        private GameObject _objCollide;
        private SpriteRenderer[] _srs;
        private RaycastHit _hit;
        private static readonly int Transparency = Shader.PropertyToID("_Transparency");


        private Dictionary<SpriteRenderer, Coroutine> _threads = new();

        private void Update()
        {
            FadeoutUpdate();
        }

        private void FadeoutUpdate()
        {
            Debug.DrawLine(transform.position, tPlayer.position, Color.green);
            if (Physics.Linecast(transform.position, tPlayer.position, out _hit))
            {
                // TODO which one to set transparency should be considered
                if (_hit.collider.CompareTag("Wall"))
                {
                    if (_objCollide != _hit.collider.gameObject)
                    {
                        _objCollide = _hit.collider.gameObject;
                        if (_srs != null)
                        {
                            foreach (var sr in _srs)
                            {
                                if (_threads.TryGetValue(sr, out var thread1))
                                {
                                   StopCoroutine(thread1); 
                                }

                                Coroutine thread = StartCoroutine(Fade(sr.material, 1f, 0.2f, 20));
                                _threads[sr] = thread;
                            }

                            _srs = null;
                        }

                        _srs = _objCollide.GetComponentsInChildren<SpriteRenderer>();
                        foreach (var sr in _srs)
                        {
                            if (_threads.TryGetValue(sr, out var thread1))
                            {
                                StopCoroutine(thread1); 
                            }
                            Coroutine thread = StartCoroutine(Fade(sr.material, fFadeAmount, 0.2f, 20));
                            _threads[sr] = thread;
                        }
                    }
                }
                else
                {
                    _objCollide = null;
                    if (_srs != null)
                    {
                        foreach (var sr in _srs)
                        {
                            if (_threads.TryGetValue(sr, out var thread1))
                            {
                                StopCoroutine(thread1); 
                            }
                            Coroutine thread = StartCoroutine(Fade(sr.material, 1f, 0.2f, 20));
                            _threads[sr] = thread;
                        }

                        _srs = null;
                    }
                }
            }
        }
        
        IEnumerator Fade(Material mat, float alpha, float time, int sample)
        {
            float transparency = mat.GetFloat(Transparency);
            float timeDuration = time / sample;
            float alphaDur = (alpha - transparency) / sample;
            for (int i = 0; i < sample; i++)
            {
                float curAlpha = transparency + alphaDur * i;
                mat.SetFloat(Transparency, curAlpha);
                yield return new WaitForSeconds(timeDuration);
            }

            mat.SetFloat(Transparency, alpha);
        }
    }
}