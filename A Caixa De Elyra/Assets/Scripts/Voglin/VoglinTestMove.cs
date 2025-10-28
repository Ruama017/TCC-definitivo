using UnityEngine;

public class VoglinTestMove : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(2, 0); // move pra direita
    }
}
