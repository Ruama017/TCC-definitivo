using UnityEngine;

public class PulmaoDaMorte : MonoBehaviour
{
    [Header("Dano")]
    public int damagePerTick = 1;       // dano por tick
    public float damageInterval = 0.5f; // intervalo entre ticks em segundos

    [Header("Ciclo do gás")]
    public ParticleSystem gasParticles; // arraste o GasEffect aqui
    public float gasDuration = 3f;      // tempo que o gás fica ativo (sopro)
    public float gasCooldown = 2f;      // tempo que fica inativo
    public bool gasStartsActive = true; // se começa emitindo

    // estados internos
    private float gasTimer = 0f;
    private bool gasAtivo;
    private float damageTimer = 0f;
    private PlayerHealth playerInArea = null;

    void Start()
    {
        gasAtivo = gasStartsActive;
        if (gasParticles != null)
        {
            if (gasAtivo) gasParticles.Play();
            else gasParticles.Stop();
        }
    }

    void Update()
    {
        // ciclo liga/desliga do gás
        gasTimer += Time.deltaTime;
        if (gasAtivo && gasTimer >= gasDuration)
        {
            gasAtivo = false;
            gasTimer = 0f;
            if (gasParticles != null) gasParticles.Stop();
        }
        else if (!gasAtivo && gasTimer >= gasCooldown)
        {
            gasAtivo = true;
            gasTimer = 0f;
            if (gasParticles != null) gasParticles.Play();
        }

        // aplica dano se player estiver na área e o gás estiver ativo
        if (playerInArea != null && gasAtivo)
        {
            // respeita invencibilidade do player
            if (!playerInArea.canTakeDamage) return;

            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                playerInArea.TakeDamage(damagePerTick);
            }
        }
    }

    // Quando algo entra no trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            playerInArea = ph;
            damageTimer = 0f;

            // opcional: se preferir tocar o gás só quando player entra
            // if (gasParticles != null && !gasParticles.isPlaying) gasParticles.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null && ph == playerInArea)
        {
            playerInArea = null;
            damageTimer = 0f;

            // opcional: parar o gás ao sair se você controla assim
            // if (gasParticles != null && gasParticles.isPlaying) gasParticles.Stop();
        }
    }
}
