using UnityEngine;
using Dreamteck.Splines;

[System.Serializable]
public class Racers
{
    public string displayName;
    public GameObject racerPrefab;
    public float trackDistance;
    public int placment;
    public int lap;
    public PlayerClass playerClass;
    public SplineProjector splineProjector;
    public float checkPoint;
    public bool isFinished;
    public ChairMotor motor;
    public CharacterProfile character;
    public int score;
}
