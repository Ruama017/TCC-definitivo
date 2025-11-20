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

    private bool isAttacking = false;
    private bool isDead = false;

    // ====== Propriedades públicas para PlayerAttackHitbox ======
    public int CurrentHealth => currentHealth;
    public bool IsAlive => !isDead;

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
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        // Aplica 1 de dano por hit, mesmo com super ativo
        currentHealth -= 1;
        anim.SetTrigger("Hit");

        if (currentHealth <= 0)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        isDead = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for (int i = 0; i < 3; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.15f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        // Ativa o Thorne via referência pública
        if (thorne != null)
        {
            thorne.gameObject.SetActive(true);
            thorne.ActivateThorne();
        }

        Destroy(gameObject);
    }
}
