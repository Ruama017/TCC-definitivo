using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida do Player")]
    public int maxHealth = 5;
    public int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Cristais")]
    public int maxCrystals = 10;
    public int currentCrystals = 0;
    public Image crystalIcon;
    public Text crystalAmountText;
    public TMP_Text crystalAmountTMP;

    [Header("Escudo")]
    public bool canTakeDamage = true;       
    public float shieldDuration = 15f;      
    private bool shieldActive = false;
    public GameObject shieldEffect;         

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public AudioSource deathSound;
    public ParticleSystem deathEffect;

    [Header("SFX de Dano")]
    public AudioSource damageSound;

    // 🔹 Referência pro Animator
    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
        UpdateCrystalUI();

        anim = GetComponent<Animator>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (shieldEffect != null)
            shieldEffect.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage || isDead) return;

        currentHealth -= damage;

        if (damageSound != null)
            damageSound.Play();

        while (damage > 0 && currentCrystals > 0)
        {
            currentCrystals--;
            currentHealth++;
            damage--;
        }

        if (currentHealth < 0) currentHealth = 0;

        UpdateHearts();
        UpdateCrystalUI();
        CheckDeath();
    }

    // ✅ Método adicionado para morte instantânea (usado pelos espinhos)
    public void InstantDeath()
    {
        if (isDead) return;

        currentHealth = 0;
        UpdateHearts();
        Die();
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }

    public void CollectCrystal(int amount)
    {
        if (isDead) return;

        currentCrystals += amount;
        if (currentCrystals > maxCrystals)
            currentCrystals = maxCrystals;

        UpdateCrystalUI();
    }

    void UpdateCrystalUI()
    {
        if (crystalAmountText != null)
            crystalAmountText.text = currentCrystals.ToString();

        if (crystalAmountTMP != null)
            crystalAmountTMP.text = currentCrystals.ToString();
    }

    public void ActivateShield()
    {
        if (shieldActive || isDead) return;

        shieldActive = true;
        canTakeDamage = false;

        if (shieldEffect != null)
            shieldEffect.SetActive(true);

        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    void DeactivateShield()
    {
        shieldActive = false;
        canTakeDamage = true;

        if (shieldEffect != null)
            shieldEffect.SetActive(false);
    }

    void CheckDeath()
    {
        if (currentHealth <= 0 && !isDead)
            Die();
    }

    void Die()
    {
        isDead = true;

        if (deathSound != null)
            deathSound.Play();

        if (deathEffect != null)
            deathEffect.Play();

        // 🔹 Toca a animação de morte
        if (anim != null)
            anim.SetTrigger("Death");

        // Desativa o PlayerController
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
            pc.enabled = false;

        // Mostra Game Over após o delay da animação
        StartCoroutine(ShowGameOverAfterDelay(1.2f));
    }

    private System.Collections.IEnumerator ShowGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        currentCrystals = 0;
        UpdateHearts();
        UpdateCrystalUI();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}
