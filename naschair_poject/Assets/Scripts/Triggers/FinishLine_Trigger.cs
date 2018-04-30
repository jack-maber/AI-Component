using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine_Trigger : TriggerBase
{
    RaceManager rm;
    LapManager lm;
    public ParticleSystem Confetti;
    public bool isOneWay = true;

    GameObject oneWayObject;

    private void Start()
    {
        rm = GameManager.GetRaceManager();
        lm = GameManager.GetLapManager();

        if (isOneWay)
            SetUpOneWay();
    }

    void SetUpOneWay()
    {
        oneWayObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
        oneWayObject.transform.position = transform.position + (transform.forward * (scale.z/2));
        oneWayObject.transform.parent = transform;
        oneWayObject.transform.localEulerAngles = new Vector3(270, 0, 180);

        Destroy(oneWayObject.GetComponent<MeshFilter>());
        Destroy(oneWayObject.GetComponent<MeshRenderer>());

        oneWayObject.transform.localScale = new Vector3(scale.x, 1, scale.y) / 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        ParticleSystem.EmissionModule em;
        if (Confetti != null) //Null check to stop any errors from breaking the lap system
        {
            em = Confetti.emission;
            em.enabled = false;
        }

        if (other.tag == "Player")
        {
            Racers racer = rm.GetRacerWithPrefab(other.gameObject); //Grab the racer profile for this object

            if (racer == null) //If no racer was returned then don't run any of this 
                return;

            if (racer.checkPoint >= 1- lm.checkPointPercentage)//If the racer has reached all the checkPoints
            {
                racer.checkPoint = 0;
                lm.IncreaseLapForRacer(racer);

                if(racer.lap >= rm.raceData.lapCount && Confetti != null) //Null check here again
                {
                    em.enabled = true;
                }
            } 
        }
    }
}
