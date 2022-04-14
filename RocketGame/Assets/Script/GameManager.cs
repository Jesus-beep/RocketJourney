using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Erstellt: Martin am 14.07
//Bearbeitet: Nina am 13.08.

public class GameManager : MonoBehaviour
{   
    private static GameManager _instance;
    
    [SerializeField] private int TotalStars = 0;
    //Shop modifications
    [SerializeField] private float FuelTankRocket = 100f;
	//Upgradable maximal fuel amount, up to 300

    [SerializeField] private float FuelConsumption = 20f;
    //Upgradable consumption, down to 5


    public int CurrentStars = 0;
	public float FuelCurrent = 0;
    public Level[] levelList;

    private GameObject Star1;
    private GameObject Star2;
    private GameObject Star3;
	private bool collected1 = false;
	private bool collected2 = false;
	private bool collected3 = false;

    //ShopUpgrades
    public bool TankUpgradeI = false;
    public bool TankUpgradeII = false;
    public bool TankUpgradeIII = false;

    public bool ConsumptionUpgradeI = false;
    public bool ConsumptionUpgradeII = false;
    public bool ConsumptionUpgradeIII = false;

    public int Tries;

    // Singleton Muster
    public static GameManager Instance { get { return _instance; } }

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
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += CheckStars;
    }

    public float GetFuelTankMax()
    {
        return FuelTankRocket;
    }

    public void SetFuelTankMax(float amount)
    {
        FuelTankRocket = amount;
    }

    public float getConsumption()
    {
        return FuelConsumption;
    }

    public void setConsumption(float amount)
    {
        FuelConsumption = amount;
    }

    public void addTotalStars(int amount)
    {
        // Fügt dem Spieler konto Sterne hinzu
        TotalStars += amount;
    }

    public void removeTotalStars (int amount)
    {
        // Entfernt Sterne vom Konto
        TotalStars -= amount;
    }

    public int getTotalStars()
    {
        // Gibt die Anzahl an Sternen im Spielerkonto zurück
        return TotalStars;
    }

	//Bearbeitet: Nina
    public void CheckStars(Scene scene , LoadSceneMode mode)
    {
        // check if Menu Scene
        // bearbeitet am 07.08 von Martin 
        // aktiviert die nicht aufgesammelten Sterne
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            Star1 = GameObject.Find("Star1");
            Star2 = GameObject.Find("Star2");
            Star3 = GameObject.Find("Star3");

			Star1.SetActive(!levelList[SceneManager.GetActiveScene().buildIndex - 1].getStar1collected());
			Star2.SetActive(!levelList[SceneManager.GetActiveScene().buildIndex - 1].getStar2collected());
			Star3.SetActive(!levelList[SceneManager.GetActiveScene().buildIndex - 1].getStar3collected());
			
            /*int CollectedStarsInThisLevel = levelList[SceneManager.GetActiveScene().buildIndex - 1].getNumberOfCollectedStars();

            switch (CollectedStarsInThisLevel)
            {
                case (0):
                    Star1.SetActive(true);
                    Star2.SetActive(true);
                    Star3.SetActive(true);
                    break;
                case (1):
                    Star1.SetActive(false);
                    Star2.SetActive(true);
                    Star3.SetActive(true);
                    break;
                case (2):
                    Star1.SetActive(false);
                    Star2.SetActive(false);
                    Star3.SetActive(true);
                    break;
                case (3):
                    Star1.SetActive(false);
                    Star2.SetActive(false);
                    Star3.SetActive(false);
                    break;
                default:
                    Star1.SetActive(false);
                    Star2.SetActive(false);
                    Star3.SetActive(false);
                    break;
            }*/
        }
    }
	
	//Nina
	public void HitStar(string StarName)
	{
        //Speichert die gesammelten Sterne im GameManager
		if(StarName.Equals("Star1"))
			collected1 = true;
		else if(StarName.Equals("Star2"))
			collected2 = true;
		else if(StarName.Equals("Star3"))
			collected3 = true;
	}
	
	public void SaveStars()
	{
        //Speichert die gesammelten Sterne in der level liste
        levelList[SceneManager.GetActiveScene().buildIndex - 1].setStar1collected(collected1);
		levelList[SceneManager.GetActiveScene().buildIndex - 1].setStar2collected(collected2);
		levelList[SceneManager.GetActiveScene().buildIndex - 1].setStar3collected(collected3);
	}
	
	public void LoadStars()
	{
        //Lädt die gesammelten Sterne aus der level Liste
		collected1 = levelList[SceneManager.GetActiveScene().buildIndex - 1].getStar1collected();
		collected2 = levelList[SceneManager.GetActiveScene().buildIndex - 1].getStar2collected();
		collected3 = levelList[SceneManager.GetActiveScene().buildIndex - 1].getStar3collected();
	}
}
