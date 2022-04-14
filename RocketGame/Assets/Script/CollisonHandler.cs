using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Erstellt: Martin am 10.07
public class CollisonHandler : MonoBehaviour
{
    [SerializeField] private float delayload = 1f;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip successClip;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private ParticleSystem successParticle;

    private AudioSource audioSource;
    private bool godMode = false;
    bool isTransitioning = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondDebugKeys();
    }

    private void RespondDebugKeys()
    {
        // Verwendet für Debuging
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            godMode = !godMode;
        }
        //zum level neu laden
        else if (Input.GetKeyDown(KeyCode.R))
        {
            CrashSequence();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //debugging godmode
        if (isTransitioning || godMode)
            return;
        
        //gegen was ist der Spieler geflogen?
        switch (collision.gameObject.tag)
        {
            case "Goal":
                EndLevelSequence();
                break;

            case "Friendly":
                break;

            default:
                CrashSequence();     
                break;
            }
 
    }

    private void CrashSequence()
    {
        //wenn der spieler mit der rakete crashed
        GameManager.Instance.CurrentStars = 0;
        GameManager.Instance.Tries++;
		GameManager.Instance.LoadStars();

        isTransitioning = true;
        deathParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(deathClip, audioSource.volume);
        GetComponent<Mover>().enabled = false;
        Invoke("ReloadScene", delayload);
    }

    private void EndLevelSequence()
    {
        //wenn der spieler das level schafft

        //speichert die anzahl der gesammelten sterne und fügt sie dem Konto hinzu
        GameManager.Instance.addTotalStars(GameManager.Instance.CurrentStars - GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex - 1].getNumberOfCollectedStars());
        GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex-1].setNumberOfCollectedStars(GameManager.Instance.CurrentStars - GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex - 1].getNumberOfCollectedStars());
        GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex-1].addNumberOfFinishes(1);
		GameManager.Instance.SaveStars();

        //check the achievements
        AchievementManager.Instance.CheckAchievemnts();
        GameManager.Instance.CurrentStars = 0;
        GameManager.Instance.Tries = 0;

        isTransitioning = true;
        successParticle.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(successClip, audioSource.volume);
        GetComponent<Mover>().enabled = false;
        Invoke("LoadNextScene", delayload);
    }

    private void ReloadScene()
    {
        //ladet die Scene neu
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        
    }

    private void LoadNextScene()
    {
        //ladet die nächste Scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 1;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
