using UnityEngine;

public class PlayerVictoryTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CaixaElyra"))
        {
            Debug.Log("PLAYER PEGOU A CAIXA DE ELYRA → VITÓRIA!");

            if (VictoryManager.instance != null)
                VictoryManager.instance.PlayVictorySequence();
            else
                Debug.LogError("VictoryManager não encontrado na cena!");
        }
    }
}
