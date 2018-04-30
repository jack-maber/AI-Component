using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using FMOD;
using FMODUnity;

public class PlayerController : ChairMotor
{
    [HideInInspector]
    public PlayerClass profile;
    float resetTimer = 0;
    public Player playerInputObject = null; //Changed by Tom so that I could implement the PlayerManager/PlayerSpawner.
    public int overideID = 0; //If playerInputObject is null then use this to get the player id.
    public CameraFOV camFov;

    public override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        if (playerInputObject == null)
            playerInputObject = ReInput.players.GetPlayer(overideID); //TEMP get the player based on ID
    }

    public override void Update()
    {
        base.Update();

        if(camFov != null)
            camFov.SetFovRange(currentSpeed / 70);

        if(profile.uiManager != null)
            profile.uiManager.ChangePushDisplay(showPushIcon, chargeRange);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        GetInput();
    }

    void GetInput()
    {
        Vector3 vec3 = Vector3.zero; //Create a basic store for all vector3 data to keep the code neat.

        vec3.x = playerInputObject.GetAxis("Hor_Movment");
        vec3.y = 0; //Set the y to zero when not using it to make sure when we reuse this variable we dont have the last y value set.
        vec3.z = playerInputObject.GetAxis("Ver_Movment");
        inputVaraibles.controllerMovment = vec3;    //Send the variable over to the ChairMotor input class.

        vec3.x = playerInputObject.GetAxis("Hor_Rotation");
        vec3.y = 0;
        vec3.z = playerInputObject.GetAxis("Ver_Rotation");
        inputVaraibles.controllerRotation = vec3;   //Send the variable over to the ChairMotor input class.

        bool boolean = false; //Create a basic boolean so we can store the input as a variable to keep the code neat.


        boolean = playerInputObject.GetButtonDown("Push");
        inputVaraibles.pushPressed = boolean;

        boolean = playerInputObject.GetButton("Brake");
        inputVaraibles.brakePressed = boolean;  //Send the variable over to the ChairMotor input class.

        if (playerInputObject.GetButton("ResetPosRot")) //If the reset button is being pressed then start counting.
            resetTimer += Time.deltaTime;

        if (!playerInputObject.GetButton("ResetPosRot")) //If the reset button is not being pressed then reset the timer back to 0
            resetTimer = 0;

        if (playerInputObject.GetButtonDown("FlipCamera") || playerInputObject.GetButtonUp("FlipCamera"))
            profile.cam.InverseCameraView();

        inputVaraibles.jumpPressed = playerInputObject.GetButtonDown("Jump");

        boolean = playerInputObject.GetButtonDown("Use"); //Detects use of the pickup button.
        inputVaraibles.usePickupPressed = boolean;

        boolean = playerInputObject.GetButton("Use"); //Detects use of the pickup button.
        inputVaraibles.usePickupHeld = boolean;

        inputVaraibles.resetTimer = resetTimer; //Send the variable over to the ChairMotor input class.
    }

    public override IEnumerator Push(float speed)
    {
        if (profile.uiManager != null & speed == motorData.fastPushSpeed)
            profile.uiManager.ShowGoodPush();

        return base.Push(speed);
    }
}
