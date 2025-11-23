using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    public TipPopup tipPopup;
    public float cooldown = 2f;
    private float lastTime;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastTime >= cooldown)
            {
                lastTime = Time.time;
                tipPopup.ShowTip();
            }
        }
    }
}
