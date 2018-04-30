using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;

public class ChairMotor : MonoBehaviour
{
    public ChairMotorData motorData;
    public Transform spaceTransform;
	public ParticleSystem [] ps;
    public PlayerAnimationController animController;

    float newRotation = 0;

    protected bool isCharging = false;
    protected bool canMove = false;
    protected bool useSpaceTransformer = true;

    public bool pushDisabled = false; //Used by certain pickup abilities, e.g. boost, to disable the use of pushing while they are in use.

    [HideInInspector]
    public SplineProjector splineProjector;
	[HideInInspector]
	public ChairControllerInputVaraibles inputVaraibles;
	[HideInInspector]
	public Rigidbody rBody;
    [HideInInspector]
    public float distFromGround = 0;
    [HideInInspector]
    public bool showPushIcon = true;
    [HideInInspector]
    public float currentSpeed = 0;
    [HideInInspector]
    public bool disableMovement = false;
    [HideInInspector]
    public float chargeRange = 0;

    float pushCoolDownTimer = 0;

    public bool stunned = false;

    Vector3 checkGroundOrigin, checkGroundDirection, breakVelocity;
    Ray ray;
    public Transform spaceTracker;
    GraphConverter graphConverter;
	List<ParticleSystem.EmissionModule> em = new List<ParticleSystem.EmissionModule> ();

    private CharacterSwitcher switcher;

    public virtual void Awake()
    {
        SetUpRigidBody(); //Apply values from character motor settings to the rigidbody
        switcher = GetComponent<CharacterSwitcher>();

        graphConverter = gameObject.AddComponent<GraphConverter>(); //Add and store the graph calculator.
        //Debug.Assert(graphConverter, "Somthing went wrong and a graph calcualtor was not added to " + this.name); 

        //Set up the graph calculator
        graphConverter.curve = motorData.speedCurve;
        graphConverter.sampleAmount = motorData.curveSamples;
        graphConverter.StartCalculation();

        if (spaceTransform == null)
            spaceTransform = transform; //If no space transform has been set use this transform

        spaceTracker = new GameObject("[Tracker] Space").GetComponent<Transform>(); //The space tracker is used for getting the stick direction from local space ignoring rotation
        splineProjector = gameObject.GetComponent<SplineProjector>();

        foreach (ParticleSystem p in ps)
		{
			em.Add (p.emission);
		}
    }

    void SetUpRigidBody()
    {
        rBody = gameObject.AddComponent<Rigidbody>(); //Add and store the rigidbody.
        //Debug.Assert(rBody, "Somthing went wrong and a rigidbody was not added to " + this.name);

        rBody.mass = motorData.mass;
        rBody.drag = motorData.drag;
        rBody.angularDrag = motorData.angularDrag;
        rBody.useGravity = motorData.useGravity;
        rBody.isKinematic = motorData.isKinimatic;
        rBody.interpolation = motorData.interpolation;
        rBody.collisionDetectionMode = motorData.collisionDetection;
        rBody.constraints = motorData.constraints;
        rBody.centerOfMass = motorData.centerOfMass;

        Collider col = GetComponent<Collider>();

        if (col != null)
            col.material = (PhysicMaterial)motorData.physicsMaterial;
    }

    public virtual void Update()
    {
        if (disableMovement)
            return;

        PushTimerCheck();

        if (inputVaraibles.resetTimer >= motorData.restHoldTime)
            StartCoroutine(ResetRotPos(0f));

        checkGroundOrigin = transform.position + transform.TransformDirection(motorData.originOffset);
        checkGroundDirection = motorData.direction;

        canMove = (motorData.onlyMoveWhenGrounded) ? CheckGrounded() : true;

        if (stunned)
        {
            canMove = false;
        }

        if(animController != null)
            animController.DistFromGroundUpdate(distFromGround);

        currentSpeed = rBody.velocity.magnitude;
    }

