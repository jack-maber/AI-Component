using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceCharacterSelect : MonoBehaviour
{
    public GameObject racerPrefab;
    public Transform spawnPoint;

    public void SpawnPlayer()
    {
        GameObject clone = Instantiate(racerPrefab, spawnPoint);
    }
}
