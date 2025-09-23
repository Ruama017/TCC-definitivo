using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = 0f;
        if (Input.GetKey(KeyCode.Home)) move = -1f; // esquerda
        if (Input.GetKey(KeyCode.End)) move = 1f;   // direita

        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);
    }
}