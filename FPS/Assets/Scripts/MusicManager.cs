using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instancia; //para que solo haya un musicmanager

    public AudioSource audioSource;

    [Header("Musicas")]
    public AudioClip musicaMenu;
    public AudioClip musicaPausa;
    public AudioClip musicaVictoria;
    public AudioClip musicaDerrota;

    void Awake()
    {
        if (instancia == null)
        {
            //si ya existe otro MusicManager se destryue
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CambiarMusicaSegunEscena(); //reproduce la música adecuada al empezar
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //cambia la música al cargar una nueva escena
    {
        CambiarMusicaSegunEscena();
    }

    void CambiarMusicaSegunEscena()
    {
        string nombreEscena = SceneManager.GetActiveScene().name;
        Debug.Log("Escena actual: " + nombreEscena);

        // Gameplay va sin música
        if (nombreEscena.Contains("Gameplay"))
        {
            audioSource.Stop(); // corta la música del menu
            Debug.Log("Entramos al Gameplay: música detenida");
        }
        else
        {
            ReproducirMenu(); // En otra escena, reproduce música de menú, ya que gameplay es la unica q no lleva 
        }
    }

    public void ReproducirMenu()
    {
        if (musicaMenu == null) return;
        if (audioSource.clip == musicaMenu && audioSource.isPlaying) return;

        audioSource.Stop();
        audioSource.clip = musicaMenu;
        audioSource.loop = true; //loop infinito en menu
        audioSource.Play();
        Debug.Log("Música del menú reproducida");
    }

    public void ReproducirPausa()
    {
        if (musicaPausa == null) return;

        audioSource.Stop();
        audioSource.clip = musicaPausa;
        audioSource.loop = true; // loop mientras está pausado
        audioSource.Play();
        Debug.Log("Música de pausa reproducida");
    }

    public void ReproducirVictoria()
    {
        if (musicaVictoria == null) return;

        audioSource.Stop();
        audioSource.clip = musicaVictoria;
        audioSource.loop = false; //se reproduce solo uan vez
        audioSource.Play();
        Debug.Log("Música de victoria reproducida");
    }

    public void ReproducirDerrota()
    {
        if (musicaDerrota == null) return;

        audioSource.Stop();
        audioSource.clip = musicaDerrota;
        audioSource.loop = false;
        audioSource.Play();
        Debug.Log("Música de derrota reproducida");
    }
}
