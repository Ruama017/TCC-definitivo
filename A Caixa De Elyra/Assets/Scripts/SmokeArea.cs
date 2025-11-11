using UnityEngine;

public class SmokeArea : MonoBehaviour
{
    public float oxygenDrainRate = 10f;  // quanto de oxigênio drena por segundo

    private OxygenManager playerOxygen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entrou na fumaça!");
            playerOxygen = collision.GetComponent<OxygenManager>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player dentro da fumaça!");
            
            if (playerOxygen != null)
            {
                playerOxygen.DecreaseOxygen(oxygenDrainRate * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player saiu da fumaça!");
            playerOxygen = null;
        }
    }
}
