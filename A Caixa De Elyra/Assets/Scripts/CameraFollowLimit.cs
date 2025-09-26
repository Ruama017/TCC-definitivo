using UnityEngine;

public class CameraFollowLimit : MonoBehaviour
{
    public Transform target;    // Player
    public Vector3 offset;      // Ajuste de posição da câmera (ex: (0,0,-10))
    public float smoothSpeed = 0.125f; // Suavização

    [Header("Limites da câmera")]
    public float minX; // início do cenário
    public float maxX; // fim do cenário

    void LateUpdate()
    {
        if(target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z) + offset;

        // Aplica limite horizontal
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);

        // Suaviza o movimento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;
    }
}
