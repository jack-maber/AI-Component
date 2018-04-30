using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

    public enum PickUps { Boost, SatchelCharge, Frisbee, Shield}; //Count end is used to get the last enum number
    public float respawnTime = 20f;
    public bool dynamicType = true;

    public PickUps pickupType = PickUps.Boost;
    private RaceManager rm;
    private PickUpManager pm;

    public MeshRenderer pMesh;
    private Collider pCol;

    WaitForSeconds timer;

    private void Start()
    {
        rm = GameManager.GetRaceManager();
        timer = new WaitForSeconds(respawnTime);
        pm = GameManager.GetPickUpManager();
        
        if (pMesh == null)
            pMesh = GetComponent<MeshRenderer>();
        pCol = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            RacerPickUp rp = other.GetComponent<RacerPickUp>(); //Used to assign pickups.

            if (rp.currentBehaviourScript != null)
                return;

            if (dynamicType) //Set per pickup, if false, it uses the preset pickup value rather than dynamically assigning one by chance.
            {
                PickupChanceClass thresholdValues = GetThresholdPickupChance(other.gameObject); //Reference of the chance values set by scriptable object.
                PickUps selectedPickup = GetPickupType(thresholdValues);
                pickupType = selectedPickup;
            }
            string typeName;

            switch (pickupType)
            {
                case PickUps.Boost:
                    typeName = "Boost";
                    break;
                case PickUps.SatchelCharge:
                    typeName = "SatchelCharge";
                    break;
                case PickUps.Frisbee:
                    typeName = "Frisbee";
                    break;
                case PickUps.Shield:
                    typeName = "Shield";
                    break;
                default:
                    typeName = "Boost";
                    break;
            }

            rp.UpdatePickup(typeName);
            
            StartCoroutine("Respawn");
        }
    }

    IEnumerator Respawn()
    {
        pMesh.enabled = false;
        pCol.enabled = false;

        yield return timer;

        pMesh.enabled = true;
        pCol.enabled = true;
    }

    PickupChanceClass GetThresholdPickupChance(GameObject racerObject)
    {

        Racers myRacer = rm.GetRacerWithPrefab(racerObject);
        float myPos = myRacer.placment; //Get my current position out of all racers.
        
        PickupChanceClass spawnChances;

        float finishedCount = rm.GetFinishedRacerCount(); //Gets the amount of players who have finished the race.
        myPos -= finishedCount; //Offsets my position by those who have finished to get my position relative to the active racers.

        if (myPos != 1) 
        {
            float racerCount = rm.GetActiveRacerCount(); //Get the number of racers so a percenatage can be calculated.
            
            racerCount = rm.allRacers.racerCollection.Count - finishedCount; //Gets the number of active racers by offsetting the total by number of finished racers.       

            float positionPercentage = myPos / racerCount; //Convert position into a percentage.

            if (positionPercentage < 0.68f) //Cuts up middle and rear racers eitherside of 68%.
            {
                spawnChances = pm.thresholdData.middlePositionChance;
            }
            else
            {
                spawnChances = pm.thresholdData.rearPositionChance;
            }
        } else
        {
            spawnChances = pm.thresholdData.firstPlaceChance;
        }

        return spawnChances;

    }

    PickUps GetPickupType (PickupChanceClass spawnChances)
    {

        //All chances added together to create a range.
        float chanceRange = spawnChances.boostChance + spawnChances.shieldChance + spawnChances.satchelChance + spawnChances.frisbeeChance;

        float chance = Random.Range(0f, chanceRange);

        //Stores the chances as seperate elements of a list so they can be checked against the chance.
        List<float> chanceValues = new List<float>();
        chanceValues.Add(spawnChances.boostChance);
        chanceValues.Add(spawnChances.shieldChance);
        chanceValues.Add(spawnChances.satchelChance);
        chanceValues.Add(spawnChances.frisbeeChance);

        int selectedIndex = 0;
        float chanceThreshold = 0f;

        //Checks against the range to find which chance is hit.
        for (int i = 0; i < chanceValues.Count; i++)
        {

            chanceThreshold += chanceValues[i];

            if (chance <= chanceThreshold)
            {
                selectedIndex = i;
                break;
            }

        }

        PickUps selectedPickup = PickUps.Boost;

        switch (selectedIndex)
        {
            case 0:
                selectedPickup = PickUps.Boost;
                break;
            case 1:
                selectedPickup = PickUps.Shield;
                break;
            case 2:
                selectedPickup = PickUps.SatchelCharge;
                break;
            case 3:
                selectedPickup = PickUps.Frisbee;
                break;
        }

        return selectedPickup;
    }

}