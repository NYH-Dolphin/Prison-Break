using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Camera.main != null) transform.rotation = Camera.main.transform.rotation;
    }
}