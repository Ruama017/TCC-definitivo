using UnityEngine;
using System.Collections;

public class PulmaoDeMorteJr : MonoBehaviour
{
    [Header("Referências")]
    public ParticleSystem gasParticles; // arrasta aqui o sistema de partículas do gás

    [Header("Configurações de Efeito")]
    public int damage = 1;
    public float slowMultiplier = 0.5f; // player fica com metade da velocidade
    public float effectDuration = 3f; // tempo que o player fica afetado
    public Color effectColor = new Color(1f, 0.5f, 0.5f, 1f); // cor do player durante o efeito

    [Header("Ciclo do Gás")]
    public float gasOnTime = 4f;  // tempo soltando gás
    public float gasOffTime = 4f; // tempo sem soltar

    void Start()
    {
        StartCoroutine(GasEmissionRoutine());
    }

    IEnumerator GasEmissionRoutine()
    {
        while (true)
        {
            // Solta o gás por X segundos
            gasParticles.Play();
            yield return new WaitForSeconds(gasOnTime);

            // Para o gás e espera
            gasParticles.Stop();
            yield return new WaitForSeconds(gasOffTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ApplyDeathLungEffect(collision.gameObject));
        }
    }

    IEnumerator ApplyDeathLungEffect(GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        SpriteRenderer playerSprite = player.GetComponent<SpriteRenderer>();

        // aplica o dano
        if (playerHealth != null)
            playerHealth.TakeDamage(damage);

        // salva valores originais
        float originalSpeed = playerController.moveSpeed;
        Color originalColor = playerSprite.color;

        // aplica lentidão e muda a cor do player
        playerController.moveSpeed *= slowMultiplier;
        playerSprite.color = effectColor;

        yield return new WaitForSeconds(effectDuration);

        // retorna tudo ao normal
        playerController.moveSpeed = originalSpeed;
        playerSprite.color = originalColor;
    }
}
