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
        [TextArea] public string[] textos;
        public float delayEntreTextos = 2f;
        public float fadeTexto = 0.5f;
    }

    public Image cutsceneImage;
    public TextMeshProUGUI cutsceneText;
    public Image fadePanel;
    public AudioSource musicaFundo;
    public float tempoFade = 1f;

    [Header("Logo final")]
    public Image logoImage;
    public float tempoLogo = 2f;
    public string nomeCenaJogo = "Nivel1";

    public Cena[] cenas = new Cena[4];

    private int cenaAtual = 0;
    private Coroutine fadeCoroutine;
    private Coroutine textoCoroutine;

    void Start()
    {
        if (musicaFundo != null) musicaFundo.Play();
        MostrarCena(cenaAtual);
    }

    // Botão para pular toda a cutscene
    public void PularCutscene()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }

    void MostrarCena(int indice)
    {
        if (indice >= cenas.Length)
        {
            StartCoroutine(MostrarLogo());
            return;
        }

        cutsceneImage.sprite = cenas[indice].imagem;
        cutsceneImage.gameObject.SetActive(true);

        // Fade in da imagem
        fadeCoroutine = StartCoroutine(Fade(1, 0, tempoFade));

        // Começa a mostrar os textos automaticamente
        cutsceneText.gameObject.SetActive(true);
        textoCoroutine = StartCoroutine(MostrarTextos(cenas[indice]));
    }

    IEnumerator MostrarTextos(Cena cena)
    {
        foreach (var texto in cena.textos)
        {
            yield return StartCoroutine(FadeText(texto, cena.fadeTexto, cena.delayEntreTextos));
        }

        // Quando termina os textos, espera um pouco e avança para a próxima imagem automaticamente
        yield return new WaitForSeconds(0.5f);
        cenaAtual++;
        MostrarCena(cenaAtual);
    }

    IEnumerator MostrarLogo()
    {
        cutsceneImage.gameObject.SetActive(false);
        cutsceneText.gameObject.SetActive(false);

        logoImage.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImage(logoImage, 0, 1, tempoFade));
        yield return new WaitForSeconds(tempoLogo);
        yield return StartCoroutine(FadeImage(logoImage, 1, 0, tempoFade));

        SceneManager.LoadScene(nomeCenaJogo);
    }

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

        // Mantém visível
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


