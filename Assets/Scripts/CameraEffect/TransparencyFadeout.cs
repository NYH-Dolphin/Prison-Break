using UnityEngine;

namespace CameraEffect
{
    /// <summary>
    /// Require object in tag [Wall] and contains a [Collider] for detection
    /// Detect the desire object's child GameObject with SpriteRenderer with [2d Sprite] material
    /// </summary>
    public class TransparencyFadeout : MonoBehaviour
    {
        [SerializeField] private Transform tPlayer;
        private SpriteRenderer[] _srs;
        private RaycastHit _hit;
        private static readonly int Transparency = Shader.PropertyToID("_Transparency");

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
                    GameObject objHit = _hit.collider.gameObject;
                    _srs = objHit.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var sr in _srs)
                    {
                        sr.material.SetFloat(Transparency, 0.5f);
                    }
                }
                else
                {
                    if (_srs != null)
                    {
                        foreach (var sr in _srs)
                        {
                            sr.material.SetFloat(Transparency, 1f);
                        }
                        _srs = null;
                    }
                }
            }
        }
    }
}