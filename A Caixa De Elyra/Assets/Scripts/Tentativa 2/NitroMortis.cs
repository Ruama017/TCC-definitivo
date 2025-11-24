using UnityEngine;
using System.Collections;

public class NitroMortis : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Distâncias de Ataque")]
    public float meleeRange = 2f;     
    public float rangedRange = 7f;    

    private Animator anim;
    private Transform player;

    [Header("Ataque 1 - Garra")]
    public Collider2D clawHitbox; 
    public float clawHitboxDelay = 0.3f;      
    public float clawHitboxDuration = 0.25f;  

    [Header("Ataque 2 - Boca")]
    public GameObject projectilePrefab;
    public Transform projectileSpawn; 
    public float projectileDelay = 0.4f;   

    [Header("Thorne")]
    public ThorneBossController thorne; // ARRASTE O THORNE PELO INSPECTOR

    [Header("Sons")]
    public AudioClip attackSfx;
    public AudioClip deathSfx;

    private bool isAttacking = false;
    private bool isDead = false;

    // ====== Propriedades públicas ======
    public int CurrentHealth => currentHealth;
    public bool IsDead => isDead; // <<< NOVA PROPRIEDADE PÚBLICA

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();

        if (clawHitbox != null)
            clawHitbox.enabled = false;
    }

    void Update()
    {
        if (isDead || player == null) return;

        FlipTowardsPlayer();

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            if (distance <= meleeRange)
                StartCoroutine(ClawAttack());
            else if (distance <= rangedRange)
                StartCoroutine(MouthAttack());
        }
    }

    void FlipTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1); 
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    IEnumerator ClawAttack()
    {
        isAttacking = true;
        anim.SetTrigger("ClawAttack");

        yield return new WaitForSeconds(clawHitboxDelay);

        clawHitbox.enabled = true;

        if (attackSfx != null)
            AudioSource.PlayClipAtPoint(attackSfx, transform.position);

        yield return new WaitForSeconds(clawHitboxDuration);

        clawHitbox.enabled = false;

        yield return new WaitForSeconds(0.4f);

        isAttacking = false;
    }

    IEnumerator MouthAttack()
    {
        isAttacking = true;

        anim.SetTrigger("MouthAttack");

        yield return new WaitForSeconds(projectileDelay);

        ShootProjectile();

        if (attackSfx != null)
            AudioSource.PlayClipAtPoint(attackSfx, transform.position);

        yield return new WaitForSeconds(0.4f);

        isAttacking = false;
    }

    void ShootProjectile()
    {
        GameObject p = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        Vector2 dir = (player.position - transform.position).normalized;
        p.GetComponent<Rigidbody2D>().velocity = dir * 6f;
    }

    // =================== DANO ===================
    public void TakeDamage(bool playerHasSuper = false)
    {
        if (isDead) return;

        int appliedDamage = playerHasSuper ? 2 : 1;

        currentHealth -= appliedDamage;
        Debug.Log("Dano aplicado: " + appliedDamage + " | Vida atual: " + currentHealth);

        anim.SetTrigger("Hit");

        if (currentHealth <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        isDead = true; // <<< AQUI é atualizado antes de qualquer outra ação

        if (deathSfx != null)
            AudioSource.PlayClipAtPoint(deathSfx, transform.position);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.15f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        if (thorne != null)
        {
            thorne.gameObject.SetActive(true);
            thorne.ActivateThorne();
        }

        Destroy(gameObject);
    }
}
