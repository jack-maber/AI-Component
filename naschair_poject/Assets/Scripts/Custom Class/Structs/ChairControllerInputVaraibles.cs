using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ChairControllerInputVaraibles
{
    public Vector3 controllerMovment;
    public Vector3 controllerRotation;
    public bool pushPressed;
    public bool brakePressed;
    public bool landBoostPressed;
    public bool usePickupPressed; //Detects the individual pressing of the pickup button.
    public bool usePickupHeld; //Detects when the pickup button is being held.
    public float resetTimer;
    public bool jumpPressed;
}
