using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Erstellt: Martin am 17.07
public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    private AudioSource auSourc;
    public AudioMixer mixer;
    public AudioMixerSnapshot UnPaused;
    public AudioMixerSnapshot Paused;
    public bool changeImage = true;

    [SerializeField] private GameObject UI;
    [SerializeField] private List<BackgroundAudioClip> BackgroundMusicList;


    // Singleton muster
    public static SoundManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        auSourc = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UnPaused.TransitionTo(0);
        // sucht das UI element
        UI = GameObject.Find("UI_Design");

        //fügt die FUnktion hinzu dass immer wenn eine Scene geladen wird das UI Element in der Scene gesucht wird
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // wenn kein Backingtrack läuft wird ein zufälliger gestartet
        if (!auSourc.isPlaying)
        {
            HandleBackgroundMusic();
        }
    }

    public void ChangeEffectVolume(float volume)
    {
        //setzt die Effektlautstärke auf eine gewisse Menge
        mixer.SetFloat("Effects", volume);
        if (changeImage)
        {
            UI.GetComponent<UIManager>().activateEffects();
        }
            
    }

    public void ChangeMusicVolume(float volume)
    {
        //Setzt die Musik auf eine bestimmte Lautstärke
        mixer.SetFloat("Music", volume);
        if(changeImage)
        {
            UI.GetComponent<UIManager>().activateMusic();
        }
            
    }

    public void ChangeMasterVolume(float volume)
    {
        //setzt die Lautstärke des Mixers
        mixer.SetFloat("Master", volume);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // wenn die Scene geladen wird sucht er das UI
        UI = GameObject.Find("UI_Design");
    }

    private void HandleBackgroundMusic()
    {
        //sucht sich eine zufälliges Hintergrundlied (falls mehr als einer in der Liste ist)
        int indexNumber = Random.Range(0, BackgroundMusicList.Count - 1);
        auSourc.volume = BackgroundMusicList[indexNumber].volume;
        auSourc.PlayOneShot(BackgroundMusicList[indexNumber].audio);
    }

    [System.Serializable]
    public class BackgroundAudioClip
    {
        // der Hintergrundclip als Klasse
        public AudioClip audio;
        [Range(0,1f)] public float volume = 1f;
    }
}
