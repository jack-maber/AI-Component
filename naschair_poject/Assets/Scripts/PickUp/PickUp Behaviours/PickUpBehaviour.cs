using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PickUpBehaviour : MonoBehaviour {

    //To be parsed.
    public PlayerClass playerClass;
    public PickUp_Data pickUpData;
    public RacerPickUp racer;
    public ChairMotor motor;
    public PlayerAnimationController animControl;

    public void RemovePickup()
    {
        racer.UpdatePickup("Null");
    }
    
    //Returns an offset position from the player relative to forward axis for the spawning of objects e.g. satchel charges. 
    public Vector3 GetObjectSpawnPoint(float modifier) //Mod of 1 = 1 unit in front. Mod of -1 = 1 unit behind.
    {
        Vector3 spawnPoint;
        spawnPoint = transform.forward * modifier;
        spawnPoint = transform.position + spawnPoint;
        return spawnPoint;
    }

}
