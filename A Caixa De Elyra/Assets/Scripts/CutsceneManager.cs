using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class Cena
    {
        public Sprite imagem;
        [TextArea] public string texto;
    }

    public Image cutsceneImage;          // onde as imagens aparecem
    public TextMeshProUGUI cutsceneText; // texto da cutscene
    public Image fadePanel;              // painel preto para fade
    public AudioSource musicaFundo;      // música de fundo
    public float tempoCena = 3f;         // tempo de cada cena
    public float tempoFade = 1f;         // duração do fade
    public Cena[] cenas = new Cena[4];   // 4 imagens com texto

    [Header("Logo final")]
    public Image logoImage;              // logo do jogo
    public float tempoLogo = 2f;         // tempo que a logo fica visível
    public string nomeCenaJogo = "Nivel1"; // nome da cena do jogo

    void Start()
    {
        StartCoroutine(RodarCutscene());
    }

    IEnumerator RodarCutscene()
    {
        if (musicaFundo != null) musicaFundo.Play();

        // Mostra as 4 cenas
        for (int i = 0; i < cenas.Length; i++)
        {
            cutsceneImage.sprite = cenas[i].imagem;
            cutsceneText.text = cenas[i].texto;

            // Fade in
            yield return StartCoroutine(Fade(1, 0, tempoFade));

            yield return new WaitForSeconds(tempoCena);

            // Fade out
            yield return StartCoroutine(Fade(0, 1, tempoFade));
        }

        // Esconde imagem/texto da cutscene
        cutsceneImage.gameObject.SetActive(false);
        cutsceneText.gameObject.SetActive(false);

        // Mostra a logo
        logoImage.gameObject.SetActive(true);

        // Fade in da logo
        yield return StartCoroutine(FadeImage(logoImage, 0, 1, tempoFade));

        // Espera
        yield return new WaitForSeconds(tempoLogo);

        // Fade out da logo
        yield return StartCoroutine(FadeImage(logoImage, 1, 0, tempoFade));

        // Carrega cena do jogo
        SceneManager.LoadScene(nomeCenaJogo);
    }

    // Fade da tela preta
    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float t = 0;
        Color c = fadePanel.color;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadePanel.color = c;
            yield return null;
        }
    }

    // Fade da logo (independente da tela preta)
    IEnumerator FadeImage(Image img, float startAlpha, float endAlpha, float duration)
    {
        float t = 0;
        Color c = img.color;
        while (t < 1)
        {
            t += Time.deltaTime / duration;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t);
            img.color = c;
            yield return null;
        }
    }
}


