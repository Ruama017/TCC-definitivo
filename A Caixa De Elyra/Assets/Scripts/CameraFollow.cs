using UnityEngine;

public class CameraFollow : MonoBehaviour
{
<<<<<<< Updated upstream
    public Transform target; // Player
    public float smoothSpeed = 0.15f;
    public Vector2 minBounds; // limite mínimo (x,y)
    public Vector2 maxBounds; // limite máximo (x,y)
    public Vector3 offset;    // ajuste da posição da câmera
=======
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 0, -10);
>>>>>>> Stashed changes

    void LateUpdate()
    {
        if (target == null) return;

<<<<<<< Updated upstream
        // Posição desejada
        Vector3 desiredPosition = target.position + offset;

        // Suavização
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Limita a câmera dentro dos bounds
        float clampX = Mathf.Clamp(smoothedPosition.x, minBounds.x, maxBounds.x);
        float clampY = Mathf.Clamp(smoothedPosition.y, minBounds.y, maxBounds.y);

        // Atualiza posição
        transform.position = new Vector3(clampX, clampY, -10f);
=======
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
>>>>>>> Stashed changes
    }
}


<<<<<<< Updated upstream
=======


>>>>>>> Stashed changes
