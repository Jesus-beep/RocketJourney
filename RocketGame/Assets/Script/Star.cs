using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Erstellt: Martin am 16.07
//Bearbeitet: Nina am 13.08.

public class Star : MonoBehaviour
{
    [SerializeField] private AudioClip CollectStarSound;

    private void OnTriggerEnter(Collider other)
    {
        //wenn der Spieler den Stern aufsammelt
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.CurrentStars += 1;
			GameManager.Instance.HitStar(this.name);
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        //spielt den pickup sound ab wenn der Spieler über den Stern fliegt und deaktiviert den Stern
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<AudioSource>().PlayOneShot(CollectStarSound, GetComponent<AudioSource>().volume);
        yield return new WaitForSeconds(CollectStarSound.length);
        Destroy(this.gameObject);
    }
}
