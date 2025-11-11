using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    public float maxOxygen = 100f;
    public float currentOxygen;
    public Slider oxygenSlider;
    public PlayerHealth playerHealth;
    public float damageRate = 1f;

    private void Start()
    {
        currentOxygen = maxOxygen;
        oxygenSlider.value = maxOxygen;
    }

    private void Update()
    {
        oxygenSlider.value = currentOxygen;

        if (currentOxygen <= 0)
        {
            playerHealth.TakeDamage(Mathf.RoundToInt(damageRate * Time.deltaTime));

        }
    }

    public void DecreaseOxygen(float amount)
    {
        currentOxygen -= amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
    }

    public void RestoreOxygen()
    {
        currentOxygen = maxOxygen;
    }
}
