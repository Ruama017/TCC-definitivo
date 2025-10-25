using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public bool ground = true;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private bool facingRight = true; // Declarando a variável

    void Update()
    {
        // Move o inimigo
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Verifica se há chão à frente
        ground = Physics2D.Linecast(groundCheck.position, transform.position, groundLayer);
        Debug.Log(ground);

        // Inverte direção se não houver chão
        if (!ground)
        {
            speed *= -1;
        }

        // Verifica se precisa virar a sprite
        if (speed > 0 && !facingRight)
        {
            Flip();
        }
        else if (speed < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Inverte o eixo X
        transform.localScale = scale;
    }
}
