using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

//Erstellt: Martin am 17.07

public class UIMainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject MainScreen;
    [SerializeField] private GameObject ChooseLevelScreen;
    [SerializeField] private GameObject ShopScreen;
    [SerializeField] private GameObject SettingsScreen;
    [SerializeField] private GameObject AchievementScreen;
	[SerializeField] private GameObject CreditsScreen;
    [SerializeField] private GameObject[] LevelChooseUI;


    [Header("Shop Buttons")]
    [SerializeField] private Button FuelTankUpgradeButton;
    [SerializeField] private Button FuelTankUpgradeButtonII;
    [SerializeField] private Button FuelTankUpgradeButtonIII;
    [SerializeField] private int FuelTankUpgradeCost;
    [SerializeField] private int FuelTankUpgradeCostII;
    [SerializeField] private int FuelTankUpgradeCostIII;
    [SerializeField] private int FuelTankUpgradeTo;
    [SerializeField] private int FuelTankUpgradeToII;
    [SerializeField] private int FuelTankUpgradeToIII;

    [SerializeField] private Button ConsumptionUpgradeButton;
    [SerializeField] private Button ConsumptionUpgradeButtonII;
    [SerializeField] private Button ConsumptionUpgradeButtonIII;
    [SerializeField] private int ConsumptionUpgradeCost;
    [SerializeField] private int ConsumptionUpgradeCostII;
    [SerializeField] private int ConsumptionUpgradeCostIII;
    [SerializeField] private int ConsumptionUpgradeTo;
    [SerializeField] private int ConsumptionUpgradeToII;
    [SerializeField] private int ConsumptionUpgradeToIII;

    //Shop Items
    private bool FuelTankUpgrade;
    private bool FuelTankUpgradeII;
    private bool FuelTankUpgradeIII;
    private bool ConsumptionUpgrade;
    private bool ConsumptionUpgradeII;
    private bool ConsumptionUpgradeIII;

    [Header("SoundSlider")]
    [SerializeField] private Slider Master;
    [SerializeField] private Slider Effects;
    [SerializeField] private Slider Music;

    [Header("UI Elements")]
    [SerializeField] private GameObject StarCounterObject;

    [Header("Achievements")]
    [SerializeField] private Button AllStarsButton;
	[SerializeField] private GameObject AllStarsTrophy;
    [SerializeField] private TextMeshProUGUI AllStarsButtonText;

    [SerializeField] private Button AllLevelsButton;
    [SerializeField] private GameObject AllLevelsTrophy;
    [SerializeField] private TextMeshProUGUI AllLevelsButtonText;

    [SerializeField] private Button GameCompleteButton;
    [SerializeField] private GameObject GameCompleteTrophy;
    [SerializeField] private TextMeshProUGUI GameCompleteButtonText;


    private float MasterValue;
    private float EffectsValue;
    private float MusicValue;
    private TextMeshProUGUI StarCounterText;

    private void Awake()
    {
        //Zieht die gespeicherten Upgrades aus dem game Manager
        FuelTankUpgrade = GameManager.Instance.TankUpgradeI;
        FuelTankUpgradeII = GameManager.Instance.TankUpgradeII;
        FuelTankUpgradeIII = GameManager.Instance.TankUpgradeIII;

        ConsumptionUpgrade = GameManager.Instance.ConsumptionUpgradeI;
        ConsumptionUpgradeII = GameManager.Instance.ConsumptionUpgradeII;
        ConsumptionUpgradeIII = GameManager.Instance.ConsumptionUpgradeIII;
    }
    private void Start()
    {
        //updatet die Slider in den Settings wenn sie angepasst wurden
        ConnectSlidersToSoundManager();
        StarCounterText = StarCounterObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }
    }

    private void ConnectSlidersToSoundManager()
    {
        //Verbindet die Settings slider mit dem audio mixer
        //master
        SoundManager.Instance.mixer.GetFloat("Master", out MasterValue);
        Master.value = MasterValue;
        Master.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeMasterVolume(Master.value); });

        //effects
        SoundManager.Instance.mixer.GetFloat("Effects", out EffectsValue);
        Effects.value = EffectsValue;
        Effects.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeEffectVolume(Effects.value); });

        //master
        SoundManager.Instance.mixer.GetFloat("Music", out MusicValue);
        Music.value = MusicValue;
        Music.onValueChanged.AddListener(delegate { SoundManager.Instance.ChangeMusicVolume(Music.value); });
    }

    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnSettings()
    {
        MainScreen.SetActive(false);
        SettingsScreen.SetActive(true);
    }

    public void OnChooseLevel()
    {
        MainScreen.SetActive(false);
        ChooseLevelScreen.SetActive(true);
        CheckLevelUI();

    }

    public void OnShop()
    {
        MainScreen.SetActive(false);
        ShopScreen.SetActive(true);
        CheckShopItems();
        UpdateStarCounter();
    }

    public void OnAchievement()
    {
        MainScreen.SetActive(false);
        AchievementScreen.SetActive(true);
        CheckAchievement();
    }
	
	public void OnCredits()
    {
        MainScreen.SetActive(false);
        CreditsScreen.SetActive(true);
    }

    public void OnExit()
    {
        //beendet spiel
        Application.Quit();
    }
	
    public void OnBack()
    {
        MainScreen.SetActive(true);
        ChooseLevelScreen.SetActive(false);
        ShopScreen.SetActive(false);
        AchievementScreen.SetActive(false);
        SettingsScreen.SetActive(false);
        CreditsScreen.SetActive(false);
    }

    public void OnLevelSelect(int index)
    {
        SceneManager.LoadScene(index);
    }

    private void CheckLevelUI()
    {
        //Aktiviert Level sterne UI wenn sie im Level eingesammelt wurden
        foreach (GameObject level in LevelChooseUI)
        {
            int levelNumber = int.Parse(level.name.Substring(level.name.Length - 1));
            int CollectedStars = GameManager.Instance.levelList[levelNumber-1].getNumberOfCollectedStars();

            switch(CollectedStars)
            {
                case (0):
                    level.transform.Find("Star1").gameObject.SetActive(false);
                    level.transform.Find("Star2").gameObject.SetActive(false);
                    level.transform.Find("Star3").gameObject.SetActive(false);
                    break;
                case (1):
                    level.transform.Find("Star1").gameObject.SetActive(true);
                    level.transform.Find("Star2").gameObject.SetActive(false);
                    level.transform.Find("Star3").gameObject.SetActive(false);
                    break;
                case (2):
                    level.transform.Find("Star1").gameObject.SetActive(true);
                    level.transform.Find("Star2").gameObject.SetActive(true);
                    level.transform.Find("Star3").gameObject.SetActive(false);
                    break;
                case (3):
                    level.transform.Find("Star1").gameObject.SetActive(true);
                    level.transform.Find("Star2").gameObject.SetActive(true);
                    level.transform.Find("Star3").gameObject.SetActive(true);
                    break;
                case (4):
                    level.transform.Find("Star1").gameObject.SetActive(true);
                    level.transform.Find("Star2").gameObject.SetActive(true);
                    level.transform.Find("Star3").gameObject.SetActive(true);
                    break;
                case (5):
                    level.transform.Find("Star1").gameObject.SetActive(true);
                    level.transform.Find("Star2").gameObject.SetActive(true);
                    level.transform.Find("Star3").gameObject.SetActive(true);
                    break;
                case (6):
                    level.transform.Find("Star1").gameObject.SetActive(true);
                    level.transform.Find("Star2").gameObject.SetActive(true);
                    level.transform.Find("Star3").gameObject.SetActive(true);
                    break;
            }
        }
    }

    private void CheckAchievement()
    {
        // aktiviert oder deaktiviert die achievemnts wenn sie erreicht wurden bzw nicht erreicht wurden
		// AllStars-Achievement
        if(AchievementManager.Instance.GetAchievementAllStarsCompleted() && !AchievementManager.Instance.GetAchievementAllStarsCollected())
        {
            AllStarsButton.gameObject.SetActive(true);
            AllStarsButtonText.text = "Collect";
            AllStarsButton.image.color = Color.green;

        } else if(AchievementManager.Instance.GetAchievementAllStarsCollected())
        {
            AllStarsButton.gameObject.SetActive(false);
			AllStarsButtonText.text = " ";
			AllStarsTrophy.SetActive(true);
        } else
        {
            AllStarsButton.gameObject.SetActive(true);
            AllStarsButtonText.text = "To Do";
            AllStarsButton.image.color = Color.red;
            
        }

        // All Levels Finished
        if (AchievementManager.Instance.GetAchievementAllLevelsCompleted() && !AchievementManager.Instance.GetAchievementAllLevelsCollected())
        {
            AllLevelsButton.gameObject.SetActive(true);
            AllLevelsButtonText.text = "Collect";
            AllLevelsButton.image.color = Color.green;
        }
        else if (AchievementManager.Instance.GetAchievementAllLevelsCollected())
        {
            AllLevelsButton.gameObject.SetActive(false);
            AllLevelsButtonText.text = " ";
            AllLevelsTrophy.SetActive(true);
        }
        else
        {
            AllLevelsButton.gameObject.SetActive(true);
            AllLevelsButtonText.text = "To Do";
            AllLevelsButton.image.color = Color.red;

        }

        // Game Complete
        if (AchievementManager.Instance.GetAchievementGameCompleteCompleted() && !AchievementManager.Instance.GetAchievementGameCompleteCollected())
        {
            GameCompleteButton.gameObject.SetActive(true);
            GameCompleteButtonText.text = "Collect";
            GameCompleteButton.image.color = Color.green;

        }
        else if (AchievementManager.Instance.GetAchievementGameCompleteCollected())
        {
            GameCompleteButton.gameObject.SetActive(false);
            GameCompleteButtonText.text = " ";
            GameCompleteTrophy.SetActive(true);
        }
        else
        {
            GameCompleteButton.gameObject.SetActive(true);
            GameCompleteButtonText.text = "To Do";
            GameCompleteButton.image.color = Color.red;
        }
    }

    private void CheckShopItems()
    {
        //aktiviert oder deaktiviert die shop items zum upgraden je nachdem ob das vorherige gekauft wurde bzw das erste ist standartmäßig an
        //Fueltank Upgrade
        if(FuelTankUpgrade)
        {
            FuelTankUpgradeButton.interactable = false;
        } else
        {
            FuelTankUpgradeButton.interactable = true;
        }

        if(FuelTankUpgradeII || !FuelTankUpgrade)
        {
            FuelTankUpgradeButtonII.interactable = false;
        }
        else
        {
            FuelTankUpgradeButtonII.interactable = true;
        }

        if (FuelTankUpgradeIII || !FuelTankUpgradeII)
        {
            FuelTankUpgradeButtonIII.interactable = false;
        }
        else
        {
            FuelTankUpgradeButtonIII.interactable = true;
        }


        //Consumption Upgrade
        if (ConsumptionUpgrade)
        {
            ConsumptionUpgradeButton.interactable = false;
        }
        else
        {
            ConsumptionUpgradeButton.interactable = true;
        }

        if (ConsumptionUpgradeII || !ConsumptionUpgrade)
        {
            ConsumptionUpgradeButtonII.interactable = false;
        }
        else
        {
            ConsumptionUpgradeButtonII.interactable = true;
        }

        if (ConsumptionUpgradeIII || !ConsumptionUpgradeII)
        {
            ConsumptionUpgradeButtonIII.interactable = false;
        }
        else
        {
            ConsumptionUpgradeButtonIII.interactable = true;
        }
    }


    //Shop Upgrades
    public void OnTankUpgradeI()
    {
        //Upgrade wird im Gamemanager gespeichert wenn genug sterne vorhanden sind und die sterne aktualisiert
        if(GameManager.Instance.getTotalStars() >= FuelTankUpgradeCost)
        {
            GameManager.Instance.removeTotalStars(FuelTankUpgradeCost);
            FuelTankUpgradeButton.interactable = false;
            GameManager.Instance.TankUpgradeI = true;
            FuelTankUpgrade = true;
            GameManager.Instance.SetFuelTankMax(FuelTankUpgradeTo);
            UpdateStarCounter();
            CheckShopItems();
        }
    }

    public void OnTankUpgradeII()
    {
        //Upgrade wird im Gamemanager gespeichert wenn genug sterne vorhanden sind und die sterne aktualisiert
        if (GameManager.Instance.getTotalStars() >= FuelTankUpgradeCostII)
        {
            GameManager.Instance.removeTotalStars(FuelTankUpgradeCostII);
            FuelTankUpgradeButtonII.interactable = false;
            GameManager.Instance.TankUpgradeII = true;
            FuelTankUpgradeII = true;
            GameManager.Instance.SetFuelTankMax(FuelTankUpgradeToII);
            UpdateStarCounter();
            CheckShopItems();
        }
    }

    public void OnTankUpgradeIII()
    {
        //Upgrade wird im Gamemanager gespeichert wenn genug sterne vorhanden sind und die sterne aktualisiert
        if (GameManager.Instance.getTotalStars() >= FuelTankUpgradeCostIII)
        {
            GameManager.Instance.removeTotalStars(FuelTankUpgradeCostIII);
            FuelTankUpgradeButtonIII.interactable = false;
            GameManager.Instance.TankUpgradeIII = true;
            FuelTankUpgradeIII = true;
            GameManager.Instance.SetFuelTankMax(FuelTankUpgradeToIII);
            UpdateStarCounter();
            CheckShopItems();
        }
    }

    public void OnConsumptionUpgradeI()
    {
        //Upgrade wird im Gamemanager gespeichert wenn genug sterne vorhanden sind und die sterne aktualisiert
        if (GameManager.Instance.getTotalStars() >= ConsumptionUpgradeCost)
        {
            GameManager.Instance.removeTotalStars(ConsumptionUpgradeCost);
            ConsumptionUpgradeButton.interactable = false;
            GameManager.Instance.ConsumptionUpgradeI = true;
            ConsumptionUpgrade = true;
            GameManager.Instance.setConsumption(ConsumptionUpgradeTo);
            UpdateStarCounter();
            CheckShopItems();
        }
    }

    public void OnConsumptionUpgradeII()
    {
        //Upgrade wird im Gamemanager gespeichert wenn genug sterne vorhanden sind und die sterne aktualisiert
        if (GameManager.Instance.getTotalStars() >= ConsumptionUpgradeCostII)
        {
            GameManager.Instance.removeTotalStars(ConsumptionUpgradeCostII);
            ConsumptionUpgradeButtonII.interactable = false;
            GameManager.Instance.ConsumptionUpgradeII = true;
            ConsumptionUpgradeII = true;
            GameManager.Instance.setConsumption(ConsumptionUpgradeToII);
            UpdateStarCounter();
            CheckShopItems();
        }
    }

    public void OnConsumptionUpgradeIII()
    {
        //Upgrade wird im Gamemanager gespeichert wenn genug sterne vorhanden sind und die sterne aktualisiert
        if (GameManager.Instance.getTotalStars() >= ConsumptionUpgradeCostIII)
        {
            GameManager.Instance.removeTotalStars(ConsumptionUpgradeCostIII);
            ConsumptionUpgradeButtonIII.interactable = false;
            GameManager.Instance.ConsumptionUpgradeIII = true;
            ConsumptionUpgradeIII = true;
            GameManager.Instance.setConsumption(ConsumptionUpgradeToIII);
            UpdateStarCounter();
            CheckShopItems();
        }
    }

    // Achievements
    public void OnCollectAllStarsAchievement(int amount)
    {
        //einsammeln der Belohnung für das achievement und deaktiviert den dazugehörigen Button
        GameManager.Instance.addTotalStars(amount);
        AchievementManager.Instance.AchievementAllStarsCollecte();
        AllStarsButton.interactable = false;
        AllStarsButtonText.text = "Collected";
        AllStarsButton.image.color  = Color.gray;
        CheckAchievement();
    }

    public void OnCollectAllLevelsAchievement(int amount)
    {
        //einsammeln der Belohnung für das achievement und deaktiviert den dazugehörigen Button
        GameManager.Instance.addTotalStars(amount);
        AchievementManager.Instance.AchievementAllLevelsCollecte();
        AllLevelsButton.interactable = false;
        AllLevelsButtonText.text = "Collected";
        AllLevelsButton.image.color = Color.gray;
        CheckAchievement();
    }

    public void OnCollectGameCompleteAchievement(int amount)
    {
        //einsammeln der Belohnung für das achievement und deaktiviert den dazugehörigen Button
        GameManager.Instance.addTotalStars(amount);
        AchievementManager.Instance.AchievementGameCompleteCollecte();
        GameCompleteButton.interactable = false;
        GameCompleteButtonText.text = "Collected";
        GameCompleteButton.image.color = Color.gray;
        CheckAchievement();
    }


    private void UpdateStarCounter ()
    {
        StarCounterText.text = GameManager.Instance.getTotalStars().ToString();
    }
}
