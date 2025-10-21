using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VoglinController : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;                // arraste o Player aqui no inspector (ou encontre por tag)
    public Transform attackPoint;           // child posicionado na boca (spawn do projétil)
    public GameObject smokePrefab;          // prefab do projétil (fumaça)
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float followRange = 8f;          // quando o player estiver dentro, Voglin segue
    public float attackRange = 5f;          // distância para iniciar ataque
    public float stopDistance = 0.8f;       // distancia minima para parar de se aproximar

    [Header("Attack")]
    public float attackCooldown = 2.0f;     // tempo entre ataques
    public float projectileSpeed = 7f;
    public int projectileDamage = 1;        // 1 coração por hit
    public float attackDelay = 0.25f;       // delay entre iniciar animação e spawnar projétil

    [Header("Misc")]
    public bool debugDraw = true;

    Rigidbody2D rb;
    float lastAttackTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        // se player não foi arrastado, tenta achar por tag "Player"
        if (player == null)
        {
            var pgo = GameObject.FindGameObjectWithTag("Player");
            if (pgo) player = pgo.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;

        // decidir seguir
        bool shouldFollow = dist <= followRange && dist > stopDistance;
        if (shouldFollow)
        {
            Vector2 dir = toPlayer.normalized;
            rb.velocity = dir * moveSpeed;
        }
        else
        {
            // pairio: sem mover (ou reduzir velocidade)
            rb.velocity = Vector2.zero;
        }

        // Animator: estado de voo
        if (animator) animator.SetBool("isFlying", true);

        // virar para o player (opcional)
        if (toPlayer.x > 0.01f) transform.localScale = new Vector3(1,1,1);
        else if (toPlayer.x < -0.01f) transform.localScale = new Vector3(-1,1,1);

        // ataque
        if (dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(DoAttack());
        }
    }

    IEnumerator DoAttack()
    {
        lastAttackTime = Time.time;
        // Trigger de animação
        if (animator) animator.SetTrigger("attackTrigger");

        // esperar o momento do "bico abrir" - use AttackDelay ou sincronize com Animation Event
        yield return new WaitForSeconds(attackDelay);

        SpawnProjectile();

        // Se preferir, aqui pode esperar animação terminar e resetar states via Animation Event
    }

    void SpawnProjectile()
    {
        if (smokePrefab == null || attackPoint == null || player == null) return;

        Vector3 spawnPos = attackPoint.position;
        Vector2 dir = (player.position - spawnPos).normalized;

        GameObject go = Instantiate(smokePrefab, spawnPos, Quaternion.identity);
        // ajustar rotação do sprite para apontar
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        go.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // configurar componente do projétil
        var proj = go.GetComponent<SmokeProjectile>();
        if (proj != null)
        {
            proj.Initialize(dir, projectileSpeed, projectileDamage, gameObject);
        }
        else
        {
            // fallback: se não houver script, tenta mover pelo rigidbody
            var rbp = go.GetComponent<Rigidbody2D>();
            if (rbp) rbp.velocity = dir * projectileSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!debugDraw) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, followRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

