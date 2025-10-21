using UnityEngine;

public class SmokeAttack : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;

    // ESTE MÉTODO É O Init que o BoglinController chama
    public void Init(Transform player)
    {
        target = player;
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.2f)
            {
                PlayerHealth ph = target.GetComponent<PlayerHealth>();
                if (ph != null) ph.TakeDamage(1);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}