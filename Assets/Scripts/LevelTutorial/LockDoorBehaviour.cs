using UnityEngine;

namespace LevelTutorial
{
    public class LockDoorBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioSource openSFX;
        private bool _bRotate;
        public static LockDoorBehaviour Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void OnOpenDoor()
        {
            openSFX.Play();
            _bRotate = true;
        }

        private void Update()
        {
            if (_bRotate)
            {
                Vector3 endPoint = transform.eulerAngles;
                endPoint.y = 0;
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, endPoint, 0.02f);
            }
        }
    }
}