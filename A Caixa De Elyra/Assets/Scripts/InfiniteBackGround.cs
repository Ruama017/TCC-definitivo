using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    public Transform cam;           // A câmera ou jogador
    public float spriteWidth;       // Largura do sprite em unidades
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;

        // Se spriteWidth não for definido, pega do SpriteRenderer
        if(spriteWidth == 0)
        {
            spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }

    void Update()
    {
        float temp = cam.position.x - startPosition.x;

        // Quando a câmera passar metade da largura da imagem, reposiciona
        if (temp >= spriteWidth)
        {
            startPosition.x += spriteWidth;
            transform.position = startPosition;
        }
        else if (temp <= -spriteWidth)
        {
            startPosition.x -= spriteWidth;
            transform.position = startPosition;
        }
    }
}

