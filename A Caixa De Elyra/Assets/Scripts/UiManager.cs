using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantém nas mudanças de cena
        }
        else
        {
            Destroy(gameObject); // Evita duplicata
        }
    }
}

