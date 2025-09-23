using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Player
    public float smoothSpeed = 0.125f; // Velocidade da suavização
    public Vector3 offset = new Vector3(0, 0, -10); // Ajuste da posição da câmera

    [Header("Limites da câmera (opcional)")]
    public Vector2 minBounds; // Limite mínimo (x,y)
    public Vector2 maxBounds; // Limite máximo (x,y)

    void LateUpdate()
    {
        if (target == null) return;

        // Posição desejada
        Vector3 desiredPosition = target.position + offset;

        // Suavização
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Limita a câmera dentro dos bounds, se quiser usar
        float clampX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
        float clampY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);

        // Atualiza posição da câmera
        transform.position = new Vector3(clampX, clampY, smoothedPosition.z);
    }
}

