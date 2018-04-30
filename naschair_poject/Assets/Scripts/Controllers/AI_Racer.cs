using System.Collections;
using UnityEngine;
using Dreamteck.Splines;

public class AI_Racer : ChairMotor
{
	public float rotationDeadZone = .1f;
    [Tooltip("Max push delay")]
    public float pushTimer = 5; 
    public float resetCheckTime = 2;
    public float minDistPerResetCheck = 1;
    public float rotationSpeed = 0.5f;
    public Vector3 trackOffset;
    public SplineProjector pathProjector;

    SplineComputer[] possibleSplines;
	Vector3 targetPosition;
	Vector3 direction;
    Vector3 previouslyRecordedPoint;

    WaitForSeconds buttonHoldTime;
    WaitForSeconds resetDelay;

    RacerPickUp myPickup;

	float distFromTarget = 0;

    public override void Awake()
    {
        base.Awake();

        buttonHoldTime = new WaitForSeconds(0.1f);
        resetDelay = new WaitForSeconds(resetCheckTime);
        myPickup = GetComponent<RacerPickUp>();
    }

    public void Start()
    {
        useSpaceTransformer = false; //Set to false as no camera is present for AI

        //Set the timer for the push delay
        if (pushTimer < motorData.chargeTime)
            pushTimer = motorData.chargeTime;

        previouslyRecordedPoint = transform.position; //position it was at last frame
        StartCoroutine(AIPush());
        StartCoroutine(AIAutoReset());
    }

	public override void FixedUpdate()
	{
		base.FixedUpdate();

        targetPosition = (pathProjector.result.position + pathProjector.result.direction); //Get the target pos on the spline
		direction = targetPosition - transform.position; //Direction from the racer to the target pos
		distFromTarget = Vector3.Distance (targetPosition, transform.position); //Distance from the target pos

		AIBaseMovment();
		AIRotateTowardsTarget();
        AIAutoBrake();
        //PickUpBehaviour(); //Reacts to held pickups and uses them according to method behaviour.
	}

    void PickUpBehaviour ()
    {
        //Controls the behaviour of the AI in response to holding different kinds of pickup.
        switch (myPickup.currentPickupType)
        {
            case "Null":
                break;
            case "Boost": //If I have boost, use immediately.
                inputVaraibles.usePickupPressed = true;
                break;
            default:
                break;
        }
    }

	void AIRotateTowardsTarget()
	{
        Vector3 newDirection = transform.InverseTransformPoint(targetPosition); //Local space for the target pos

        //Rotate right
        if (newDirection.x > rotationDeadZone)
            inputVaraibles.controllerRotation.x = rotationSpeed;

        //Rotate left
        if (newDirection.x < -rotationDeadZone)
            inputVaraibles.controllerRotation.x = -rotationSpeed;

        //Center
        if (newDirection.x > -rotationDeadZone && newDirection.x < rotationDeadZone)
            inputVaraibles.controllerRotation.x = 0;
    }

	void AIBaseMovment()
	{
		direction = direction.normalized;
		inputVaraibles.controllerMovment.x = direction.x;	
		inputVaraibles.controllerMovment.z = direction.z;	
	}

    IEnumerator AIPush()
    {
        while (true)
        {
            inputVaraibles.pushPressed = true;
            yield return buttonHoldTime;
            inputVaraibles.pushPressed = false;
            yield return new WaitForSeconds(Random.Range(motorData.chargeTime, pushTimer) - 0.1f);
        }
    }

    IEnumerator AIAutoReset()
    {
        while (true)
        {
            previouslyRecordedPoint = transform.position;
            yield return resetDelay;
            float distFromPreviouslyRecordedPoint = Vector3.Distance(transform.position, previouslyRecordedPoint);
            if (distFromPreviouslyRecordedPoint < minDistPerResetCheck)
                StartCoroutine(ResetRotPos(0));
        }
    }
    
    void AIAutoBrake()
	{
		inputVaraibles.brakePressed = (distFromTarget > 4 && distFromTarget < 6);
    } 

    public void UpdateAiPaths(SplineComputer[] newSplines)
    {
        possibleSplines = newSplines;
        SetRandomAiPath();
    }

    public void SetRandomAiPath()
    {
        int rnd = Random.Range(0, possibleSplines.Length);
        pathProjector.computer = possibleSplines[rnd];
    }
}
