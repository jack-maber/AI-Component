using UnityEngine;

[System.Serializable]
public struct BoostSegment
{
    public float percentage;
    public BoostTypes boostType;
    public Color colour;
    [HideInInspector]
    public float fill;
    [HideInInspector]
    public float lowerLimit;
    [HideInInspector]
    public float upperLimit;
}
