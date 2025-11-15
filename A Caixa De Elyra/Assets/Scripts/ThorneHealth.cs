using UnityEngine;
using System.Collections;

public class ThorneHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public SpriteRenderer spriteRenderer; // arraste o Sprite do Thorne
    public AudioSource deathSound;        // som de morte do Thorne
    public float fadeDuration = 2f;       // tempo para desaparecer

    private bool dead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (dead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            StartCoroutine(DieEffect());
        }
    }

    IEnumerator DieEffect()
    {
        dead = true;

        if (deathSound != null)
            deathSound.Play();

        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            // pisca
            float alpha = Mathf.PingPong(elapsed * 5f, 1f); // piscando rápido
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // desaparece totalmente
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // desativa IA e colisão
        GetComponent<ThorneBossController>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // destrói objeto depois de 0.5s
        Destroy(gameObject, 0.5f);
    }
}
