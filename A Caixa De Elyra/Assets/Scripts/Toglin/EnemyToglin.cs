using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyToglin : MonoBehaviour
{
    [Header("Detecção / Movimento")]
    public float speed = 2.5f;                 // velocidade horizontal
    public float stoppingDistance = 0.4f;      // distancia pra parar antes de encostar
    public Transform playerTransform;

    [Header("Knockback")]
    public float knockbackForce = 3f;          // força horizontal do rebote
    public float knockbackDuration = 0.18f;    // tempo do recuo

    [Header("Dano")]
    public int damage = 1;
    public float damageInterval = 1f;

    [Header("Comportamento")]
    public bool staysAsRockIfPlayerLeaves = false;

    [Header("Sprites")]
    public Sprite rockSprite;                  // opcional: sprite fixa de pedra (se deixar vazio usa a sprite inicial)

    // estados internos
    private bool awake = false;
    private bool isRecoiling = false;
    private float lastDamageTime = -999f;

    // componentes
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // escala original pra controlar flip corretamente
    private float initialScaleX;
    private Sprite initialSprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // configurações físicas seguras
        rb.gravityScale = 0f; // se preferir que o inimigo sofra gravidade, coloque 1 e remova manipulações Y.
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // guarda escala/sprite iniciais
        initialScaleX = transform.localScale.x;
        if (sr != null) initialSprite = sr.sprite;
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }

        // começa camuflado: Animator desligado e sprite de pedra
        if (anim != null) anim.enabled = false;
        ApplyRockSprite();
        rb.velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (!awake || playerTransform == null || isRecoiling) return;

        Vector2 pos = rb.position;
        Vector2 target = playerTransform.position;
        Vector2 fullDir = (target - pos);
        float dist = fullDir.magnitude;

        // só move horizontalmente para evitar "voar" por causa de colisões verticais
        float dirX = Mathf.Sign(fullDir.x); // -1, 0 ou +1

        if (Mathf.Abs(fullDir.x) > 0.01f && dist > stoppingDistance)
        {
            // Mantemos a velocidade Y atual (evita "flutuar"). Só ajustamos X.
            rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

            // flip correto usando a escala inicial para não inverter feições
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(initialScaleX) * dirX;
            transform.localScale = s;
        }
        else
        {
            // para o movimento horizontal quando perto ou sem direção
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    // ---------------- wake / sleep ----------------
    public void WakeUp()
    {
        if (awake) return;
        awake = true;

        // liga o animator e garante que a animação Idle comece do início (sem frames "abaixando")
        if (anim != null)
        {
            anim.enabled = true;
            anim.Play("Idle", 0, 0f); // toca Idle desde o início — substitua o nome se for diferente
        }

        // espera a animação de levantar e começa a andar
        StartCoroutine(StartWalkingAfterDelay(0.7f));
    }

    private IEnumerator StartWalkingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (anim != null) anim.SetBool("isWalking", true);
    }

    public void Sleep()
    {
        if (!staysAsRockIfPlayerLeaves)
        {
            awake = false;
            // para movimento
            rb.velocity = Vector2.zero;
            // reseta parametros do animator e desliga
            if (anim != null)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isAwake", false);
                anim.enabled = false;
            }
            // volta a sprite de pedra (ou a sprite inicial caso rockSprite esteja null)
            ApplyRockSprite();

            // limpa estado de recuo pra evitar travar
            isRecoiling = false;
        }
    }

    private void ApplyRockSprite()
    {
        if (sr == null) return;
        if (rockSprite != null) sr.sprite = rockSprite;
        else sr.sprite = initialSprite;
    }

    // ---------------- dano / colisão ----------------
    // usamos colisão física normal (não trigger) para manter "corpos" colidindo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryDealDamage(collision.collider.gameObject);

            // aplica knockback HORIZONTAL (somente X) para evitar elevar o inimigo
            float sign = Mathf.Sign(transform.position.x - collision.transform.position.x);
            Vector2 knockDir = new Vector2(sign, 0f).normalized;
            StartCoroutine(Knockback(knockDir));
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TryDealDamage(collision.collider.gameObject);
        }
    }

    private IEnumerator Knockback(Vector2 direction)
    {
        isRecoiling = true;
        // força horizontal temporária
        rb.velocity = direction * knockbackForce;
        yield return new WaitForSeconds(knockbackDuration);
        // para o recuo (mantém Y como estava)
        rb.velocity = new Vector2(0f, rb.velocity.y);
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
