using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Erstellt: Martin am 16.07
//Bearbeitet: Nina am 13.08.

[System.Serializable]
public class Level
{
    //enthält die wichtigsten Infos zu dem Level
    //speichert die Infos

    [SerializeField] private int numberOfCollectedStars = 0;
    [SerializeField] private int numberOfFinishes = 0;
    [TextArea]
    [SerializeField] private string StoryString;

	private bool star1collected = false;
	private bool star2collected = false;
	private bool star3collected = false;

    public int getNumberOfCollectedStars()
    {
        //gibt die anzahl an sternen die bereits gesammlt wurden wieder
        return numberOfCollectedStars;
    }

    public int getNumberOfFinishes()
    {
        //anzahl der erfolgreichen durchläufe
        return numberOfFinishes;
    }

    public void setNumberOfCollectedStars(int amount)
    {
        //anzahl an sterne die im level gesammelt wurden
        if (numberOfCollectedStars + amount > 3)
        {
            numberOfCollectedStars = 3;
        } else
        {
            numberOfCollectedStars += amount;
        }
    }

    public void addNumberOfFinishes(int amount)
    {
        //erhöht die anzahl an durchläufen die der Spieler geschafft hat
        numberOfFinishes += amount;
    }

    public string getStoryText()
    {
        //gibt den story text für das Level zurück
        return StoryString;
    }
	
	//Nina: Hilfsfunktionen um die jeweiligen Sterne zu untersuchen
	public void setStar1collected(bool collected)
	{
		star1collected = collected;
	}
	
	public void setStar2collected(bool collected)
	{
		star2collected = collected;
	}
	
	public void setStar3collected(bool collected)
	{
		star3collected = collected;
	}
	
	public bool getStar1collected()
	{
		return star1collected;
	}
	
	public bool getStar2collected()
	{
		return star2collected;
	}
	
	public bool getStar3collected()
	{
		return star3collected;
	}
}
