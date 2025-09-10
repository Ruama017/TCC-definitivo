using UnityEngine;

public class GroundInfinite : MonoBehaviour
{
    public Transform cam;        
    private float spriteWidth;   
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float dist = cam.position.x - startPosition.x;

        if(dist >= spriteWidth)
        {
            startPosition.x += spriteWidth;
            transform.position = startPosition;
        }
        else if(dist <= -spriteWidth)
        {
            startPosition.x -= spriteWidth;
            transform.position = startPosition;
        }
    }
}

