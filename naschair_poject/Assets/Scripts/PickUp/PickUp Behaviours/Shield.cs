using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : PickUpBehaviour {

    public bool shielded = false;
    private GameObject myShield;
    private Animator shieldController;
    private bool shieldRemoved = false;
	
	
	void FixedUpdate () {

        if (motor.inputVaraibles.usePickupPressed & !shielded)
        {
            StartCoroutine(ShieldActive());
        }

    }

    IEnumerator ShieldActive()
    {
        //Applies the shield effect for the duration selected in data.
        float duration = pickUpData.shieldDuration;
        shielded = true;
        SpawnShield();
        shieldController = myShield.GetComponent<Animator>();
        yield return new WaitForSeconds(duration);
        RemoveShield();
        RemovePickup();
    }

    public void RemoveShield()
    {
        if (!shieldRemoved)
        {
            shieldRemoved = true;
            shieldController.SetTrigger("Destroy");
        }
    }

    void SpawnShield()
    {
        myShield = Instantiate(pickUpData.shieldObject, transform.position, Quaternion.identity);
        myShield.transform.SetParent(transform);
    }
}
