using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPan : MonoBehaviour
{
    CinemachineVirtualCamera cineCam;
    CinemachineTransposer transposer;
    private bool panning = false;
    // Start is called before the first frame update
    void Start()
    {
        TutorialAEvents.current.cameraPan += PanIn;
    }

    private void PanIn()
    {
        cineCam = this.GetComponent<CinemachineVirtualCamera>();
        transposer = cineCam.GetCinemachineComponent<CinemachineTransposer>();
        panning = true;
    }

    void Update()
    {
        if(panning)
        {
            Vector3 endPan = transposer.m_FollowOffset;
            endPan.x = -3.5f;
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, endPan, 0.2f);
        }
    }


}
