using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTrigger : TriggerBase
{
	public float force = 1;
	public Vector3 direction = Vector3.forward;

	void OnTriggerStay(Collider col)
	{
		ChairMotor chairMotor = col.gameObject.GetComponent<ChairMotor> ();
		if(chairMotor != null)
		{
			chairMotor.rBody.AddForce (transform.TransformDirection(direction) * force);
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (transform.position, 0.05f);
		Gizmos.DrawLine (transform.position, transform.position + transform.TransformDirection(direction));
	}
}
