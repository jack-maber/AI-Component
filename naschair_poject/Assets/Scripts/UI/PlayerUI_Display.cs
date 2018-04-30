using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI_Display : MonoBehaviour
{
    [Header("General")]
    public PlayerClass playerClass;
    public Canvas canvas;

    [Header("Lap and Placement")]
    public Text placement;
    public Text lap;

    [Header("Power up display")]
    public GameObject powerUpRoot;
    public Image powerUpIcon;
    public Text powerUpHint;
    public Sprite boostIcon;
    public Sprite satchelIcon;
    public Sprite frisbeeIcon;
    public Sprite shieldIcon;

    [Header("Push")]
    public Color pushReady;
    public Color pushCharging;
    public string pushReadyMessage = "Ready";
    public string pushChargingMessage = "Wait";
    public Image pushIcon;
    public Text pushText;
    public Image border;
    public Color borderColour;
    public float borderDecaySpeed = 5;

    float borderDecayRange = 0;
    Color alphaZero;

    [Header("Turn Around")]
    public GameObject turnAroundRoot;

    int lastPlacment = 100, lastLap = 100;

    RaceManager rm;

    public void ChangeUiState(bool enabled = true)
    {
        canvas.enabled = enabled;
    }

    private void Start()
    {
        rm = GameManager.GetRaceManager();
        alphaZero = borderColour;
        alphaZero.a = 0;
    }

    public void Update()
    {
        UpdateLapAndPlacement();
        UpdatePowerUpDisplay();
        UpdateBorder();
    }

    void UpdateBorder()
    {
        if (borderDecayRange <= 0)
            return;

        borderDecayRange -= Time.deltaTime / borderDecayRange;
        border.color = Color.Lerp(alphaZero, borderColour, borderDecayRange);
    }

    void UpdateLapAndPlacement()
    {
        if (playerClass.position != lastPlacment)
        {
            lastPlacment = playerClass.position;
            switch (playerClass.position)
            {
                case 1:
                    placement.text = "1st";
                    break;

                case 2:
                    placement.text = "2nd";
                    break;

                case 3:
                    placement.text = "3rd";
                    break;

                default:
                    placement.text = playerClass.position + "th";
                    break;
            }
        }

        if (playerClass.lap != lastLap)
        {
            lastLap = playerClass.lap;
            lap.text = playerClass.lap + "/" + rm.raceData.lapCount;
        }
    }

    public void UpdatePlayerClass(PlayerClass p)
    {
        playerClass = p;
    }

    public void UpdateCamera(Camera c)
    {
        canvas.worldCamera = c;
        canvas.planeDistance = 1f;
    }

    public void ChangePushDisplay(bool canPush, float range)
    {
        Color pushIconColour = (playerClass.controller.showPushIcon) ? pushReady : pushCharging;
        string newText = (playerClass.controller.showPushIcon) ? pushReadyMessage : pushChargingMessage;

        pushIcon.color = pushIconColour;
        pushText.text = newText;
        pushIcon.fillAmount = range;
    }

    public void UpdatePowerUpDisplay()
    {
        switch (playerClass.pickUpManager.currentPickupType)
        {
            case "Boost":
                powerUpIcon.sprite = boostIcon;
                break;
            case "SatchelCharge":
                powerUpIcon.sprite = satchelIcon;
                break;
            case "Frisbee":
                powerUpIcon.sprite = frisbeeIcon;
                break;
            case "Shield":
                powerUpIcon.sprite = shieldIcon;
                break;
            default:
                powerUpRoot.SetActive(false);
                return;
        }

        if (!powerUpRoot.activeSelf)
            powerUpRoot.SetActive(true);

        string input = playerClass.playerInstance.controllers.maps.GetFirstElementMapWithAction("Use", true).elementIdentifierName;
        powerUpHint.text = "Hold: " + input + " to use " + playerClass.pickUpManager.currentPickupType;
    }

    public void ShowTurnAround(bool show = true)
    {
        Debug.Log(turnAroundRoot == null);
        turnAroundRoot.SetActive(show);
    }

    public void ShowGoodPush()
    {
        borderDecayRange = 1;
    }
}
