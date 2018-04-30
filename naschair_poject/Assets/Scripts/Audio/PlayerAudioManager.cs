using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour {

    public GameObject root;
    public float minChairSoundCutoff = 0.01f; //Lower range of chair velocity sound modificiation.
    public float maxChairSoundCutoff = 50f; //Max range of chair velocity sound modificiation.

    public float minColSoundCutoff = 0.5f;
    public float maxColSoundCutoff = 80f;

    private ChairMotor playerControl;
    private Rigidbody rb;

    private void Start()
    {
        rb = root.GetComponent<Rigidbody>();
        playerControl = root.GetComponent<ChairMotor>();
    }


    public float ChairMovementAudio() //If the chair is on the ground and moving, it returns velocity which can be used to manipulate audio. 0f = no sound.
    {

        float chairSoundModifier = 0f;

        if (playerControl.CheckGrounded()) //Only parse velocity for manipulation of chair sound if it is on the floor.
        {
            chairSoundModifier = rb.velocity.magnitude;
            chairSoundModifier = Mathf.Clamp(chairSoundModifier, minChairSoundCutoff, maxChairSoundCutoff); //Clamps the velocity value between set limits, so that too small or large sounds are not produced.
            chairSoundModifier = (chairSoundModifier == minChairSoundCutoff) ? 0f : chairSoundModifier; //If the velocity is at the bottom of the range, it is parsed as 0 to indicate no sound.
            //Debug.Log("Chair Sound: " + chairSoundModifier);
        }

        return chairSoundModifier; //If this returns 0, that means no sound should be made at all.

    }

    private void Update()
    {
        ChairMovementAudio();
    }

    private void OnCollisionEnter(Collision col)
    {
        float colVelocity = Mathf.Clamp(col.relativeVelocity.magnitude, 0f, maxColSoundCutoff); //Clamps the colVelocity below the maximum threshold.

        if (colVelocity > minColSoundCutoff) //If the velocity is above the minimum threshold, a sound is viable.
        {           

            //TODO Play collision sound.
            //Debug.Log(colVelocity);
            //Debug.Log("Player Generic Collision Sound! Modifier: " + colVelocity);

            if (col.gameObject.CompareTag("Locker"))
            {
                //Debug.Log("Player Locker Collision Sound!");
                //TODO Play locker sound.
            }

        }

    }

}
