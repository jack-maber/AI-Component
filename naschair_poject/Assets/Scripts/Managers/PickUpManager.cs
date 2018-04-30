using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : BaseManager {

    public PickUp_Data pickUpData;
    public PickupThresholdChance_Data thresholdData;

    public override void Awake()
    {
        base.Awake();
    }

}
