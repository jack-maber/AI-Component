using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostDisplay : MonoBehaviour
{
    public BoostData boostData;
    public Transform uiRoot;
    public GameObject boostMeter;
    public RectTransform dialObject;
    public bool showMeter = true;

    [Header("Dial Start Transform")]
    public Vector3 startPosition;
    public Vector3 startRotation;

    [Header("Dial End Transform")]
    public Vector3 endPosition;
    public Vector3 endRotation;

    List<Image> displayImages = new List<Image>();
    Image dialImage;

    [Range(0,1)]
    public float dialPosition = 1;

    public void SetUpUI(BoostData data)
    {
        boostData = data;
        CalculateNewFillAmount(boostData);
        displayImages.Clear();
        dialImage = dialObject.GetComponent<Image>();

        for (int i = boostData.boostSegment.Count - 1; i >= 0; i--)
        {
            GameObject clone = SetUpUIObject(boostData.boostSegment[i]);
            Image image = clone.GetComponent<Image>();
            RefreshUI(image, i);
            displayImages.Add(image);
        }
    }

    GameObject SetUpUIObject(BoostSegment boostSeg)
    {
        string newName = boostSeg.boostType.ToString() + "[" + boostSeg.percentage + "]";
        GameObject clone = Instantiate(boostMeter, uiRoot);

        RectTransform rect = clone.GetComponent<RectTransform>();
        rect.localPosition = Vector3.zero;
        rect.localScale = new Vector3(1,1,1);

        clone.name = newName;
        return clone;
    }

    public void RefreshUI(Image image, int segmentId)
    {
        image.fillAmount = boostData.boostSegment[segmentId].fill / 100;
        image.color = boostData.boostSegment[segmentId].colour;
    }

    public void UpdateDialPosition(float range)
    {
        dialObject.transform.localPosition = Vector3.Lerp(startPosition, endPosition, range);
        dialObject.transform.localEulerAngles = Vector3.Lerp(startRotation, endRotation, range);
    }

    public static BoostData CalculateNewFillAmount(BoostData newData)
    {
        for (int i = 0; i < newData.boostSegment.Count; i++)
        {
            float fillAmount = newData.boostSegment[i].percentage;
            float lowerLimit = 0;
            float upperLimit = newData.boostSegment[i].percentage;
            BoostSegment newSeg = newData.boostSegment[i];

            if (i != 0)
            {
                lowerLimit = newData.boostSegment[i - 1].fill;
                fillAmount = newData.boostSegment[i].percentage + newData.boostSegment[i - 1].fill;
                upperLimit = fillAmount;
            }

            //Save the new percentage so we can set the correct fill amount regardless of the order we spawn the ui elements
            newSeg.fill = fillAmount;
            newSeg.lowerLimit = lowerLimit;
            newSeg.upperLimit = upperLimit;
            newData.boostSegment[i] = newSeg;
        }

        return newData;
    }

    public static BoostTypes GetBoostType(BoostData newData, float dialProgress)
    {
        dialProgress = dialProgress * 100;

        for (int i = 0; i < newData.boostSegment.Count; i++)
        {
            if (dialProgress > newData.boostSegment[i].lowerLimit && dialProgress < newData.boostSegment[i].upperLimit)
                return newData.boostSegment[i].boostType;
        }

        return BoostTypes.Normal;
    }

    public void ShowHideBoostDisplay(bool show = false)
    {
        showMeter = show;

        for (int i = 0; i < displayImages.Count; i++)
        {
            displayImages[i].enabled = show;
            dialImage.enabled = show;
        }
    }
}
