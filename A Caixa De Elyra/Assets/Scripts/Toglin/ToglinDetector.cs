using UnityEngine;

public class ToglinDetector : MonoBehaviour
{
    public EnemyToglin parentToglin; // arraste o objeto pai no inspector
    public string playerTag = "Player";

    private void Awake()
    {
        if (parentToglin == null)
            parentToglin = GetComponentInParent<EnemyToglin>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            parentToglin?.WakeUp();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            parentToglin?.Sleep(); // se quiser que volte a pedra, ative staysAsRockIfPlayerLeaves = false
        }
    }
}
