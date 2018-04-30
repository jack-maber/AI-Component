using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceUI_Display : MonoBehaviour
{
    public static RaceUI_Display Ref;

    [Header("Countdown")]
    public Text countDownDisplay;
    public float countDownOpacityRange = 0;

    float timeToFade = 0, fadeTimer = 0;
    Color originalCountdownColour, fadeCountDownColour;

    void Awake()
    {
        Ref = this;
    }

    void Start()
    {
        originalCountdownColour = countDownDisplay.color;
        fadeCountDownColour = originalCountdownColour;
        fadeCountDownColour.a = 0;
    }

    void UpdateCountDownDisplay()
    {
        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
        }
        else if (fadeTimer < 0)
        {
            fadeTimer = 0;
        }

        countDownDisplay.color = Vector4.Lerp(originalCountdownColour, fadeCountDownColour, (fadeTimer / timeToFade));
    }

    public void SetCountDown(string newValue, float newTimeToFade)
    {
        countDownDisplay.text = newValue;
        timeToFade = newTimeToFade;
        fadeTimer = timeToFade;
    }
}
