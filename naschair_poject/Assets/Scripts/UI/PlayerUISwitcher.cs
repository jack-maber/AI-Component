using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUISwitcher : MonoBehaviour
{
    public int id;
    PlayerManager pm;
    bool isEnabled = false, wasEnabled = false;
    public FixedCamera cam;

    public GameObject staticObject, selectedObject;

    private void Start()
    {
        pm = GameManager.GetPlayerManager();
    }

    private void Update()
    {
        isEnabled = (pm.activePlayers.Count > id);

        if (wasEnabled != isEnabled)
        {
            staticObject.SetActive(!isEnabled);
            selectedObject.SetActive(isEnabled);
            cam.SetTarget(pm.activePlayers[id].controller.spaceTracker);
        }

        wasEnabled = isEnabled;
    }
}
