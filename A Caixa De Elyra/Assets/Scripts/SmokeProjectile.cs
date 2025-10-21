using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SmokeProjectile : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 1;
    public float lifeTime = 5f;
    public bool destroyOnHit = true;

    Rigidbody2D rb;
    Vector2 direction;
    GameObject owner; // para evitar dano a quem atirou

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // se não tiver rigidbody, podemos mover por transform no Update
    }

    public void Initialize(Vector2 dir, float spd, int dmg, GameObject ownerGO = null)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        owner = ownerGO;
        Destroy(gameObject, lifeTime);
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }

    void Update()
    {
        // fallback movement se sem rigidbody
        if (rb == null)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Ignorar colisão com o proprietário (voglin)
        if (owner != null && col.gameObject == owner) return;

        // caso colida com o player, aplicar dano
        if (col.CompareTag("Player"))
        {
            TryDamagePlayer(col.gameObject, damage);
            if (destroyOnHit) Destroy(gameObject);
            return;
        }

        // opcional: destruir ao colidir com paredes, terreno, ou outros
        // if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) Destroy(gameObject);
    }

    void TryDamagePlayer(GameObject playerGO, int dmg)
    {
        // tentativas comuns: TakeDamage, Damage, ApplyDamage, ReceiveDamage
        var ph = playerGO.GetComponent<MonoBehaviour>(); // we'll use reflection below

        // Reflection approach: tenta chamar métodos comuns
        var playerType = playerGO.GetType(); // not useful; use GetComponent of scripts

        // Procura por componente que tenha método de dano
        var components = playerGO.GetComponents<MonoBehaviour>();
        foreach (var comp in components)
        {
            if (comp == null) continue;
            var compType = comp.GetType();

            // tenta métodos nomeados comuns
            var m = compType.GetMethod("TakeDamage");
            if (m == null) m = compType.GetMethod("Damage");
            if (m == null) m = compType.GetMethod("ApplyDamage");
            if (m == null) m = compType.GetMethod("ReceiveDamage");
            if (m != null)
            {
                // assume int param
                var parameters = m.GetParameters();
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
                {
                    m.Invoke(comp, new object[] { dmg });
                    return;
                }
                // se recebe float
                if (parameters.Length == 1 && parameters[0].ParameterType == typeof(float))
                {
                    m.Invoke(comp, new object[] { (float)dmg });
                    return;
                }
            }

            // tenta campo público "currentHealth" ou "health"
            var f = compType.GetField("currentHealth");
            if (f == null) f = compType.GetField("health");
            if (f != null)
            {
                try
                {
                    var valObj = f.GetValue(comp);
                    if (valObj is int)
                    {
                        int cur = (int)valObj;
                        f.SetValue(comp, Mathf.Max(0, cur - dmg));
                        return;
                    }
                    if (valObj is float)
                    {
                        float curf = (float)valObj;
                        f.SetValue(comp, Mathf.Max(0f, curf - dmg));
                        return;
                    }
                }
                catch { }
            }
        }

        // último recurso: se Player tem script "PlayerHealth" comum
        var phComp = playerGO.GetComponent("PlayerHealth");
        if (phComp != null)
        {
            // tenta chamar "TakeDamage" dinamicamente
            var m2 = phComp.GetType().GetMethod("TakeDamage");
            if (m2 != null) { m2.Invoke(phComp, new object[] { dmg }); return; }
        }

        Debug.LogWarning("SmokeProjectile: não encontrou método de dano no Player. Verifique o script PlayerHealth. Você pode expor um método TakeDamage(int) ou Damage(int).");
    }
}
