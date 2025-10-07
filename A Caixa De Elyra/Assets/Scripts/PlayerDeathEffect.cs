using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDeathEffect : MonoBehaviour
{
    [Header("Referências")]
    public Image deathFade;           // arraste a imagem preta aqui
    public ParticleSystem deathParticles; // arraste o sistema de partículas aqui
    public float fadeDuration = 1f;

    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Ativa partículas
        if(deathParticles != null)
            deathParticles.Play();

        // Inicia fade out
        if(deathFade != null)
            StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color color = deathFade.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            deathFade.color = color;
            yield return null;
        }

        color.a = 1f;
        deathFade.color = color;

        // Aqui você pode chamar a função de Game Over, reiniciar a fase, etc.
        Debug.Log("Game Over");
    }
}
