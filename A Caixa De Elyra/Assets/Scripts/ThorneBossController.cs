using UnityEngine;
using System.Collections;

public class ThorneBossController : MonoBehaviour
{
    [Header("Configuração")]
    public float moveSpeed = 3f;
    public float attackRange = 2f;
    public int maxHealth = 10;

    private Transform player;
    private Animator anim;
    private int currentHealth;

    private bool isAttacking = false;
    private bool isDead = false;

    private bool teleportBehindNext = true; // alterna atrás → frente → atrás...

    [Header("Ataque da Espada")]
    public Collider2D swordHitbox;       // collider da espada
    public int swordDamage = 1;
    public float swordHitboxDelay = 0.3f;     
    public float swordHitboxDuration = 0.25f; 

    // ----------------------------------------------------------
    // Função para ativar o Thorne após NitroMortis morrer
    // ----------------------------------------------------------
    public void ActivateThorne()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        isDead = false;
        isAttacking = false;

        if (swordHitbox != null)
            swordHitbox.enabled = false; // garante que a hitbox começa desativada

        StartCoroutine(TeleportCycle());
    }

    void Update()
    {
        if (isDead || player == null) return;

        MoveAndAttack();
        FlipTowardsPlayer();
    }

    // ----------------------------------------------------------
    // MOVIMENTAÇÃO + ATAQUE
    // ----------------------------------------------------------
    void MoveAndAttack()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            anim.SetBool("isWalking", true);

            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isWalking", false);

            if (!isAttacking)
                StartCoroutine(Attack());
        }
    }

    // ----------------------------------------------------------
    // ATAQUE COM HITBOX CORRIGIDA
    // ----------------------------------------------------------
    IEnumerator Attack()
    {
        isAttacking = true;

        anim.SetTrigger("attack");

        // espera até o momento da espada acertar
        yield return new WaitForSeconds(swordHitboxDelay);

        // Ativa apenas o collider (GameObject deve estar ativo)
        if (swordHitbox != null)
            swordHitbox.enabled = true;

        yield return new WaitForSeconds(swordHitboxDuration);

        if (swordHitbox != null)
            swordHitbox.enabled = false;

        yield return new WaitForSeconds(0.2f); // restante da animação

        isAttacking = false;
    }

    // ----------------------------------------------------------
    // CICLO DE TELEPORTE
    // ----------------------------------------------------------
    IEnumerator TeleportCycle()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(15f);

            anim.SetTrigger("teleport"); 

            yield return new WaitForSeconds(0.1f);

            Vector3 newPos;

            if (teleportBehindNext)
            {
                float offset = -2f * Mathf.Sign(player.position.x - transform.position.x);
                newPos = player.position + new Vector3(offset, 0, 0);
            }
            else
            {
                float offset = 2f * Mathf.Sign(player.position.x - transform.position.x);
                newPos = player.position + new Vector3(offset, 0, 0);
            }

            transform.position = newPos;

            teleportBehindNext = !teleportBehindNext;

            yield return new WaitForSeconds(0.1f);
        }
    }

    // ----------------------------------------------------------
    // FLIP (SPRITE VIRADO PARA ESQUERDA)
    // ----------------------------------------------------------
    void FlipTowardsPlayer()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);  // olha para esquerda
        else
            transform.localScale = new Vector3(-1, 1, 1); // olha para direita
    }

    // ----------------------------------------------------------
    // DANO E MORTE
    // ----------------------------------------------------------
    public void TakeDamage(int damage, bool playerHasSuper)
    {
        if (!playerHasSuper || isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("Hit");

        if (currentHealth <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        isDead = true;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
