using UnityEngine;

public class RepetirInfinito : MonoBehaviour
{
    private float larguraSprite;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        // Calcula a largura real do sprite (baseado no tamanho e escala)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        larguraSprite = sr.bounds.size.x;
    }

    void Update()
    {
        // Se a câmera passar do sprite pela direita
        if (cam.transform.position.x > transform.position.x + larguraSprite)
        {
            transform.position = new Vector3(
                transform.position.x + 2 * larguraSprite, // joga pra frente
                transform.position.y,
                transform.position.z
            );
        }
    }
}
