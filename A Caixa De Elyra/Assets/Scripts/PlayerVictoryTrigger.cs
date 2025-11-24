using UnityEngine;

public class PlayerVictoryTrigger : MonoBehaviour
{
    [Header("Referências")]
    public ThorneBossController bossThorne; // opcional, pode deixar vazio

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (!gameObject.CompareTag("CaixaElyra"))
            return;

        // Se não tiver no Inspector, tenta achar automaticamente
        if (bossThorne == null)
        {
            bossThorne = FindObjectOfType<ThorneBossController>();
            if (bossThorne == null)
            {
                Debug.LogError("[PlayerVictoryTrigger] Boss Thorne não encontrado na cena!");
                return;
            }
        }

        if (!bossThorne.IsDead)
        {
            Debug.Log("[PlayerVictoryTrigger] Thorne ainda vivo. Caixa bloqueada.");
            return;
        }

        if (VictoryManager.instance != null)
        {
            VictoryManager.instance.TriggerVictorySequence();
        }
        else
        {
            Debug.LogError("[PlayerVictoryTrigger] VictoryManager.instance está NULL!");
        }
    }
}
