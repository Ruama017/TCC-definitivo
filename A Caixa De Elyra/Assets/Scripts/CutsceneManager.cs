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
        [TextArea] public string[] textos;      // múltiplos textos
        public float delayEntreTextos = 2f;     // tempo que cada texto fica visível
        public float fadeTexto = 0.5f;          // duração do fade do texto
    }

    public Image cutsceneImage;           // onde as imagens aparecem
    public TextMeshProUGUI cutsceneText;  // texto da cutscene
    public Image fadePanel;               // painel preto para fade
    public AudioSource musicaFundo;       // música de fundo
    public float tempoFade = 1f;          // duração do fade da imagem

    [Header("Logo final")]
    public Image logoImage;               // logo do jogo
    public float tempoLogo = 2f;          // tempo que a logo fica visível
    public string nomeCenaJogo = "Nivel1"; // nome da cena do jogo

    public Cena[] cenas = new Cena[4];    // 4 imagens com múltiplos textos

    void Start()
    {
        StartCoroutine(RodarCutscene());
    }

    IEnumerator RodarCutscene()
    {
        if (musicaFundo != null) musicaFundo.Play();

        // Percorre todas as cenas
        foreach (var cena in cenas)
        {
            cutsceneImage.sprite = cena.imagem;

            // Fade in da imagem
            yield return StartCoroutine(Fade(1, 0, tempoFade));

            // Mostra os textos com fade
            foreach (var texto in cena.textos)
            {
                yield return StartCoroutine(FadeText(texto, cena.fadeTexto, cena.delayEntreTextos));
            }

            // Fade out da imagem
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

    // Faz fade do texto
    IEnumerator FadeText(string texto, float fadeDuration, float displayTime)
    {
        cutsceneText.text = texto;
        Color c = cutsceneText.color;

        // Fade in
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Lerp(0, 1, t);
            cutsceneText.color = c;
            yield return null;
        }

        // Mantém o texto visível
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Lerp(1, 0, t);
            cutsceneText.color = c;
            yield return null;
        }
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

    // Fade da logo
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



