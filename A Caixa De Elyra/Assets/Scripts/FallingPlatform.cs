using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [Header("Configurações")]
    public float fallDelay = 1f;      // tempo até sumir
    public float shakeAmount = 0.1f;  // intensidade da balançada
    public float shakeSpeed = 10f;    // velocidade da balançada
    public float respawnTime = 5f;    // tempo até voltar

    private Vector3 originalPosition;
    private bool playerOnPlatform = false;
    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    void Start()
    {
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !playerOnPlatform)
        {
            playerOnPlatform = true;
            StartCoroutine(ShakeAndDisappear());
        }
    }

    IEnumerator ShakeAndDisappear()
    {
        float elapsed = 0f;

        // Balança antes de sumir
        while(elapsed < fallDelay)
        {
            float xOffset = Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            transform.position = originalPosition + new Vector3(xOffset, 0, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Desaparece visualmente e desativa colisão
        spriteRenderer.enabled = false;
        platformCollider.enabled = false;

        // Espera respawn
        yield return new WaitForSeconds(respawnTime);

        // Volta à posição original
        transform.position = originalPosition;
        spriteRenderer.enabled = true;
        platformCollider.enabled = true;

        playerOnPlatform = false;
    }
}