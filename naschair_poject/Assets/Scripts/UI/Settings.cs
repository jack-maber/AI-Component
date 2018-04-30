using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Dropdown graphicsSettings, resolutionSettings;

    List<string> graphicSettingsNames = new List<string>();
    List<string> resSettingsNames = new List<string>();
    Dictionary<string, Resolution> resSettings = new Dictionary<string, Resolution>();

    private void Start()
    {
        SetUpDropDown(QualitySettings.names, graphicSettingsNames, graphicsSettings, QualitySettings.GetQualityLevel());
        SetUpScreenSettings();
    }

    void SetUpScreenSettings()
    {
        List<string> res = new List<string>();

        foreach (Resolution r in Screen.resolutions)
        {
            string newName = r.width + "x" + r.height + "*" + r.refreshRate;
            if (!resSettings.ContainsKey(newName))
            {
                resSettings.Add(newName, r);
                resSettingsNames.Add(newName);
            }
        }

        int activeID = 0;

        for (int i = 0; i < resSettingsNames.Count; i ++)
        {
            int width = resSettings[resSettingsNames[i]].width;
            int height = resSettings[resSettingsNames[i]].height;
            int refreshRate = resSettings[resSettingsNames[i]].refreshRate;

            if (Screen.currentResolution.width == width && Screen.currentResolution.height == height && Screen.currentResolution.refreshRate == refreshRate)
                activeID = i;
        }

        SetUpDropDown(res.ToArray(), resSettingsNames, resolutionSettings, activeID);
    }

    void SetUpDropDown(string[] names, List<string> stringList, Dropdown dropDown, int activeID)
    {
        foreach (string s in names)
        {
            stringList.Add(s);
        }

        dropDown.ClearOptions();
        dropDown.AddOptions(stringList);

        dropDown.value = activeID;
    }

    public void UpdateGrapicsOptions(int option)
    {
        QualitySettings.SetQualityLevel(option, true);
    }

    public void UpdateScreenSettings(int option)
    {
        int width = resSettings[resSettingsNames[option]].width;
        int height = resSettings[resSettingsNames[option]].height;
        int refreshRate = resSettings[resSettingsNames[option]].refreshRate;

        Screen.SetResolution(width, height, true, refreshRate);
    }
}
