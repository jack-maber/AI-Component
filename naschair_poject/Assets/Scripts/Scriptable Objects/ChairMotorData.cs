using UnityEngine;

[CreateAssetMenu(fileName = "ChairMotorData", menuName = "RockPool/Data Instace/Chair Motor", order = 1)]
public class ChairMotorData : ScriptableObject
{
    [Header("Speed Settings")]
    [Tooltip("The speed at which the chair moves when not using the kick feature")]
    public float baseSpeed = 1f;
    [Tooltip("The amount of force applied to the chair at each normal push")]
    public float pushSpeed = 6;
    [Tooltip("The amount of force applied to the chair at each 'good' push")]
    public float fastPushSpeed = 10;
    [Tooltip("How much of the force is applied over the defined amount of time")]
    public AnimationCurve speedCurve;
    [Range(5, 200), Tooltip("The quality of the above animation curve")]
    public int curveSamples = 10;
    [Range(0, 10), Tooltip("How much force is removed per second")]
    public float brakingSpeed = 0.1f;

    [Header("Push Settings")]
    [Tooltip("The time it takes for the push timer to fully recharge")]
    public float chargeTime = 1;
    [Tooltip("The time after the push timer fully charges in which the player can get a 'good' push")]
    public float pushSweetSpotTimer = 0.5f;
    [Tooltip("Can the push charge whilst airborne")]
    public bool airborneCharging = false; //Toggles charging of push meter while airborne.
    [Tooltip("The amount of force applied to the chair upwards when the jump button is pressed")]
    public float jumpForce = 10;

    [Header("Physics Settings")]
    [Tooltip("The rigidbody mass")]
    public float mass = 1;
    [Tooltip("The rigidbody drag (Air resistance)")]
    public float drag = 0;
    [Tooltip("Same as drag but for rotation force")]
    public float angularDrag = 0.05f;
    [Tooltip("The extra force applied downwards")]
    public float customGravity = -9f;
    [Tooltip("Does the rigidbody use gravity (This does not diable the custom gravity)")]
    public bool useGravity = true;
    [Tooltip("The rigidbody isKinimatic value")]
    public bool isKinimatic = false;
    public RigidbodyInterpolation interpolation;
    public CollisionDetectionMode collisionDetection;
    public RigidbodyConstraints constraints;
    [Tooltip("The physics material that is applied to the collider")]
    public PhysicMaterial physicsMaterial;
    [Tooltip("Center of mass for the rigidbody")]
    public Vector3 centerOfMass = Vector3.zero;
    [Tooltip("Roation clamp for the chair x and z axis")]
    public Vector3 maxRotationClamp = Vector3.zero;
    [Tooltip("Roation clamp for the chair x and z axis")]
    public Vector3 minRotationClamp = Vector3.zero;
    [Tooltip("The force applied to keep the chair upright")]
    public float upRightForce = 100;

    [Header("Input Settings")]
    [Tooltip("Time the reset button has to be held down to force reset")]
    public float restHoldTime = 1f;
    [Tooltip("Speed which the chair and the camera rotate")]
    public float rotationSpeed = 10;

    [Header("Is Grounded Settings")]
    [Tooltip("Lenght of the raycast that checks the if the chair is grounded")]
    public float checkLength = 1f;
    [Tooltip("The offset from the origin in which the raycast is casted")]
    public Vector3 originOffset;
    [Tooltip("Direction the check grounded raycast goes")]
    public Vector3 direction = -Vector3.up;

    [Header("Debug")]
    public bool showGizmos = true;
    public bool onlyMoveWhenGrounded = true;
}
