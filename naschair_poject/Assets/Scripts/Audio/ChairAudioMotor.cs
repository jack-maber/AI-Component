using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChairAudioMotor : MonoBehaviour
{
    public ChairMotor chairMotor;

    public StudioEventEmitter movmentSound;
    public StudioEventEmitter brakeSound;
    public StudioEventEmitter collisionSound;

	void Update()
	{
		PlayAudio ();
	}

    //void OnCollisionEnter(Collision other)
    //{
    //    collisionSound.Play();
    //}

    void PlayAudio()
    {
		if (chairMotor.rBody.velocity.magnitude >= 0.1f && !movmentSound.IsPlaying() && chairMotor.CheckGrounded())
        {
            movmentSound.Play();
        }
		else if (chairMotor.rBody.velocity.magnitude < 0.1f || !chairMotor.CheckGrounded())
        {
            movmentSound.Stop();
        }

        movmentSound.SetParameter("velocity", chairMotor.rBody.velocity.magnitude);

        float buffer = 0.5f;

        if (chairMotor.inputVaraibles.brakePressed && chairMotor.CheckGrounded() && chairMotor.rBody.velocity.magnitude >= buffer && !brakeSound.IsPlaying())
        {
            brakeSound.Play();
        }
        else if ((!chairMotor.inputVaraibles.brakePressed || !chairMotor.CheckGrounded() || chairMotor.rBody.velocity.magnitude < buffer) && brakeSound.IsPlaying())
        {
            brakeSound.Stop();
        }

        brakeSound.SetParameter("isBraking", chairMotor.rBody.velocity.magnitude);

    }

    private void OnCollisionEnter(Collision col)
    {
        float colVelocity = col.relativeVelocity.magnitude;

        if (colVelocity > 0.1f) //If the velocity is above the minimum threshold, a sound is viable.
        {
            collisionSound.Play();
            collisionSound.SetParameter("collisionVelocity", colVelocity);
        }
    }
}
