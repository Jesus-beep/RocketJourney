using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Erstellt: Martin am 10.07

public class Fueltank : MonoBehaviour
{
    [SerializeField] private int FuelAmount;
    [SerializeField] private AudioClip CollectFuelTankSound;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        //Überprüft ob der Spieler in den Bezinkanister fliegt
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Mover>().addFuel(FuelAmount);
            StartCoroutine(PlaySound());
        }
    }
    private IEnumerator PlaySound()
    {
        //spielt den Sound ab
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(CollectFuelTankSound, GetComponent<AudioSource>().volume);
        yield return new WaitForSeconds(CollectFuelTankSound.length);
        Destroy(this.gameObject);
    }
}
