using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Boost : PickUpBehaviour {

    private Rigidbody rb;
    private float boostTime;
    private float boostForce;
    private bool boostActive = false; //Tracks when boost is to be applied.
    private bool animationPlaying = false;

    public void Start()
    {
        boostTime = pickUpData.boostDuration;
        boostForce = pickUpData.boostForce;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        switch (pickUpData.boostMode)
        {
            case PickUp_Data.BoostMode.Uncontrolled:
                if (motor.inputVaraibles.usePickupPressed & !boostActive)
                {
                    boostActive = true; //One shot use detection.
                    Debug.Log("Using uncontrolled boost!");
                }
                break;
            case PickUp_Data.BoostMode.Controlled:
                if (motor.inputVaraibles.usePickupHeld)
                {
                    boostActive = true; //Only boosts while the button is held down.
                    motor.pushDisabled = true;
                    //ANIM: Play particle effect.
                    //animControl.TriggerItemLayer();
                    //animControl.UpdateBoost(true);
          
                    //Debug.Log("Using controlled boost!");

                } else if (!motor.inputVaraibles.usePickupHeld)
                {
                    boostActive = false;
                    motor.pushDisabled = false;
                    //ANIM: Stop particle effect.

                    //animControl.UpdateBoost(false);
                }
                break;
            default:
                break;
        }

        if (animationPlaying != boostActive)
        {
            animationPlaying = !animationPlaying;
            if (animationPlaying)
            {
                animControl.TriggerItemLayer();
                animControl.UpdateBoost(true);
            } else
            {
                animControl.UpdateBoost(false);
            }

        }

        if (boostTime <= 0f)
        {
            Debug.Log("Boost used up!");
            animControl.UpdateBoost(false);
            RemovePickup();

        }


    }

    private void FixedUpdate()
    {

        switch (pickUpData.boostMode)
        {
            case PickUp_Data.BoostMode.Uncontrolled:

                if (boostActive)
                {
                    UncontrolledBoost();
                }

                break;
            case PickUp_Data.BoostMode.Controlled:

                if (boostActive)
                {
                    ControlledBoost();
                }

                break;
            default:
                break;
        }

    }

    private void UncontrolledBoost()
    {
            boostTime -= Time.deltaTime;
            ApplyBoost();

        //ANIM: Play particle effect until completion.
        //animControl.TriggerItemLayer();
        //animControl.UpdateBoost(true);
    }

    private void ControlledBoost()
    {

        boostTime -= Time.deltaTime;
        ApplyBoost();

    }

    private void ApplyBoost()
    {

        Vector3 boostDirection = transform.forward;
        Vector3 boost = boostDirection * boostForce * 100f * Time.deltaTime;
        rb.AddForce(boost);
    }




}
