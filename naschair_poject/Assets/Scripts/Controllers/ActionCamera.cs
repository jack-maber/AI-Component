using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCamera : MonoBehaviour
{
    public CameraController camCtrl;

    RaceManager rm;

    private void Start()
    {
        rm = GameManager.GetRaceManager();
        camCtrl = GetComponent<CameraController>();
    }

    private void Update()
    {
        Transform newTarget = rm.GetFirstActiveRacer();

        if(newTarget != null)
            camCtrl.target = newTarget;
    }
}
