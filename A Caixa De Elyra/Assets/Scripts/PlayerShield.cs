using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public ParticleSystem shieldAura; // Arrasta o Particle System aqui no Inspector
    public float shieldDuration = 15f; // Tempo do escudo

    private bool isShieldActive = false;
    private float shieldTimer = 0f;
    private int shieldCount = 0;

    void Update()
    {
        if(isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if(shieldTimer <= 0f)
            {
                DeactivateShield();
            }
        }
    }

    public void AddShield(int amount)
    {
        shieldCount += amount;
        ActivateShield();
    }

    void ActivateShield()
    {
        isShieldActive = true;
        shieldTimer = shieldDuration;
        if(shieldAura != null && !shieldAura.isPlaying)
            shieldAura.Play();
    }

    void DeactivateShield()
    {
        isShieldActive = false;
        shieldCount = 0;
        if(shieldAura != null && shieldAura.isPlaying)
            shieldAura.Stop();
    }

    public bool IsShielded()
    {
        return isShieldActive;
    }
}
