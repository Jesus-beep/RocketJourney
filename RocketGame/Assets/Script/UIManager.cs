using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//Erstellt: Martin am 16.07
//Bearbeitet: Nina am 18.07: Tank-Verhalten

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameUIObject;
    [SerializeField] private GameObject PauseMenuObject;
    [SerializeField] private GameObject SettingsUIObject;

    [SerializeField] private GameObject Stern1;
    [SerializeField] private GameObject Stern2;
    [SerializeField] private GameObject Stern3;

    
	
	[Header("Fueltank")]
	private Gradient gradient;
	[SerializeField] private Image TankBackground;
	[SerializeField] private Image Fill;
	[SerializeField] private Image Needle;
	[SerializeField] private Image BorderMax;

    [Header("SoundSlider")]
    [SerializeField] private Slider Master;
    [SerializeField] private Slider Effects;
    [SerializeField] private Slider Music;

    [Header("GameUIObjects")]
    [SerializeField] private GameObject MusicButton;
    [SerializeField] private GameObject EffectButton;
    [SerializeField] private Sprite ImageMusicOn;
    [SerializeField] private Sprite ImageMusicOff;
    [SerializeField] private Sprite ImageEffectOn;
    [SerializeField] private Sprite ImageEffectOff;
    [SerializeField] private TextMeshProUGUI StoryUI;
    [SerializeField] private GameObject MessageBox;
    [SerializeField] private float tweenTime;

    private float MasterValue = 0;
    private float EffectsValue = 0;
    private float MusicValue = 0;

    private bool Stern1Active = false;
    private bool Stern2Active = false;
    private bool Stern3Active = false;

	
	private void Start()
    {
        InitFueltank();
        ConnectSlidersToSoundManager();

        //Setzt die Story Box auf minimal klein
        MessageBox.transform.localScale = Vector2.zero;

        //Wenn das Level geladen wird setze die Anzahl an Sterne auf die Anzahl die beim letzten mal gesammelt wurde
        GameManager.Instance.CurrentStars = GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex - 1].getNumberOfCollectedStars();

        //zeigt die Story an wenn man das erste mal ins Level lädt
        if (GameManager.Instance.Tries == 0)
        {
            
            StartCoroutine(ShowStory());
        }
        
    }

    private void ConnectSlidersToSoundManager()
    {
        //Verbindet die Settings-Slider mit dem SoundManager

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

    private void Update()
    {
        //ruft das Menu auf bei 'Esc'
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingsUIObject.activeSelf)
            {
                SettingsUIObject.SetActive(false);
                Time.timeScale = 1;
            }
            else
                ChangeMenuScreen();
        }

		// stars:
        CheckStars();
        ActivateStars();
		
		// fueltank:
		FillFueltank();
    }

	//Bearbeitet: Nina 18.07.   ; Elliot fixed Bug 19.07.
    private void ActivateStars()
    {
        //Setzt die Farbe der Sterne
        if(Stern1Active)
			Stern1.GetComponent<Image>().color = new Color(0.87f, 0.75f, 0f);
		if(Stern2Active)
			Stern2.GetComponent<Image>().color = new Color(0.87f, 0.75f, 0f);
		if(Stern3Active)
			Stern3.GetComponent<Image>().color = new Color(0.87f, 0.75f, 0f);	
    }

    private void CheckStars()
    {
        //aktiviert die sterne wenn welche aufgesammelt wurden
        switch (GameManager.Instance.CurrentStars)
        {
            case (0):
                Stern1Active = false;
                Stern2Active = false;
                Stern3Active = false;
                break;
            case (1):
                Stern1Active = true;
                Stern2Active = false;
                Stern3Active = false;
                break;
            case (2):
                Stern1Active = true;
                Stern2Active = true;
                Stern3Active = false;
                break;
            case (3):
                Stern1Active = true;
                Stern2Active = true;
                Stern3Active = true;
                break;
        }
    }
	
	//Erstellt: Nina, 18.07.
	private void InitFueltank()
	{
		// 100 Liter = 25% gefuellt
		// maximal Upgrade bis 300 Liter (wegen Design)
		
		gradient = new Gradient();
		float maxFuel = GameManager.Instance.GetFuelTankMax();
		float maxFill = maxFuel/400;
		TankBackground.fillAmount = maxFill;
		BorderMax.transform.localEulerAngles = new Vector3(0, 0, -maxFill*360.0f);
		Needle.transform.localEulerAngles = new Vector3(0, 0, -maxFill*360.0f);
		
		// generate the gradient
		float danger = 0.1f * maxFill;
		float critical = 0.2f * maxFill;
		
		GradientColorKey[] colorKey = new GradientColorKey[5];
		colorKey[0].color = new Color(0.52f, 0, 0);
		colorKey[0].time = 0.0f;
		colorKey[1].color = new Color(0.52f, 0, 0);
		colorKey[1].time = danger;		
		colorKey[2].color = new Color(0.87f, 0.75f, 0);
		colorKey[2].time = critical;
		colorKey[3].color = new Color(0, 0.48f, 0);
		colorKey[3].time = maxFill;
		colorKey[4].color = new Color(0, 0.48f, 0);
		colorKey[4].time = 1.0f;
		
		GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
		alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
		
		Fill.fillAmount = maxFill;
		Fill.color = gradient.Evaluate(maxFill);
	}
	
	// Erstellt: Nina, 18.07.
	private void FillFueltank()
	{
		float currentFuel = GameManager.Instance.FuelCurrent;
		float currentFill = currentFuel/400;
		Fill.fillAmount = currentFill;
		Fill.color = gradient.Evaluate(currentFill);
		Needle.transform.localEulerAngles = new Vector3(0, 0, -currentFill*360.0f);
	}

    public void ChangeMenuScreen()
    {
        //setzt die Zeit runter und aktiviert das Pausen Menü
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
        PauseMenuObject.SetActive(!PauseMenuObject.activeSelf);
    }

    public void OnSettings()
    {
        //aktiviert das Settings Menü
        PauseMenuObject.SetActive(false);
        SettingsUIObject.SetActive(true);
    }

    public void OnSettingsBack()
    {
        //geht vom Settings Menü wieder zum Pausen Menü zurück
        SettingsUIObject.SetActive(false);
        PauseMenuObject.SetActive(true);
    }

    public void OnExit()
    {
        //geht ins Hauptmenü zurück und setzt die gespeicherten Sterne und Versuche runter
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(0);

        GameManager.Instance.CurrentStars = 0;
        GameManager.Instance.Tries = 0;
    }

    public void OnChangeMusic()
    {
        //Passt die Musik an. Wenn sie an ist wird sie aus gemacht und wenn sie aus ist wird sie an gemacht
        if(MusicButton.GetComponent<Image>().sprite.name == "Music_off")
        {
            //wenn die Musik aus ist wird sie wieder auf dem selben level wie vorher gelegt
            MusicButton.GetComponent<Image>().sprite = ImageMusicOn;
            SoundManager.Instance.mixer.SetFloat("Music", MusicValue);
            Music.value = MusicValue;

        } else if (MusicButton.GetComponent<Image>().sprite.name == "Music")
        {
            //schaltet die Musik aus
            MusicButton.GetComponent<Image>().sprite = ImageMusicOff;
            SoundManager.Instance.mixer.SetFloat("Music", -80);
            SoundManager.Instance.changeImage = false;
            Music.value = -80;
            SoundManager.Instance.changeImage = true;
        }
    }

    public void activateMusic()
    {
        // das sprite an damit angezeigt wird dass die Musik an ist
        MusicButton.GetComponent<Image>().sprite = ImageMusicOn;
    }

    public void OnChangeSoundEffect()
    {
        //Passt die Effekte an. Wenn sie an ist wird sie aus gemacht und wenn sie aus ist wird sie an gemacht
        if (EffectButton.GetComponent<Image>().sprite.name == "Sound_off")
        {
            //wenn die Effekte aus sind wird sie wieder auf dem selben level wie vorher gelegt
            EffectButton.GetComponent<Image>().sprite = ImageEffectOn;
            SoundManager.Instance.mixer.SetFloat("Effects", EffectsValue);
            Effects.value = EffectsValue;

        }
        else if (EffectButton.GetComponent<Image>().sprite.name == "Sound")
        {
            //schaltet die Effekte aus
            EffectButton.GetComponent<Image>().sprite = ImageEffectOff;
        
            SoundManager.Instance.mixer.SetFloat("Effects", -80);
            SoundManager.Instance.changeImage = false;
            Effects.value = -80;
            SoundManager.Instance.changeImage = true;
        }
    }

    public void activateEffects()
    {
        // das sprite an damit angezeigt wird dass die Effekte an sin
        EffectButton.GetComponent<Image>().sprite = ImageEffectOn;
    }

    private IEnumerator ShowStory()
    {
        //zieht den Storytext aus dem Level
        StoryUI.text = GameManager.Instance.levelList[SceneManager.GetActiveScene().buildIndex - 1].getStoryText();
        
        //fading in
        TweenIn();

        yield return new WaitForSeconds(9);

        //fading out
        TweenOut();
    }

    private void TweenIn()
    {
        //lässt die Story Box größer werden (Animation)
        MessageBox.transform.LeanScale(new Vector2(0.6f, 0.6f), tweenTime);
    }

    private void TweenOut()
    {
        //lässt die Story Box wieder verschwinden
        MessageBox.transform.LeanScale(Vector2.zero, tweenTime);
    }
}
