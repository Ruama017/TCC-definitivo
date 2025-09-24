using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Referências")]
    public Transform target;       
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.125f;

    [Header("Limites Verticais")]
    public float minY = 0f;  // limite mínimo da câmera no eixo Y
    public float maxY = 10f; // limite máximo da câmera no eixo Y

    void Start()
    {
        if (target != null)
            transform.position = target.position + offset;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        // Aplica limite vertical
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}