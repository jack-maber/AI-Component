using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickUpData", menuName = "RockPool/Data Instace/PickUp Data", order = 1)]
public class PickUp_Data : ScriptableObject {

    public enum BoostMode { Uncontrolled, Controlled };

    [Header("Boost")]
    public BoostMode boostMode;
    public float boostForce;
    public float boostDuration;
    public ParticleSystem boostParticle;
    [Space(10)]
    [Header("Satchel Charge")]
    public GameObject satchelObject;
    public float satchelThrowForce;
    [Range(0f, 90f)]
    public float satchelThrowAngle;
    public float satchelRadius;
    public float satchelForce;
    public float satchelSelfDestructTimer;
    public float upwardsModifier;
    [Space(10)]
    [Header("Frisbee")]
    public GameObject frisbeeObject; //The prefab.
    public float maxFrisbeeSpeed; //Speed that the frisbee will move
    public float frisbeeRange; //How close a player needs to be to become a target.
    public float frisbeeLifetime; //How long until the frisbee destroys itself.
    public float frisbeeThrowForce; //Force of initial throw.
    public float dropOffVelocity; //The velocity at which the frisbee begins to fall to the ground.
    public float stunTime;
    [Space(10)]
    [Header("Shield")]
    public float shieldDuration;
    public GameObject shieldObject;

}
