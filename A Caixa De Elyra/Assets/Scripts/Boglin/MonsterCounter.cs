using UnityEngine;
using UnityEngine.UI;

public class MonsterCounter : MonoBehaviour
{
    [Header("Referências")]
    public Text counterText;
    public GameObject portal;
    public int requiredSouls = 5;

    private int soulsCollected = 0;

    // Chamado pelo Soul quando chega no contador
    public void AddSoul()
    {
        soulsCollected++;
        UpdateUI();
        CheckPortal();
    }

    public void UpdateUI()
    {
        if (counterText != null)
            counterText.text = $"{soulsCollected}/{requiredSouls}";
    }

    public void CheckPortal()
    {
        if (portal != null)
            portal.SetActive(soulsCollected >= requiredSouls);
    }
}
