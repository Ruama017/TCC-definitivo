using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    [Header("UI")]
    public Slider oxygenSlider;    // Arraste o Slider da UI aqui

    [Header("Oxigênio")]
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 10f; // Oxigênio por segundo

    [Header("Dano ao Player")]
    public float damagePerSecond = 10f;     // Dano por segundo quando sem oxigênio
    public PlayerHealth playerHealth;       // Referência ao PlayerHealth

    private float currentOxygen;
    private bool inSmoke = false;

    void Start()
    {
        currentOxygen = maxOxygen;
        oxygenSlider.maxValue = maxOxygen;
        oxygenSlider.value = currentOxygen;
    }

    void Update()
    {
        if (inSmoke)
        {
            // Reduz oxigênio
            currentOxygen -= oxygenDepletionRate * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
            oxygenSlider.value = currentOxygen;

            // Aplica dano se oxigênio zerar
            if (currentOxygen <= 0)
            {
                // Converte float para int para PlayerHealth
                int damage = Mathf.RoundToInt(damagePerSecond * Time.deltaTime);
                playerHealth.TakeDamage(damage);
            }
        }
        else
        {
            // Recupera oxigênio lentamente fora da fumaça (opcional)
            currentOxygen += (oxygenDepletionRate / 2) * Time.deltaTime;
            currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);
            oxygenSlider.value = currentOxygen;
        }
    }

    // Chamado quando o player entra em uma área de fumaça
    public void EnterSmoke()
    {
        inSmoke = true;
    }

    // Chamado quando o player sai da área de fumaça
    public void ExitSmoke()
    {
        inSmoke = false;
    }
}
