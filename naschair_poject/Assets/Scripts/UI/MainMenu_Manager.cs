using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_Manager : MonoBehaviour
{
    public static bool isPaused = false;

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void SetSinglePlayerMode (bool setting)
    {
        PlayerManager pm = GameManager.GetPlayerManager();
        pm.singleplayerMode = setting;
    }

    public void OpenSettings()
    {
        SceneManager.LoadSceneAsync("Settings", LoadSceneMode.Additive);
    }

    public void CloseSettings()
    {
        SceneManager.UnloadSceneAsync("Settings");
        SetNewSelected.lookForSelection = true;
    }

    public static void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        SceneManager.LoadSceneAsync("Pause Screen", LoadSceneMode.Additive);

        PlayerManager pm = GameManager.GetPlayerManager();

        GameManager.MouseLock(false);

        if (pm.activePlayers.Count > 0)
            for (int y = 0; y < pm.activePlayers.Count; y++)
            {
                if(pm.activePlayers[y].uiManager != null)
                    pm.activePlayers[y].uiManager.ChangeUiState(false);
            }
    }

    public static void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync("Pause Screen");
        SetNewSelected.lookForSelection = true;

        PlayerManager pm = GameManager.GetPlayerManager();
        GameManager.MouseLock(true);

        if (pm.activePlayers.Count > 0)
            for (int y = 0; y < pm.activePlayers.Count; y++)
            {
                if (pm.activePlayers[y].uiManager != null)
                    pm.activePlayers[y].uiManager.ChangeUiState(true);
            }
    }
}
