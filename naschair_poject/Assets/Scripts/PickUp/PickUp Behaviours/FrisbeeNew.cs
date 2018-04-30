using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

[RequireComponent(typeof(SplineProjector))]
public class FrisbeeNew : MonoBehaviour
{
    public PickUp_Data pickupData;
    public Transform target;
    public ChairMotor targetMotor;
    public SplineProjector splineProjector;
    public GameObject userPrefab;
    public PlayerAnimationController animControl;

    Rigidbody rigidBody;
    Vector3 targetDirection;

    LapManager lm;
    RaceManager rm;

    private void Start()
    {
        lm = GameManager.GetLapManager();
        rm = GameManager.GetRaceManager();

        rigidBody = GetComponent<Rigidbody>();

        splineProjector = GetComponent<SplineProjector>();
        splineProjector.computer = lm.spline;

        Racers racer = rm.GetRacerWithPrefab(userPrefab);
        int targetPlace = (racer.placment > 1) ? racer.placment - 1 : 0;
        Debug.Log(racer.placment);
        target = rm.GetRacerByPlace(targetPlace).transform;
        targetMotor = target.GetComponent<ChairMotor>();
    }

    private void FixedUpdate()
    {
        if (target == null)
            return;

        targetDirection = target.position - transform.position;
        targetDirection = targetDirection.normalized;

        if (TargetInSight())
        {
            FollowTarget();
        }
        else
        {
            FollowSpline();
        }

        HitTarget();
    }

    void FollowSpline()
    {
        Vector3 newDirection = (splineProjector.result.position + splineProjector.result.direction) - transform.position;
        Vector3 newForce = (newDirection.normalized * pickupData.maxFrisbeeSpeed) - rigidBody.velocity;
        rigidBody.AddForce(newForce, ForceMode.Acceleration);
    }

    void FollowTarget()
    {
        Vector3 newForce = (targetDirection * pickupData.maxFrisbeeSpeed) - rigidBody.velocity;
        rigidBody.AddForce(newForce, ForceMode.Acceleration);
    }

    public bool TargetInSight()
    {
        Ray ray = new Ray(transform.position, targetDirection);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, pickupData.frisbeeRange))
        {
            if (hit.transform == target)
                return true;
        }

        return false;
    }

    void HitTarget()
    {
        float dist = Vector3.Distance(transform.position, target.position);



        if (dist <= 2)
        {
            if (!target.GetComponent<RacerPickUp>().IsShielded(target.gameObject))
            {
                targetMotor.FailPush();
                animControl.TriggerDamage();
            }
            Destroy(gameObject);
        }
    }
}
