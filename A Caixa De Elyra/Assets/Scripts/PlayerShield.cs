using UnityEngine;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour
{
    [Header("Configuração do Escudo")]
    public float shieldDuration = 15f;
    public KeyCode activateShieldKey = KeyCode.A;

    [Header("Referências")]
    public PlayerHealth playerHealth;
    public ParticleSystem shieldAura; // a aura de partículas do escudo
    public Image shieldIcon;          // ícone no Canvas
    public Text shieldAmountText;     // quantidade disponível

    private int availableShields = 1; // número de escudos coletados
    private bool shieldActive = false;

    void Start()
    {
        if (shieldAura != null)
            shieldAura.Stop();

        UpdateShieldUI();
    }

    void Update()
    {
        // Ativar escudo ao apertar tecla
        if (Input.GetKeyDown(activateShieldKey) && availableShields > 0 && !shieldActive)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        shieldActive = true;
        availableShields--;
        UpdateShieldUI();

        // Torna o player imune
        if (playerHealth != null)
            playerHealth.canTakeDamage = false;

        // Ativa o efeito visual
        if (shieldAura != null)
            shieldAura.Play();

        // Inicia contagem do tempo do escudo
        StartCoroutine(ShieldDurationCoroutine());
    }

    System.Collections.IEnumerator ShieldDurationCoroutine()
    {
        yield return new WaitForSeconds(shieldDuration);

        // Desativa o escudo
        shieldActive = false;

        if (playerHealth != null)
            playerHealth.canTakeDamage = true;

        if (shieldAura != null)
            shieldAura.Stop();
    }

    public void CollectShield(int amount = 1)
    {
        availableShields += amount;
        UpdateShieldUI();
    }

    void UpdateShieldUI()
    {
        if (shieldAmountText != null)
            shieldAmountText.text = availableShields.ToString();
    }
}
