using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida do Player")]
    public int maxHealth = 5;
    public int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private int partialDamage = 0; // acumula dano parcial antes de perder um coração

    [Header("Corações Extras (Fase 3)")]
    public GameObject extraHeartPrefab;   
    public Transform extraHeartsParent;   
    public List<Image> extraHearts = new List<Image>();

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

    [Header("Status Negativo")]
    private bool isPoisoned = false;
    private bool isWeakened = false;

    [Header("Bota / Super")]
    public bool hasBootSuper = false;
    public float superDuration = 5f;
    private float superTimer = 0f;

    private Animator anim;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHearts();
        UpdateExtraHearts();
        UpdateCrystalUI();

        anim = GetComponent<Animator>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (shieldEffect != null)
            shieldEffect.SetActive(false);
    }

    void Update()
    {
        if (hasBootSuper)
        {
            superTimer -= Time.deltaTime;
            if (superTimer <= 0f)
            {
                hasBootSuper = false;
                superTimer = 0f;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if ((shieldActive || !canTakeDamage) || isDead) return;

        if (isWeakened)
            damage = Mathf.CeilToInt(damage * 1.2f);

        for (int i = extraHearts.Count - 1; i >= 0 && damage > 0; i--)
        {
            if (extraHearts[i].gameObject.activeSelf)
            {
                extraHearts[i].gameObject.SetActive(false);
                damage--;
            }
        }

        while (damage > 0 && currentCrystals > 0)
        {
            currentCrystals--;
            damage--;
        }

        while (damage > 0 && currentHealth > 0)
        {
            partialDamage++;
            damage--;

            if (partialDamage >= 2)
            {
                currentHealth--;
                partialDamage = 0;
            }
        }

        if (damageSound != null)
            damageSound.Play();

        UpdateHearts();
        UpdateExtraHearts();
        UpdateCrystalUI();

        CheckDeath();
    }

    public void InstantDeath()
    {
        if (isDead) return;

        currentHealth = 0;
        foreach (Image img in extraHearts)
            img.gameObject.SetActive(false);

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
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
    }

    void UpdateExtraHearts()
    {
        foreach (Image img in extraHearts)
            if (img.gameObject.activeSelf)
                img.sprite = fullHeart;
    }

    public void AddExtraHearts(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject h = Instantiate(extraHeartPrefab, extraHeartsParent);
            h.SetActive(true);
            Image img = h.GetComponent<Image>();
            img.sprite = fullHeart;
            extraHearts.Add(img);
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
        {
            shieldEffect.SetActive(true);
            ParticleSystem ps = shieldEffect.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Play();
        }

        Invoke(nameof(DeactivateShield), shieldDuration);
    }

    void DeactivateShield()
    {
        shieldActive = false;
        canTakeDamage = true;

        if (shieldEffect != null)
        {
            ParticleSystem ps = shieldEffect.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Stop();
            shieldEffect.SetActive(false);
        }
    }

    void CheckDeath()
    {
        if (currentHealth <= 0 && !isDead && AllExtraHeartsGone())
            Die();
    }

    bool AllExtraHeartsGone()
    {
        foreach (Image img in extraHearts)
            if (img.gameObject.activeSelf)
                return false;
        return true;
    }

    void Die()
    {
        isDead = true;

        float deathOffsetY = -0.3f;
        transform.position += new Vector3(0, deathOffsetY, 0);

        if (deathSound != null)
            deathSound.Play();

        if (deathEffect != null)
            deathEffect.Play();

        if (anim != null)
            anim.SetTrigger("Death");

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
            pc.enabled = false;

        StartCoroutine(ShowGameOverAfterDelay(1.2f));
    }

    private IEnumerator ShowGameOverAfterDelay(float delay)
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
        isPoisoned = false;
        isWeakened = false;

        foreach (Image img in extraHearts)
            Destroy(img.gameObject);

        extraHearts.Clear();
        hasBootSuper = false;
        superTimer = 0f;
        partialDamage = 0;

        UpdateHearts();
        UpdateCrystalUI();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu(string menuSceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void ApplyPoison(float duration, float tickDamage, float tickInterval)
    {
        if (!isPoisoned)
            StartCoroutine(PoisonCoroutine(duration, tickDamage, tickInterval));
    }

    IEnumerator PoisonCoroutine(float duration, float tickDamage, float tickInterval)
    {
        isPoisoned = true;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            TakeDamage(Mathf.CeilToInt(tickDamage));
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
        isPoisoned = false;
    }

    public void ApplyWeakness(float duration)
    {
        if (!isWeakened)
            StartCoroutine(WeaknessCoroutine(duration));
    }

    IEnumerator WeaknessCoroutine(float duration)
    {
        isWeakened = true;
        yield return new WaitForSeconds(duration);
        isWeakened = false;
    }

    public void CollectBoot()
    {
        hasBootSuper = true;
        superTimer = superDuration;
    }

    public bool IsSuperActive()
    {
        return hasBootSuper && superTimer > 0f;
    }
}
