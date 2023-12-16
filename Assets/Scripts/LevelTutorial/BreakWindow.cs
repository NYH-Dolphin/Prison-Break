using System;
using UnityEngine;

namespace LevelTutorial
{
    public class BreakWindow : MonoBehaviour
    {
        private void OnDestroy()
        {
            AudioControl.Instance.PlayGlass();
        }
    }
}