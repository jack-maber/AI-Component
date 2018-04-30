using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	public static GameManager _instance;
	public static PlayerManager playerManager;
	public static LapManager lapManager;
	public static MapManager mapManager;
	public static RaceManager raceManager;
    public static InputModuleManager inputModuleManager;
    public static PickUpManager pickUpManager;

    public void Awake() 
	{
        if (_instance == null)
		{
			_instance = this;
            GameObject.DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(gameObject);
		}

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(playerManager != null)
            CheckForPause();
    }

    void CheckForPause()
    {
        for (int i = 0; i < playerManager.activePlayers.Count; i++)
        {
            if (playerManager.activePlayers[i].playerInstance.GetButtonDown("Pause"))
            {
                if(MainMenu_Manager.isPaused == false)
                    MainMenu_Manager.Pause();
                else
                    MainMenu_Manager.UnPause();
            }
        }
    }

    public static void UpdateInstance(System.Object newInstance)
    {
        if (newInstance.GetType() == typeof(PlayerManager))
            playerManager = (PlayerManager)newInstance;

        if (newInstance.GetType() == typeof(LapManager))
            lapManager = (LapManager)newInstance;

        if (newInstance.GetType() == typeof(MapManager))
            mapManager = (MapManager)newInstance;

        if (newInstance.GetType() == typeof(RaceManager))
            raceManager = (RaceManager)newInstance;

        if (newInstance.GetType() == typeof(InputModuleManager))
            inputModuleManager = (InputModuleManager)newInstance;

        if (newInstance.GetType() == typeof(PickUpManager))
        {
            pickUpManager = (PickUpManager)newInstance;
        }
        //Debug.Log("Instance Updated[" + newInstance + "]");
    }

    public static void ClearInstance(System.Object newInstance)
    {
        if (newInstance.GetType() == typeof(PlayerManager))
            playerManager = null;

        if (newInstance.GetType() == typeof(LapManager))
            lapManager = null;

        if (newInstance.GetType() == typeof(MapManager))
            mapManager = null;

        if (newInstance.GetType() == typeof(RaceManager))
            raceManager = null;

        if (newInstance.GetType() == typeof(InputModuleManager))
            inputModuleManager = null;

        if (newInstance.GetType() == typeof(PickUpManager))
        {
            pickUpManager = null;
        }
        //Debug.Log("Cleared Updated[" + newInstance + "]");
    }

    public static bool CheckInstanceIsSet(Object instanceToCheck)
    {
        if (instanceToCheck.GetType() == typeof(PlayerManager))
            return playerManager != null;

        if (instanceToCheck.GetType() == typeof(LapManager))
            return lapManager != null;

        if (instanceToCheck.GetType() == typeof(MapManager))
            return mapManager != null;

        if (instanceToCheck.GetType() == typeof(RaceManager))
            return raceManager != null;

        if (instanceToCheck.GetType() == typeof(InputModuleManager))
            return inputModuleManager != null;

        if (instanceToCheck.GetType() == typeof(PickUpManager))
            return pickUpManager != null;

        return false;
    }

    public static PlayerManager GetPlayerManager()
    {
        if (playerManager == null)
        {
            //Debug.Break();
            Debug.LogError("No Player Manager found");
        }

        return playerManager;
    }

    public static LapManager GetLapManager()
    {
        if (lapManager == null)
        {
            //Debug.Break();
            Debug.LogError("No Lap Manager found");
        }

        return lapManager;
    }

    public static MapManager GetMapManager()
    {
        if (mapManager == null)
        {
           // Debug.Break();
            Debug.LogError("No Map Manager found");
        }

        return mapManager;
    }

    public static RaceManager GetRaceManager()
    {
        if (raceManager == null)
        {
            //Debug.Break();
            Debug.LogError("No Race Manager found");
        }

        return raceManager;
    }

    public static InputModuleManager GetInputModuleManager()
    {
        if (inputModuleManager == null)
        {
            //Debug.Break();
            Debug.LogError("No InputModule Manager found");
        }

        return inputModuleManager;
    }

    public static PickUpManager GetPickUpManager()
    {
        if (pickUpManager == null)
        {
            //Debug.Break();
            Debug.LogError("No PickUp Manager found");
        }

        return pickUpManager;
    }

    public static void MouseLock(bool locked = true)
    {
        Cursor.visible = !locked;

        if (!Cursor.visible)
            Cursor.lockState = (locked) ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
