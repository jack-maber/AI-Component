using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[System.Serializable]
public class PlayerClass
{
    public int playerID;
    public int score;
    public Player playerInstance; //Rewired input (controller)
    //public GameObject characterObject;
    public bool ready = false; //Status for character selection screen.
    public CharacterProfile characterProfile; //Selected character data, such name, image etc.
    public GameObject activePlayerPrefab; //Gameobject for the current scene.
    public GameObject activeCamera; //Players camera in the scene.
    public CharacterProfileEditor profileEditor; //Interface used for updating this players selection panel in char selection screen.
    public RacerPickUp pickUpManager;
    public CameraFOV camFOV;
    public PlayerController controller;
    public PlayerUI_Display uiManager;
    public FixedCamera cam;
    public CharacterSwitcher characterSwitcher;

    [Header("Lap Info")]
    public int position = 0; //Race position in scene.
    public int lap = 0;
    public float splineDist = 0; //Position along the spline in scene.
}
