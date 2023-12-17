using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace General
{
    public class Ending : MonoBehaviour
    {
        [SerializeField] private VideoPlayer video;

        private void Update()
        {
            if (!video.isPlaying)
            {
                SceneManager.LoadScene("Opening Scene");
            }

            if (Input.anyKey)
            {
                SceneManager.LoadScene("Opening Scene");
            }
        }
    }
}