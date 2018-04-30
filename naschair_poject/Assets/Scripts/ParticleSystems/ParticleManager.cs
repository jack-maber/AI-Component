using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    private GameObject spawnObjectArray;
    private ParticleSystem[] psArray;
    private GameObject[] activeParticles;

    public float maxTime = 5;
    public float minTime = 2;
    public float destroyTime = 10;

    private float time;

    private float spawnTime;
    private int Element;
    private bool closePlayer;

    [System.Serializable]
    public class ParticleList
    {
        public GameObject spawnObject;
        public ParticleSystem[] ps;
    }
    public ParticleList[] particleLists;
 
    

  
	// Use this for initialization
	void Start ()
    {
        SetRandomTime();
        time = minTime;       
	}

    // Update is called once per frame
    void FixedUpdate ()
    {
        //counts up
        time += Time.deltaTime;
        
        CheckTimeCallMethods();
        
	}

    void CheckTimeCallMethods()
    {
        //check if its the right time to spawn object
        if (time >= spawnTime)
        {
            GetRandomElement();
            SpawnObject();
            enableParticles();
            SetRandomTime();
        }
    }
    

    void SpawnObject()
    {
        time = 0;
        spawnObjectArray = Instantiate(spawnObjectArray, transform.position, spawnObjectArray.transform.rotation);
        //adds timer to destroy object
        Destroy(spawnObjectArray, destroyTime);      
    }


    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);       
    }


    void enableParticles()
    {
        for (int i = 0; i < psArray.Length; i++)
        {
            psArray[i].Play();         
        }                
    }


    //Selects a random element from the particle manager to spawn.
    void GetRandomElement()
    {
        while (closePlayer == false)
        {
            Element = Random.Range(0, particleLists.Length);
            spawnObjectArray = particleLists[Element].spawnObject;

            // fill local array with particle systems that have been selected
            psArray = particleLists[Element].ps;

            //Check if player is close
            CheckNearPlayer();
        }
    }        
    
    void CheckNearPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(Vector3.zero, 50);

        if (closePlayer == false)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i] != null)
                {
                    closePlayer = true;
                    Debug.Log("Player close to particle");
                }
            }
        }
    }
}
