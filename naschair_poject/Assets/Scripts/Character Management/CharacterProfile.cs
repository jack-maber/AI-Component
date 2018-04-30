using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEditor;

[System.Serializable]
public class CharacterProfile
{
    public string displayName;
    public Sprite displayImage;
    public int characterID;
    public GameObject characterPrefab;
    public Camera_Data camData;
    public AudioEventKey readyUpSound;
}
