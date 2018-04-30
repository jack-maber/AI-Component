using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickupThresholdChanceData", menuName = "RockPool/Data Instace/PickupThresholdChance Data", order = 1)]
public class PickupThresholdChance_Data : ScriptableObject
{

    public PickupChanceClass firstPlaceChance = new PickupChanceClass();
    public PickupChanceClass middlePositionChance = new PickupChanceClass();
    public PickupChanceClass rearPositionChance = new PickupChanceClass();

}
