using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoostData", menuName = "RockPool/Data Instace/Boost Data", order = 1)]
public class BoostData : ScriptableObject
{
    public List<BoostSegment> boostSegment = new List<BoostSegment>();
}
