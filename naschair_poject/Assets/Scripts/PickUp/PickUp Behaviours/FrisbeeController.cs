using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrisbeeController : PickUpBehaviour
{
    Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //If the usepickup button is pressed and no satchel has been spawned, throw a satchel.
        if (motor.inputVaraibles.usePickupPressed)
        {
            Debug.Log("Throwing frisbee!");
            ThrowFrisbee();
            RemovePickup();
        }
    }

    void ThrowFrisbee()
    {
        Vector3 frisbeeSpawnPoint = GetObjectSpawnPoint(2.5f);
        GameObject spawnedFrisbee = Instantiate(pickUpData.frisbeeObject, frisbeeSpawnPoint, transform.rotation);
        Frisbee newFrisbeeClass = spawnedFrisbee.GetComponent<Frisbee>();
        newFrisbeeClass.pickupData = pickUpData;
        //newFrisbeeClass.userPrefab = gameObject;
        newFrisbeeClass.playerVelocity = rb.velocity;


    }

}
