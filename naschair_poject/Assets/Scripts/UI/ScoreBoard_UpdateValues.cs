using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard_UpdateValues : MonoBehaviour 
{
    public List<ScoreCard> scoreCards = new List<ScoreCard>();

    RaceManager rm;

    void Start()
    {
        UpdateBoard();
    }

    public void UpdateBoard()
    {
        rm = GameManager.GetRaceManager();

        //Disable all cards first
        for(int i = 0; i < scoreCards.Count; i++)
        {
            scoreCards[i].DisableCard();
        }

        rm.allRacers.racerCollection.Sort((x, z) => x.placment.CompareTo(z.placment));

        for (int i = 0; i < rm.allRacers.racerCollection.Count; i++)
        {
            scoreCards[i].EnableCard();
            scoreCards[i].UpdatePlayerData(rm.allRacers.racerCollection[i]);
        }
    }
}
