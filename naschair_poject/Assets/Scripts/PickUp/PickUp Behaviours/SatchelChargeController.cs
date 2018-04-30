using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatchelChargeController : PickUpBehaviour
{

    private GameObject satchelObject;
    private GameObject spawnedSatchel;
    private bool satchelThrown = false;
    private Rigidbody racerRb;

    private SatchelCharge satchelClass;

    private void Start()
    {
        satchelObject = pickUpData.satchelObject;
        racerRb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //If the usepickup button is pressed and no satchel has been spawned, throw a satchel.
        if (motor.inputVaraibles.usePickupPressed & satchelThrown == false)
        {
            Debug.Log("Throwing satchel!");
            ThrowSatchel();
            satchelThrown = true;
        }
        else if (motor.inputVaraibles.usePickupPressed)
        {
            if (spawnedSatchel != null) //So that it doesnt try to detonate a charge that has already expired.
            {
                satchelClass.DetonateCharge();
            }

            RemovePickup();

        }
    }

    void ThrowSatchel()
    {
        //Play throw animation.
        animControl.TriggerItemLayer();
        animControl.TriggerThrowForward();

        //Spawn the satchel in front of the player.
        Vector3 satchelSpawnPosition = GetObjectSpawnPoint(1f);
        spawnedSatchel = Instantiate(satchelObject, satchelSpawnPosition, Quaternion.identity);
        Rigidbody satchelRb = spawnedSatchel.GetComponent<Rigidbody>();
        satchelRb.velocity = racerRb.velocity;

        //Sets the arc angle for the satchel.
        Vector3 startRotation = transform.rotation.eulerAngles;
        Debug.Log("Start rotation = " + startRotation);
        startRotation.x -= pickUpData.satchelThrowAngle;
        Debug.Log("Pickup Throw Angle: " + pickUpData.satchelThrowAngle);
        Debug.Log("Start rotation after angle = " + startRotation);
        spawnedSatchel.transform.rotation = Quaternion.Euler(startRotation);

        //Apply the throw force along the charges z-axis.
        Vector3 throwForce = spawnedSatchel.transform.forward;
        //Vector3 throwForce = Vector3.forward;
        Debug.Log("Forward: " + throwForce);
        throwForce *= pickUpData.satchelForce;
        Debug.Log("Force: " + throwForce);
        satchelRb.AddForce(throwForce);

        satchelClass = spawnedSatchel.GetComponent<SatchelCharge>();
        satchelClass.pickUpData = pickUpData;
        satchelClass.racer = racer;
    }


}
