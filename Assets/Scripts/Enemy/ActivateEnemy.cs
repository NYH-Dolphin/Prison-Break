using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI;

public class ActivateEnemy : MonoBehaviour
{
    public UIWeapon UI;
    public ViewCone view;

    // Update is called once per frame
    void Update()
    {
        if(UI.spriteName != "metal_pipe-brick")
        {
            view.active = false;
        }
        else{
            view.active = true;
        }
    }
}
