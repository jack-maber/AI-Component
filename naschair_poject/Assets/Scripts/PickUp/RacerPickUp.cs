using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class RacerPickUp : MonoBehaviour {


    public string currentPickupType = "Null";
    public Object currentBehaviourScript = null;
    private PlayerClass pClass;
    private PickUp_Data pickUpData;
    private ChairMotor motor;

    private void Start()
    {
        PlayerManager playerManager = GameManager.GetPlayerManager();
        pClass = playerManager.GetPlayerClassByPrefab(gameObject);

        PickUpManager pickUpManager = GameManager.GetPickUpManager();
        pickUpData = pickUpManager.pickUpData;

        motor = GetComponent<ChairMotor>();

    }

    public void UpdatePickup (string newPickup)
    {
        Debug.Log("Updating Pickup " + currentPickupType + " to " + newPickup);
        motor.pushDisabled = false;

        if (currentPickupType == "Shield")
        {
            Shield myShield = currentBehaviourScript as Shield;
            myShield.RemoveShield();
        }

        currentPickupType = newPickup;
        Destroy(currentBehaviourScript);
        currentBehaviourScript = null;
    }


    //REFACTOR ME AND REMOVE REPRODUCED CODE - from Tom to Tom.
    private void Update()
    {    
        if (currentPickupType == "Boost" & currentBehaviourScript == null)
        {
            Debug.Log("Updating currentBehaviourScript!");
            currentBehaviourScript = gameObject.AddComponent<Boost>();
            Boost newBoost = currentBehaviourScript as Boost;
            SetupPickup(newBoost);
        }

        if (currentPickupType == "SatchelCharge" & currentBehaviourScript == null)
        {
            Debug.Log("Updating currentBehaviourScript!");
            currentBehaviourScript = gameObject.AddComponent<SatchelChargeController>();
            SatchelChargeController newSatchel = currentBehaviourScript as SatchelChargeController;

            SetupPickup(newSatchel);
        }

        if (currentPickupType == "Frisbee" & currentBehaviourScript == null)
        {
            Debug.Log("Updating currentBehaviourScript!");
            currentBehaviourScript = gameObject.AddComponent<FrisbeeController>();
            FrisbeeController newFrisbee = currentBehaviourScript as FrisbeeController;

            SetupPickup(newFrisbee);
        }

        if (currentPickupType == "Shield" & currentBehaviourScript == null)
        {
            Debug.Log("Updating currentBehaviourScript!");
            currentBehaviourScript = gameObject.AddComponent<Shield>();
            Shield newShield = currentBehaviourScript as Shield;

            SetupPickup(newShield);
        }
    }

    private void SetupPickup(PickUpBehaviour newBehaviour)
    {
        newBehaviour.playerClass = pClass;
        newBehaviour.pickUpData = pickUpData;
        newBehaviour.racer = this;
        newBehaviour.motor = motor;
        newBehaviour.animControl = motor.animController;
    }

    public bool IsShielded(GameObject target)
    {
        bool shielded = false;
        if (target.GetComponent<Shield>() != null)
        {
            //If they do, check if its active.
            Shield playerShield = target.GetComponent<Shield>();
            shielded = playerShield.shielded;
        }

        return shielded;
    }

}
