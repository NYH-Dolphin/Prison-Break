using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelTutorial
{
    public class RefreshPlayerPref : MonoBehaviour
    {
        [SerializeField] private List<string> playerPrefs;
        private void Awake()
        {
            foreach (var str in playerPrefs)
            {
                PlayerPrefs.SetInt(str, 0);
            }
        }
    }
}