    public virtual void FixedUpdate()
    {
        if (disableMovement)
            return;

        UpdateSpaceTracker();

        Rotate();
        if (CheckGrounded()) //To stop weird behaviour when airborne.
        {
            SelfRight();
            if (switcher.isAI) //Because AI require clamping to function correctly, players do not.
            {
                ClampRotation();
            }
        }

        if (inputVaraibles.pushPressed && canMove && !pushDisabled)
            StartPush();

        if (inputVaraibles.brakePressed && canMove)
            Brake();

        if ((inputVaraibles.controllerMovment.x != 0 || inputVaraibles.controllerMovment.z != 0) && canMove)
            BaseMovment();

        if (!canMove)
            IncreaseGravity();

        if (inputVaraibles.jumpPressed)
            Jump();

        if (animController != null)
        {
            animController.UpdateBraking(inputVaraibles.brakePressed && canMove);
            animController.UpdateFalling(!CheckGrounded());
        }
    }

    void BaseMovment()
    {
        Vector3 pushDirection = GetStickDirection(inputVaraibles.controllerMovment.x, inputVaraibles.controllerMovment.z) * motorData.baseSpeed;
        Vector3 newPushDirection = (rBody.velocity.magnitude > pushDirection.magnitude) ? Vector3.zero : pushDirection - rBody.velocity;
        rBody.AddForce(newPushDirection);

        ChangeForceDirection(pushDirection, 1);
    }

    void StartPush()
    {
        if (pushCoolDownTimer > motorData.pushSweetSpotTimer)
        {
            FailPush();
            //Debug.Log("FAIL");
        }
        else if (pushCoolDownTimer <= 0)
        {
            StartCoroutine(Push(motorData.pushSpeed));
            StartPushCharge();
            //Debug.Log("NORMAL");
        }
        else if (pushCoolDownTimer > 0 && pushCoolDownTimer <= motorData.pushSweetSpotTimer)
        {
            StartCoroutine(Push(motorData.fastPushSpeed));
            StartPushCharge();
            animController.TriggerPerfPush();
            //Debug.Log("GOTA GO FAST WURDERERERERERERERERERE");
        }
    }

    public void StunRacer(float stunTime)
    {
        if (!stunned)
        {
            StartCoroutine(StunnedDelay(stunTime));
        }
    }

    IEnumerator StunnedDelay(float delay)
    {
        //Disables input for a set amount of time.
        stunned = true;
        yield return new WaitForSeconds(delay);
        stunned = false;
    }

    void StartPushCharge()
    {
        if (animController != null)
            animController.UpdatePushing(true);

        isCharging = true;
        pushCoolDownTimer = motorData.chargeTime + motorData.pushSweetSpotTimer;
    }

    void PushTimerCheck()
    {
        showPushIcon = (!isCharging || pushCoolDownTimer > 0 && pushCoolDownTimer < motorData.pushSweetSpotTimer);

        if (!isCharging)
            return;

        pushCoolDownTimer -= Time.deltaTime;
        chargeRange = (motorData.chargeTime - (pushCoolDownTimer - motorData.pushSweetSpotTimer)) / motorData.chargeTime;

        if(pushCoolDownTimer <= 0)
        {
            if (animController != null)
                animController.UpdatePushing(false);
            isCharging = false;
            pushCoolDownTimer = 0;
        }
    }

    public virtual IEnumerator Push(float speed)
    {
        float[] samples = graphConverter.ExtractCurveSamples();
        float waitTime = graphConverter.ExtractIntervalTime();
        for (int i = 0; i < samples.Length; i++)
        {
            Vector3 planeDirection = Vector3.ProjectOnPlane(transform.forward, GetPlaneNormal());
            rBody.AddForce(planeDirection * (samples[i]) * speed);
            yield return new WaitForSeconds(waitTime);
        }
    }

    void Brake()
    {
        Vector3 gravity = Vector3.zero;
        gravity.y = rBody.velocity.y;
        rBody.velocity = Vector3.SmoothDamp(rBody.velocity, gravity, ref breakVelocity, motorData.brakingSpeed);
    }

    void ChangeForceDirection(Vector3 newDirection, float speed)
    {
        Vector3 d = Vector3.RotateTowards(rBody.velocity, newDirection, speed * Time.deltaTime, 0);
        d.y = rBody.velocity.y;

        rBody.velocity = d;
    }

