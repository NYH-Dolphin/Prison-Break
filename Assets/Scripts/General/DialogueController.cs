using System.Collections.Generic;
using UnityEngine;

namespace General
{
    public class DialogueController : MonoBehaviour
    {
        public List<GameObject> dialogues;
        public string levelName;

        private bool bDev = false;

        private void Awake()
        {
            if (!bDev)
            {
                if (PlayerPrefs.GetInt(levelName) == 1)
                {
                    foreach (var obj in dialogues)
                    {
                        obj.SetActive(false);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(levelName, 1);
                }
            }
        }
    }
}