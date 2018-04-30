using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PickupChanceClass {

    //Stores pickup probability chance. Used by PickupThresholdChance_Data to keep data for each individual threshold.
    [Header("0 = low chance, 1 = high chance")]
    [Range(0f,1f)]
    public float boostChance;
    [Range(0f, 1f)]
    public float shieldChance;
    [Range(0f, 1f)]
    public float satchelChance;
    [Range(0f, 1f)]
    public float frisbeeChance;
	
}
