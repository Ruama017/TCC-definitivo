using UnityEngine;

public class SmokeArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var oxygen = other.GetComponent<OxygenManager>();
            if(oxygen != null)
                oxygen.EnterSmoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var oxygen = other.GetComponent<OxygenManager>();
            if(oxygen != null)
                oxygen.ExitSmoke();
        }
    }
}
