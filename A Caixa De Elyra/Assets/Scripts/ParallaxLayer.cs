using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffect;

    private Vector3 startPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        startPosition = transform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        float deltaX = cameraTransform.position.x * parallaxEffect;
        transform.position = new Vector3(startPosition.x + deltaX, startPosition.y, startPosition.z);
    }
}





