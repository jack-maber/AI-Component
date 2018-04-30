using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : BaseManager
{
    public List<Map_Data> mapData = new List<Map_Data>();
    public Transform contentForMapButtons;

    public Text trackNameDisplay;
    public Image displayImage;

    public Text selectLevelHint, nextCharacterHint, prevCharacterHint;

    PlayerManager pm;
    int selectedMap = 0;

    string startButton, prevButton, nextButton;

    private void Start()
    {
        pm = GameManager.GetPlayerManager();
        UpdateActiveMap();

        startButton = pm.activePlayers[0].playerInstance.controllers.maps.GetFirstElementMapWithAction("StartGame", true).elementIdentifierName;
        prevButton = pm.activePlayers[0].playerInstance.controllers.maps.GetFirstElementMapWithAction("PreviousCharacter", true).elementIdentifierName;
        nextButton = pm.activePlayers[0].playerInstance.controllers.maps.GetFirstElementMapWithAction("NextCharacter", true).elementIdentifierName;
    }

    private void Update()
    {
        for (int i = 0; i < pm.activePlayers.Count; i++)
        {
            bool next = (pm.activePlayers[i].playerInstance.GetButtonDown("NextCharacter"));
            bool previous = (pm.activePlayers[i].playerInstance.GetButtonDown("PreviousCharacter"));

            if (next)
                selectedMap = (selectedMap + 1 >= mapData.Count) ? 0 : selectedMap + 1;

            if (previous)
                selectedMap = (selectedMap - 1 < 0) ? mapData.Count - 1 : selectedMap - 1;

            if (previous || next)
            {
                UpdateActiveMap();
            }

            if (pm.activePlayers[i].playerInstance.GetButtonLongPress("StartGame"))
                LoadMap();
        }

        selectLevelHint.text = "Hold " + startButton + " to select the level";
        prevCharacterHint.text = "Press " + prevButton + " for next level";
        nextCharacterHint.text = "Press" + nextButton + " for previous level";
    }

    void UpdateActiveMap()
    {
        trackNameDisplay.text = mapData[selectedMap].displayName;
        displayImage.sprite = mapData[selectedMap].displayImage;
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(mapData[selectedMap].sceneName);
    }
}
