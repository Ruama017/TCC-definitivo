using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic; //é a musica do menu
    public AudioClip gameMusic; //musica do jogo
    private AudioSource audioSource;

    private static MusicManager instance;

    void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded; //quando a musica mudar
        }
        else {
            Destroy(gameObject);
        }
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.buildIndex ==0){ //menu
        PlayMusic(menuMusic);

        }
        else if (scene.buildIndex ==1){ //cena 1 
        PlayMusic(gameMusic);

        } 
    }
    void PlayMusic (AudioClip clip){
        if(audioSource.clip == clip) return; //vai evitar q reinicie a msm musica
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }
}
