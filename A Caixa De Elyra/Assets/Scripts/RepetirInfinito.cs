using UnityEngine;

public class RepetirInfinito : MonoBehaviour
{
    public float larguraSprite = 10f; // largura do sprite em Unity units
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (Camera.main.transform.position.x - startPosition.x >= larguraSprite)
        {
            startPosition.x += larguraSprite;
            transform.position = startPosition;
        }
    }
}
