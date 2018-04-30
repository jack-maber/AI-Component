using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public float laps = 1;

    public ParticleSystem Confetti;
    

    private void Start()
    {
        
        var em = Confetti.emission;
        em.enabled = false;
        laps = 1;
    }


    private void Update()
    {
        if(laps != 0)
        {
           //em.enabled = false;
        };
    }
}