    public IEnumerator ResetRotPos(float penaltyTime)
    {
        if (!disableMovement)
        {
            yield return new WaitForSeconds(penaltyTime);
            transform.position = splineProjector.result.position + new Vector3(0, 1, 0);
            transform.LookAt(transform.position + (splineProjector.result.direction.normalized * 2)); //Make the racer face the direction the race is going in 
            rBody.velocity = Vector3.zero;
            rBody.angularVelocity = Vector3.zero;
            inputVaraibles.resetTimer = 0;
        }
    }

    void ClampRotation()
    {
        Vector3 tempRot = transform.eulerAngles;
        tempRot.x = ClampRotationValue(tempRot.x, motorData.minRotationClamp.x, motorData.maxRotationClamp.x);
        tempRot.z = ClampRotationValue(tempRot.z, motorData.minRotationClamp.z, motorData.maxRotationClamp.z);
        transform.eulerAngles = tempRot;
    }

    void Rotate()
    {
        float x = inputVaraibles.controllerRotation.x;
        float z = inputVaraibles.controllerRotation.z;

        /*
        Vector3 direction = GetStickDirection(x,z);

        Quaternion newRotation = (x != 0 || z != 0) ? Quaternion.LookRotation(direction) : transform.rotation;
        newRotation.x = 0;
        newRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * motorData.rotationSpeed);
        */

        transform.Rotate(new Vector3(0, x, 0) * motorData.rotationSpeed);
    }

    float ClampRotationValue(float value, float min, float max) //https://answers.unity.com/questions/141775/limit-local-rotation.html
    {
        if (value < 90 || value > 270)
        {
            value -= (value > 180) ? 360 : 0;
            max -= (max > 180) ? 360 : 0;
            min -= (min > 180) ? 360 : 0;
        }

        value = Mathf.Clamp(value, min, max);

        if (value < 0) // If the value is in the minuses then add 360 to get the value it would be if it went back to 360
            value += 360;

        return value;
    }

    void SelfRight()
    { 
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        newRotation.y = 0;
        rBody.AddTorque(new Vector3(newRotation.x, newRotation.y, newRotation.z) * motorData.upRightForce);
    }

    void IncreaseGravity()
    {
        rBody.AddForce(new Vector3(0, motorData.customGravity, 0));
    }

    Vector3 GetStickDirection(float x, float z)
    {
        Vector3 direction = Vector3.zero;

        direction = new Vector3(x, 0, z);

        if (useSpaceTransformer)
            direction = spaceTracker.TransformDirection(direction);

        //direction.y = 0;

        return direction;
    }

    public bool CheckGrounded()
    {
        ray = new Ray(checkGroundOrigin, checkGroundDirection);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            distFromGround = hit.distance;

            if (distFromGround <= motorData.checkLength)
                return true;
        }

        return false;
    }

    public Vector3 GetPlaneNormal()
    {
        ray = new Ray(checkGroundOrigin, checkGroundDirection);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, motorData.checkLength))
        {
            return hit.normal;
        }

        return Vector3.zero;
    }

    void UpdateSpaceTracker()
    {
        spaceTracker.position = spaceTransform.position;
        Vector3 tempRot = spaceTransform.eulerAngles;
        tempRot.x = 0;
        spaceTracker.eulerAngles = tempRot;
    }

    public void FailPush()
    {
        rBody.AddForce(-transform.forward * 200);
        rBody.AddTorque(new Vector3(100, 0, 0));
        animController.TriggerPoorPush();
    }

    public void Jump()
    {
        if (CheckGrounded())
            rBody.AddForce(Vector3.up * motorData.jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        if (motorData.showGizmos == false)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.TransformDirection(motorData.centerOfMass), 0.1f);

        if (!Application.isPlaying)
        {
            checkGroundOrigin = transform.position + transform.TransformDirection(motorData.originOffset);
            checkGroundDirection = motorData.direction;
            ray = new Ray(checkGroundOrigin, checkGroundDirection);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(ray.origin, ray.GetPoint(motorData.checkLength));
        Gizmos.DrawSphere(ray.origin, 0.2f);
    }
}
