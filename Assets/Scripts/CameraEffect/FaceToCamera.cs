using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private Camera _camera;
    private float _eulerAngleZ;
    [SerializeField] private float offset;

    private void Start()
    {
        _camera = Camera.main;
        _eulerAngleZ = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        Vector3 rotation = _camera.transform.rotation.eulerAngles;
        rotation.z = _eulerAngleZ;
        rotation.y += offset;
        transform.rotation = Quaternion.Euler(rotation);


    }
}