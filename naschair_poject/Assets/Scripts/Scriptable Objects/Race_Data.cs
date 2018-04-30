using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Race Data", menuName = "RockPool/Data Instace/Race Data", order = 1)]
public class Race_Data : ScriptableObject
{
    public int lapCount = 3;
    public int maxRacers = 8;
    public float preRaceSegmentLength = 3;
    public int raceCountDown = 3;
    public float raceTime = 300f;
}
