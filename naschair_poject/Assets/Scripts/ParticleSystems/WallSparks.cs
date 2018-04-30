using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSparks : MonoBehaviour
{
    public ParticleSystem Sparks;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TrackObject")
        {
            Sparks.Play();
        }

        else
        {
            Sparks.Pause();
        }


    }


}
