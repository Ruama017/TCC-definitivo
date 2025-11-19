using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyToglin : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 2.5f;
    public float stoppingDistance = 0.4f;
    public Transform playerTransform;

    [Header("Dano normal (sem stomp)")]
    public int touchDamage = 1;
    public float damageInterval = 1f;

    [Header("Stomp (pisar)")]
    public bool canBeStomped = true;
    public int stompsToKill = 3;
    public GameObject soulPrefab;
    public float vanishDelay = 0.15f;

    [Header("Referências")]
    public AudioClip vanishSfx;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool awake = false;
    private bool isDead = false;
    private float lastDamageTime;
    private int currentStomps = 0;
    private float initialScaleX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // mantém gravidade natural (para andar no chão)
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        initialScaleX = transform.localScale.x;
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
        if (!awake || isDead || playerTransform == null) return;

        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (dist > stoppingDistance)
        {
            Vector2 dir = (playerTransform.position - transform.position).normalized;
            rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

            // Ajuste para sprite desenhado virado à ESQUERDA
            if (dir.x != 0)
            {
                Vector3 s = transform.localScale;
                s.x = (dir.x > 0 ? -1 : 1) * Mathf.Abs(initialScaleX);
                transform.localScale = s;
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public void WakeUp()
    {
        if (awake || isDead) return;
        awake = true;
        anim.enabled = true;
        anim.Play("Toglin_Idle", 0, 0f);
        StartCoroutine(StartWalkingAfterDelay(0.8f));
    }

    private IEnumerator StartWalkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        anim.Play("Toglin_And_Atac");
    }

    public void Sleep()
    {
        if (isDead) return;
        awake = false;
        anim.enabled = false;
        rb.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (!collision.collider.CompareTag("Player")) return;

        PlayerSuper ps = collision.collider.GetComponent<PlayerSuper>();
        bool playerIsSuper = ps != null && ps.isSuper;

        // ✔ Correção: Só é stomp se o player TIVER super + estiver acima
        bool stomped = canBeStomped && playerIsSuper &&
                       collision.transform.position.y > transform.position.y + 0.25f;

        if (stomped)
        {
            currentStomps++;

            if (ps != null)
                ps.BounceOnStomp(); // player dá bounce

            if (currentStomps >= stompsToKill)
            {
                TransformIntoSoul();
            }
            else
            {
                StartCoroutine(FlashOnStomp());
            }
        }
        else
        {
            // Só dá dano se o player NÃO estiver super
            if (!playerIsSuper)
                TryDealDamage(collision.collider.gameObject);
        }
    }

    private IEnumerator FlashOnStomp()
    {
        sr.color = Color.white * 1.5f;
        yield return new WaitForSeconds(0.12f);
        sr.color = Color.white;
    }

    private void TryDealDamage(GameObject player)
    {
        if (Time.time - lastDamageTime < damageInterval) return;
        lastDamageTime = Time.time;

        var ph = player.GetComponent<PlayerHealth>();
        if (ph != null) ph.TakeDamage(touchDamage);
    }

    private void TransformIntoSoul()
    {
        if (isDead) return;
        isDead = true;

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        foreach (var c in GetComponents<Collider2D>())
            c.enabled = false;

        anim.enabled = false;

        if (vanishSfx != null)
            AudioSource.PlayClipAtPoint(vanishSfx, transform.position);

        if (soulPrefab != null)
            Instantiate(soulPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject, vanishDelay);
    }
}
