using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffect;

    private Vector3 startPosition;
    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        startPosition = transform.position;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Calcula quanto a câmera se moveu desde o último frame
        Vector3 delta = cameraTransform.position - lastCameraPosition;

        // Aplica o parallax proporcional
        transform.position += new Vector3(delta.x * parallaxEffect, delta.y * parallaxEffect, 0);

        // Atualiza a última posição da câmera
        lastCameraPosition = cameraTransform.position;
    }
}





