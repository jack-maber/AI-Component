using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class ChairMotor2 : MonoBehaviour
{

    //Declares positions for checkpoints
    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;

    public NavMeshAgent agent;
    public float Speed = 15f;

    

    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>(); //Sets NevMash Agent so they can set speed individually 
        agent.speed = Speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "1") //When the chair AI collides with the triggers it sets the position to be the next checkpoint 
        {
            agent.SetDestination(pos2.position);
        }
        if (other.tag == "2")
        {
            agent.SetDestination(pos3.position);
        }
        if (other.tag == "3")
        {
            agent.SetDestination(pos4.position);
        }
        if (other.tag == "4")
        {
            agent.SetDestination(pos5.position);
        }
        if (other.tag == "5")
        {
            agent.SetDestination(pos1.position);
        }
    }

    //This raycasts to the nearest ChairAI and then slows down to stop bunching up issues
    public bool TagCast(NavMeshAgent agent, Ray ray, out RaycastHit hit, float distance, string Chair, int layerMask = System.Int32.MaxValue)
    {
        
        RaycastHit[] hits = Physics.RaycastAll(ray, distance, layerMask);
        hit = new RaycastHit();
        distance = 2; //Sets minimum distance for slow down
        hit.distance = distance;
        foreach (RaycastHit ahit in hits)
        {
            if (ahit.distance < hit.distance && ahit.transform.CompareTag(Chair))
            {
                hit = ahit;
            }
        }

        if (hit.distance < distance) //If the hit distance is less than the preset, trigger the speed decrease 
        {
            agent.speed = 6; //Sets speed to around half to let others pass
            return true;
        }

            
        else //All else fails return false 
        {
            return false;
        }
            
    }











}
