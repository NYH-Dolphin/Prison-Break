using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main != null) transform.rotation = Camera.main.transform.rotation;
    }
}