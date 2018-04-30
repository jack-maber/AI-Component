using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableBehaviour : MonoBehaviour
{

    public float range;
    public GameObject firingPlayer;

    //Finds players within a set range, finds the closest and returns it as out 'target'. If no target is found the bool returns false.
    public bool GetTarget(out Transform target)
    {
        //Nearby check.
        Collider[] hits = Physics.OverlapSphere(transform.position, range);

        bool targetFound = false;
        float nearestDistance = 0f;
        target = null;


        foreach (Collider col in hits)
        {
            GameObject subjectObject = col.gameObject;
            if (subjectObject.CompareTag("Player") & subjectObject.name != firingPlayer.name)
            {

                float angleToTarget = Vector3.Angle(transform.position, subjectObject.transform.position);
                Debug.Log("Angle to " + subjectObject.name + " = " + angleToTarget);

                if (!targetFound) //If the first player object found, set nearestDistance to match this.
                {
                    target = col.transform;
                    nearestDistance = Vector3.Distance(transform.position, col.transform.position);
                    targetFound = true;
                }
                else if (nearestDistance > Vector3.Distance(transform.position, col.transform.position)) //If second player object found, compare the distance to the last.
                {
                    target = col.transform;
                    nearestDistance = Vector3.Distance(transform.position, col.transform.position);
                }
            }
        }

        //Debug.Log("Get target ran, target = " + target.name + " target found = " + targetFound);

        return targetFound;

    }

}
