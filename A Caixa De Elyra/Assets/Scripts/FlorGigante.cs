using UnityEngine;
using System.Collections;

public class FlorGigante : MonoBehaviour
{
    [Header("Referências")]
    public ParticleSystem waterParticles; // arrasta aqui o sistema de partículas da flor

    [Header("Configurações de Efeito")]
    public int damage = 1; // dano que o player leva
    public float slowMultiplier = 0.7f; // reduz levemente a velocidade do player
    public float effectDuration = 3f; // tempo que o efeito dura
    public Color effectColor = new Color(0.6f, 0.8f, 1f, 1f); // cor do player durante o efeito (ex: azulada)

    [Header("Ciclo da Flor")]
    public float particlesOnTime = 5f;  // tempo soltando partículas
    public float particlesOffTime = 5f; // tempo sem soltar

    void Start()
    {
        StartCoroutine(WaterEmissionRoutine());
    }

    IEnumerator WaterEmissionRoutine()
    {
        while (true)
        {
            // Solta partículas rápidas (tipo jorro d’água)
            var main = waterParticles.main;
            main.simulationSpeed = 1.8f; // acelera as partículas
            waterParticles.Play();
            yield return new WaitForSeconds(particlesOnTime);

            // Pausa a emissão
            waterParticles.Stop();
            yield return new WaitForSeconds(particlesOffTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ApplyFlowerDamage(collision.gameObject));
        }
    }

    IEnumerator ApplyFlowerDamage(GameObject player)
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
