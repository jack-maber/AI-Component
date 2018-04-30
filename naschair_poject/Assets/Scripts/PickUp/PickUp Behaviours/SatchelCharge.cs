using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatchelCharge : MonoBehaviour {

    public PickUp_Data pickUpData;
    public RacerPickUp racer;
    //public ParticleSystem ps;
    public GameObject particleEffectObject;
    private Rigidbody satchelRb;


    private void Start()
    {
        StartCoroutine(SelfDestruct());
        satchelRb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Sticky"))
        {
            satchelRb.isKinematic = true;
        }
    }

    IEnumerator SelfDestruct()
    {
        //Automatically detonates the charge if its not detonated manually within its lifetime.
        float lifetime = pickUpData.satchelSelfDestructTimer;
        Debug.Log("Started self destruct timer!");
        yield return new WaitForSeconds(lifetime);

        Debug.Log("Charge lifetime reached, detonating!");
        DetonateCharge();
    }

    public void DetonateCharge()
    {

        Vector3 explosionCenter = transform.position;

        float explosionRadius = pickUpData.satchelRadius;
        float explosionForce = pickUpData.satchelForce;
        float upwardsModifier = pickUpData.upwardsModifier;



        Debug.Log("Satchel Detonated!");

        //Adds explosioon effect to all rigidbodies caught in radius.
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, explosionRadius);
        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Debug.Log("Applying explosion force to: " + col.name);

                bool shielded = false;

                //Checks if target player has a shield.
                if (col.gameObject.CompareTag("Player"))
                {
                    shielded = col.GetComponent<RacerPickUp>().IsShielded(col.gameObject); 
                }

                if (!shielded)
                {
                    rb.AddExplosionForce(explosionForce, explosionCenter, explosionRadius, upwardsModifier);
                }
            }
        }
        //ExplodePS();
        Instantiate(particleEffectObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
        
    }
        

}
