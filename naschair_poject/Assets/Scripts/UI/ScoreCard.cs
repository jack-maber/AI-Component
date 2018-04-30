using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	

public class ScoreCard : MonoBehaviour 
{
	public Image characterImage;
    public Image cardBackground;
	public Text characterName;
	public Text placement;
	public Text score;

	public void EnableCard()
	{
		characterImage.enabled = true;
		characterName.enabled = true;
		placement.enabled = true;
		score.enabled = true;
	}

	public void DisableCard()
	{
		characterImage.enabled = false;
		characterName.enabled = false;
		placement.enabled = false;
		score.enabled = false;
	}

	public void UpdatePlayerData(Racers racerData)
	{
        if(racerData != null) //Check if the racer is null (This happens when using local start in engine)
        {
            UpdateCharacterImage(racerData.character.displayImage);
            UpdateCharacterName(racerData.displayName);
        }

        if(racerData.playerClass != null)
            cardBackground.color = new Color(0,.3f,0,1);

		UpdatePlacement(racerData.placment.ToString());

        if(racerData.playerClass != null)
		    UpdateScore(racerData.score);
    }

	public void UpdateCharacterImage(Sprite newImage)
	{	
		characterImage.sprite = newImage;
	}

	public void UpdateCharacterName(string newName)
	{	
		characterName.text = newName;
	}

	public void UpdatePlacement(string newPlacement)
	{	
		placement.text = newPlacement;
	}	

	public void UpdateScore(int newScore)
	{	
		score.text = newScore.ToString();
	}	
}
