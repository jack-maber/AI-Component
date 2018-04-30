using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : BaseManager
{  
    public List<PlayerClass> activePlayers = new List<PlayerClass>(); //List of player profiles. 
    public PlayerSpawner spawner; //Reference to the playerspawner which can spawn player objects.
    public bool singleplayerMode = false;

    public Character_Data characters;

    public bool[] playerSlots = new bool[4];

    public void Start()
    {
        spawner = GetComponent<PlayerSpawner>();
    }
  
    public void AddPlayer (PlayerClass newPlayer)
    {
        int newID = GetNewPlayerID();
        newPlayer.playerID = newID;
        playerSlots[newID] = true;
        activePlayers.Add(newPlayer);
    }

    //public void RemovePlayer(PlayerClass leavingPlayer)
    //{
    //    int leavingID = leavingPlayer.playerID;
    //    playerSlots[leavingID] = false;
    //    activePlayers.Remove(leavingPlayer);
    //    Debug.Break();
    //}

    public bool PlayerAlreadyPlaying (Player player)
    {
        bool playerFound = false;

        foreach (PlayerClass profile in activePlayers)
        {
            if (profile.playerInstance == player) //If the rewired player object matches another already in play then the player is considered to already be playing.
            {
                playerFound = true;
            }
        }

        return playerFound;
    }

    //public void SetSinglePlayerMode(bool setting)
    //{
    //    singleplayerMode = setting;
    //}

    private int GetNewPlayerID()
    {
        int newPlayerID = 0;
        int index = 0;

        foreach (bool slot in playerSlots)
        {
            if (slot == false)
            {
                newPlayerID = index;
                break;
            }

            index++;

        }

        return newPlayerID;

    }

    public PlayerClass GetPlayerClassByPrefab (GameObject prefab)
    {
        PlayerClass matchedPlayer = null; 

        foreach(PlayerClass pc in activePlayers)
        {
            if ((pc.activePlayerPrefab.GetInstanceID()) == (prefab.GetInstanceID()))
            {
                matchedPlayer = pc;
            }
        }

        return matchedPlayer;

    }

    public void ClearPlayers()
    {
        activePlayers.Clear();
    }

}
