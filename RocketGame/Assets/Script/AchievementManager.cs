using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// erstellt am 07.08 Martin
public class AchievementManager : MonoBehaviour
{
    private static AchievementManager _instance;
    public static AchievementManager Instance { get { return _instance; } }

    //Achievment
    [SerializeField] private bool AllStars;
    [SerializeField] private bool AllStarsCollected;

    [SerializeField] private bool AllLevels;
    [SerializeField] private bool AllLevelsCollected;

    [SerializeField] private bool GameComplete;
    [SerializeField] private bool GameCompleteCollected;

    //Singleton muster

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
    }

    public void CheckAchievemnts()
    {
        //untersuicht ob der Spieler ein Achievemnt erreicht hat
        if (GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex - 1].getNumberOfCollectedStars() == 3)
        {
            AllStarsComplete();
        }
        bool allLevelCompleted = true;
        bool allStarsInAllLevels = true;

        foreach (Level level in GameManager.Instance.levelList)
        {
            if (level.getNumberOfFinishes() == 0)
            {
                allLevelCompleted = false;
            }

            if (level.getNumberOfCollectedStars() != 3)
            {
                allStarsInAllLevels = false;
            }
        }

        if (allLevelCompleted)
        {
            AllLevelsComplete();
        }

        if (allStarsInAllLevels)
        {
            GameCompleteComplete();
        }
    }
    //hilfsfunktionen für die Achievement seite
    public void AllStarsComplete()
    {
        AllStars = true;
    }

    public void AchievementAllStarsCollecte()
    {
        AllStarsCollected = true;
    }

    public bool GetAchievementAllStarsCompleted()
    {
        return AllStars;
    }

    public bool GetAchievementAllStarsCollected()
    {
        return AllStarsCollected;
    }

    public void AllLevelsComplete()
    {
        AllLevels = true;
    }

    public void AchievementAllLevelsCollecte()
    {
        AllLevelsCollected = true;
    }

    public bool GetAchievementAllLevelsCompleted()
    {
        return AllLevels;
    }

    public bool GetAchievementAllLevelsCollected()
    {
        return AllLevelsCollected;
    }

    public void GameCompleteComplete()
    {
        GameComplete = true;
    }

    public void AchievementGameCompleteCollecte()
    {
        GameCompleteCollected = true;
    }

    public bool GetAchievementGameCompleteCompleted()
    {
        return GameComplete;
    }

    public bool GetAchievementGameCompleteCollected()
    {
        return GameCompleteCollected;
    }
}
