using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EnemyToglin : MonoBehaviour
{
    [Header("Detecção / Movimento")]
    public float speed = 2.5f;                 // velocidade ao seguir
    public float stoppingDistance = 0.4f;      // distância mínima antes de parar
    public float knockbackForce = 3f;          // força do "rebote" após bater no player
    public Transform playerTransform;

    [Header("Dano")]
    public int damage = 1;
    public float damageInterval = 1f;

    [Header("Comportamento")]
    public bool staysAsRockIfPlayerLeaves = false;

    private bool awake = false;
    private Rigidbody2D rb;
    private Animator anim;
    private float lastDamageTime = -999f;
    private bool isRecoiling = false;          // está recuando após colisão

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }

        anim.enabled = false; // começa camuflado
    }

    private void FixedUpdate()
    {
        if (!awake || playerTransform == null || isRecoiling) return;

        Vector2 pos = rb.position;
        Vector2 target = playerTransform.position;
        Vector2 dir = (target - pos).normalized;
        float dist = Vector2.Distance(pos, target);

        if (dist > stoppingDistance)
        {
            rb.velocity = dir * speed;

            // vira sprite de acordo com a direção
            if (dir.x != 0)
            {
                Vector3 s = transform.localScale;
                s.x = Mathf.Sign(dir.x) * Mathf.Abs(s.x);
                transform.localScale = s;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void WakeUp()
    {
        if (awake) return;
        awake = true;

        anim.enabled = true;
        anim.Play("Idle"); // nome da animação de levantar
        StartCoroutine(StartWalkingAfterDelay(0.8f));
    }

    IEnumerator StartWalkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("isWalking", true);
    }

    public void Sleep()
    {
        if (!staysAsRockIfPlayerLeaves)
        {
            awake = false;
            anim.SetBool("isWalking", false);
            anim.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryDealDamage(collision.collider.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryDealDamage(collision.collider.gameObject);

            // aplica "rebote" ao bater
            Vector2 knockDir = (transform.position - collision.transform.position).normalized;
            StartCoroutine(Knockback(knockDir));
        }
    }

    private IEnumerator Knockback(Vector2 direction)
    {
        isRecoiling = true;
        rb.velocity = direction * knockbackForce;
        yield return new WaitForSeconds(0.25f);
        isRecoiling = false;
    }

    private void TryDealDamage(GameObject player)
    {
        if (Time.time - lastDamageTime < damageInterval) return;
        lastDamageTime = Time.time;

        var ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(damage);
        }
    }
}
