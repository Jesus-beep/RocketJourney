using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Erstellt: Martin am 10.07
public class Mover : MonoBehaviour
{
    // Bewegungs Skript für die rakete

    [SerializeField] private float rotationspeed = 2f;
    [SerializeField] private float thrustspeed = 1;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private ParticleSystem mainEngineParticle;
    [SerializeField] private ParticleSystem leftEngineParticle;
    [SerializeField] private ParticleSystem rightEngineParticle;

    [SerializeField] private float MaxFuel;
    [SerializeField] private float CurrentFuel;
    [SerializeField] private float consumptionSpeed = 20f;

    private AudioSource audiosource;
    private Rigidbody rigb;
    

    void Start()
    {
        Init();
    }

    private void Init()
    {
        //Initialisiert die Rakete
        MaxFuel = GameManager.Instance.GetFuelTankMax();
        consumptionSpeed = GameManager.Instance.getConsumption();
        rigb = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
        CurrentFuel = MaxFuel;
        GameManager.Instance.FuelCurrent = CurrentFuel;
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space) && CurrentFuel > 0)
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        //wenn schub gegeben wird
        rigb.AddRelativeForce(Vector3.up * thrustspeed * Time.deltaTime);

        if (!mainEngineParticle.isPlaying)
        {
            mainEngineParticle.Play();
        }

        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine);
        }
        //reduziert benzinmenge
        CurrentFuel -= Time.deltaTime * consumptionSpeed;
        GameManager.Instance.FuelCurrent = CurrentFuel;
    }

    private void StopThrusting()
    {
        //wenn kein schub gegeben wird
        mainEngineParticle.Stop();
        audiosource.Stop();
    }

    void ProcessRotation()
    {
        //Frägt die Nutzereingaben ab
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
        else
        {
            rightEngineParticle.Stop();
            leftEngineParticle.Stop();
        }
    }

    private void TurnLeft()
    {
        //rakete nach links drehen
        if (!rightEngineParticle.isPlaying)
        {
            rightEngineParticle.Play();
        }
        ApplyRotation(rotationspeed);
    }

    private void TurnRight()
    {
        //rakete nach rechts drehen
        if (!leftEngineParticle.isPlaying)
        {
            leftEngineParticle.Play();
        }
        ApplyRotation(-rotationspeed);
    }

    private void ApplyRotation(float rotationthisFrame)
    {
        rigb.freezeRotation = true; 
        transform.Rotate(Vector3.forward * rotationthisFrame * Time.deltaTime, Space.World);
        rigb.freezeRotation = false; 
    }

    public void addFuel(int amount)
    {
        //hinzufügen von Benzin 
        CurrentFuel += amount;

        if (CurrentFuel > MaxFuel)
            CurrentFuel = MaxFuel;
        GameManager.Instance.FuelCurrent = CurrentFuel;     // Elliot; 22.07.
    }
}
