using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrigger : TriggerBase
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ChairMotor cm = other.GetComponent<ChairMotor>();
            //Debug.Log(cm);
            StartCoroutine(cm.ResetRotPos(0f));
        }
    }
}
