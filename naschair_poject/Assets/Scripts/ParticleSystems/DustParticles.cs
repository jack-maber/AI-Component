using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticles : MonoBehaviour
{
    public ParticleSystem[] ps;
    List<ParticleSystem.EmissionModule> em = new List<ParticleSystem.EmissionModule>();
    public ChairMotor chairMotor;

    public virtual void Awake()
    {
        foreach (ParticleSystem p in ps)
        {
            em.Add(p.emission);
        }

        chairMotor = GetComponent<ChairMotor>();
    }


    void UpdateParticalEffects()
    {
        for (int i = 0; i < em.Count; i++)
        {
            if (em[i].enabled != chairMotor.CheckGrounded())
            {
                ParticleSystem.EmissionModule tempMod = em[i];
                tempMod.enabled = chairMotor.CheckGrounded();
                em[i] = tempMod;
            }
        }
    }







}
