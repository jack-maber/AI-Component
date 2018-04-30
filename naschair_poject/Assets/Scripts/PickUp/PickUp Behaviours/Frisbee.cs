using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frisbee : TargetableBehaviour
{

    public PickUp_Data pickupData;
    public Vector3 playerVelocity;

    public Transform myTarget; //Change to private when done debugging!
    private Rigidbody myRb;
    private bool targetFound = false;
    private float frisbeeForce;
    private Vector3 originDirection;
    private bool hitSomething = false;
    //private bool active = false;

    // Use this for initialization
    void Start()
    {
        //Grab data from pickup data scriptable object.
        range = pickupData.frisbeeRange;
        frisbeeForce = pickupData.frisbeeThrowForce;

        //Start the self destruct timer.
        StartCoroutine(DestroyDelay());

        myRb = GetComponent<Rigidbody>();

        //Record the direction the frisbee came from.
        //So that we can compare to the target direction, allows the disabling of firing past our FOV.
        //originDirection = -transform.forward;

        //Tries to find a target within the range.
        //targetFound = GetTarget(out myTarget); //TODO: Fix issue with fail causing null ref.

        //Sets the initial frisbee velocity to match the player who's throwing it.
        myRb.velocity = playerVelocity;

        //Applies the initial throwforce.
        ThrowForce();

        //StartCoroutine(SetupDelay());

    }

    // Update is called once per frame
    void FixedUpdate()
    {



        ////If the frisbee hits something it stops tracking the target.
        //if (!hitSomething)
        //{
        //    //If I have a target assigned, update my direction to face them.
        //    if (targetFound)
        //    {
        //        FaceTarget();
        //    }

        //    //Apply forward thrust to move the frisbee along towards the target if any enabling tracking.
        //    UpdateForwardVelocity();
        //}
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") & !hitSomething)
        {
            hitSomething = true;
            ChairMotor racerMotor = other.GetComponent<ChairMotor>();
            //if (!other.GetComponent<RacerPickUp>().IsShielded(other.gameObject))
            //{
                racerMotor.StunRacer(pickupData.stunTime);
            //}
        }
    }

    //Detects a collision between the frisbee and any other collider.
    //void OnCollisionEnter(Collision col)
    //{
    //    //So we can disable target tracking.
    //    hitSomething = true;
    //}

    //So we can update frisbee rotation to face the target.
    Vector3 GetTargetDirection(Vector3 target, Vector3 origin)
    {
        Vector3 direction = target - origin;
        return direction;
    }

    void ThrowForce()
    {      
        //Applies the initial throw force along the frisbee's forward axis.
        Vector3 newForce = transform.forward * frisbeeForce;
        myRb.AddForce(newForce, ForceMode.Impulse);
    }

    void FaceTarget()
    {
        //Updates the rotation of the frisbee to exactly match the direction to the target.
        Vector3 faceDirection = myTarget.position - transform.position;
        transform.rotation = Quaternion.LookRotation(faceDirection);
    }

    void UpdateForwardVelocity()
    {
        //Applies a constant force along the z-axis while the frisbee is still flying to create a gliding effect.
        Vector3 newVelocity = transform.forward * myRb.velocity.magnitude;

        //Disables the downfard force of gravity for gliding until the overall velocity of the frisbee dies off past the drop off value.
        if (newVelocity.magnitude > pickupData.dropOffVelocity)
        {
            newVelocity.y = 0f;
        }
        else
        {
            newVelocity.y = myRb.velocity.y;
        }

        myRb.velocity = newVelocity;
    }

    IEnumerator DestroyDelay()
    {
        //Automatically detonates the charge if its not detonated manually within its lifetime.
        float lifetime = pickupData.frisbeeLifetime;
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }

    //IEnumerator SetupDelay()
    //{
    //    //Delays before allowing the stun effect from frisbees to avoid it triggering on the thrower.
    //    yield return new WaitForSeconds(0.1f);
    //    Debug.Log("Active!");
    //    active = true;
    //}

}
