using UnityEngine;

public class NitroMortisController : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;
    public Animator anim;

    [Header("Pontos de Ataque")]
    public Transform clawAttackPoint;          // ponto do ataque de garra
    public Transform mouthProjectilePoint;     // ponto de spawn do projétil
    public GameObject poisonProjectilePrefab;  // prefab do projétil

    [Header("Configurações")]
    public float detectionRange = 7f;
    public float clawRange = 2.5f;
    public float attackCooldown = 3f;

    [Header("Áudio")]
    public AudioSource audioSource;
    public AudioClip clawSound;   // som do ataque de garra
    public AudioClip mouthSound;  // som do ataque de boca

    private float lastAttackTime;
    private bool isAttacking;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Se o player está longe, fica no idle
        if (distance > detectionRange || isAttacking)
        {
            anim.Play("Idle_Boss_Notro");
            return;
        }

        // Sempre vira para o player
        FlipTowardsPlayer();

        // Se pode atacar
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack(distance);
        }
        else
        {
            anim.Play("Idle_Boss_Notro");
        }
    }

    void FlipTowardsPlayer()
    {
        Vector3 scale = transform.localScale;
        scale.x = (player.position.x < transform.position.x)
            ? -Mathf.Abs(scale.x)
            : Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void StartAttack(float distance)
    {
        isAttacking = true;

        // Se estiver perto, tem 50/50 de ser garra ou boca
        if (distance <= clawRange)
        {
            int choice = Random.Range(0, 2);

            if (choice == 0)
                anim.Play("Attack_BossMortis");     // Garra
            else
                anim.Play("Attack_BossMortis2");    // Boca
        }
        else
        {
            // Longe → só ataque de boca
            anim.Play("Attack_BossMortis2");
        }
    }

    // === ANIMATION EVENTS ===

    public void NitroMortis_ClawHit()
    {
        if (clawSound) audioSource.PlayOneShot(clawSound);

        Collider2D hit = Physics2D.OverlapCircle(clawAttackPoint.position, clawRange, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            NitroMortisAttack atk = GetComponent<NitroMortisAttack>();
            atk.ApplyDamageAndPoison(hit.gameObject);
        }
    }

    public void NitroMortis_ShootProjectile()
    {
        if (mouthSound) audioSource.PlayOneShot(mouthSound);

        GameObject proj = Instantiate(poisonProjectilePrefab, mouthProjectilePoint.position, Quaternion.identity);

        Vector2 dir = (player.position - mouthProjectilePoint.position).normalized;
        proj.GetComponent<PoisonProjectile>().SetDirection(dir);
    }

    public void NitroMortis_AttackFinished()
    {
        lastAttackTime = Time.time;
        isAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (clawAttackPoint)
            Gizmos.DrawWireSphere(clawAttackPoint.position, clawRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
