using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VictoryCutsceneManager : MonoBehaviour
{
    public GameObject cutsceneCanvas; // Canvas da cutscene
    public Image cutsceneImage;       // Image para mostrar as imagens
    public TMP_Text cutsceneText;     // Texto da cutscene
    public AudioSource musicSource;   // Música da cutscene

    public Sprite[] images;           // Suas 3 imagens
    public string[] texts;            // Texto correspondente a cada imagem
    public AudioClip musicClip;       // Música da cutscene
    public float imageDuration = 6f;  // Tempo que cada imagem fica na tela

    public void StartCutscene()
    {
        cutsceneCanvas.SetActive(true);
        StartCoroutine(CutsceneSequence());
    }

    private IEnumerator CutsceneSequence()
    {
        // Toca a música
        if (musicSource != null && musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
        }

        // Loop pelas imagens e textos
        for (int i = 0; i < images.Length; i++)
        {
            cutsceneImage.sprite = images[i];
            cutsceneText.text = texts[i];
            yield return new WaitForSeconds(imageDuration);
        }

        // Finaliza a cutscene
        cutsceneCanvas.SetActive(false);
        Debug.Log("Cutscene de vitória finalizada!");
    }
}
