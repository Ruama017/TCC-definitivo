using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffect = 0; // fundo liso = 0
    private Vector3 lastCameraPosition;

    void Start()
    {
        if(cameraTransform == null) cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxEffect, delta.y * parallaxEffect, 0);
        lastCameraPosition = cameraTransform.position;
    }
}

