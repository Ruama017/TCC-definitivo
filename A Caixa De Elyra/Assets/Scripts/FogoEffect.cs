using UnityEngine;

public class FogoEffect : MonoBehaviour
{
    [Header("Dano e Efeitos")]
    public int damagePerSecond = 1;        
    public float slowMultiplier = 0.5f;    
    public float jumpMultiplier = 0.6f;    
    public Color toxicColor = new Color(0.4f, 1f, 0.4f, 1f); 
    public float colorTransitionSpeed = 2f; 

    private bool playerInside = false;
    private float damageTimer = 0f;

    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private SpriteRenderer playerSprite;

    private Color originalColor;
    private float originalSpeed;
    private float originalJumpForce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            playerController = other.GetComponent<PlayerController>();
            playerHealth = other.GetComponent<PlayerHealth>();
            playerSprite = other.GetComponent<SpriteRenderer>();

            if (playerController != null)
            {
                originalSpeed = playerController.moveSpeed;
                originalJumpForce = playerController.jumpForce;

                playerController.moveSpeed *= slowMultiplier;
                playerController.jumpForce *= jumpMultiplier;
            }

            if (playerSprite != null)
                originalColor = playerSprite.color;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (playerController != null)
            {
                playerController.moveSpeed = originalSpeed;
                playerController.jumpForce = originalJumpForce;
            }
        }
    }

    private void Update()
    {
        if (playerInside && playerHealth != null)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= 1f)
            {
                playerHealth.TakeDamage(damagePerSecond);
                damageTimer = 0f;
            }
        }

        if (playerSprite != null)
        {
            Color targetColor = playerInside ? toxicColor : originalColor;
            playerSprite.color = Color.Lerp(
                playerSprite.color,
                targetColor,
                Time.deltaTime * colorTransitionSpeed
            );
        }
    }
}
