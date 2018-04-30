using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    public Camera cam;
    float normalFOV = 60;
    float maxFOV = 80;
    float range = 0;

    private void Update()
    {
        cam.fieldOfView = Mathf.Lerp(normalFOV, maxFOV, range);
    }

    public void SetFovRange(float newRange)
    {
        range = newRange * 2;
    }
}
