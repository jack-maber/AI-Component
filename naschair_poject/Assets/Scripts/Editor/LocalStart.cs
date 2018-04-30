using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rewired;
using UnityEngine.SceneManagement;

public static class LocalStart
{
    [MenuItem("Naschair/Local Start/One Player")]
    public static void StartOnePlayer()
    {
        StartGame(1);
    }

    [MenuItem("Naschair/Local Start/Two Player")]
    public static void StartTwoPlayer()
    {
        StartGame(2);
    }

    [MenuItem("Naschair/Local Start/Three Player")]
    public static void StartThreePlayer()
    {
        StartGame(3);
    }

    [MenuItem("Naschair/Local Start/Four Player")]
    public static void StartFourPlayer()
    {
        StartGame(4);
    }

    [MenuItem("Naschair/Launch from player select")]
    public static void LaunchPlayerSelect()
    {
        SceneManager.LoadScene("PlayerSelect");
    }

    public static void StartGame(int count)
    {
        AddPlayers(count);
    }

    public static void AddPlayers(int playerCount)
    {
        PlayerManager pm = GameManager.GetPlayerManager();

        for (int i = 0; i < playerCount; i++)
        {
            PlayerClass newPlayer = new PlayerClass();
            newPlayer.playerInstance = ReInput.players.GetPlayer(i);
            newPlayer.characterProfile = pm.characters.characters[0];
            pm.AddPlayer(newPlayer);
        }
    
        RaceManager rm = GameManager.GetRaceManager();
        if (rm != null)
            rm.BeginStart();
    }
}
