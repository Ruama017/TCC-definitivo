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

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
        UpdateCrystalUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (shieldEffect != null)
            shieldEffect.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHearts();
        CheckDeath();
    }

    public void InstantDeath()
    {
        currentHealth = 0;
        currentCrystals = 0;
        UpdateHearts();
        UpdateCrystalUI();
        Die();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
        }
    }

    public void CollectCrystal(int amount)
    {
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

    void CheckDeath()
    {
        if (currentHealth <= 0 && currentCrystals <= 0)
            Die();
    }

    void Die()
    {
        if (deathSound != null)
            deathSound.Play();

        if (deathEffect != null)
            deathEffect.Play();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
            pc.enabled = false;
    }

    public void RestartLevel()
    {
        currentHealth = maxHealth;
        currentCrystals = 0;
        UpdateHearts();
        UpdateCrystalUI();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu(string menuSceneName)
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void ActivateShield()
    {
        if (shieldActive) return;

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
}