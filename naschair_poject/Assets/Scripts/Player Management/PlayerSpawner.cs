using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

    public GameObject playerObject;
	
    public GameObject SpawnPlayer (PlayerClass profile, Vector3 newPosition, Quaternion newRotation)
    {
        GameObject newPlayer = Instantiate(playerObject, newPosition, newRotation);

        PlayerController newController = newPlayer.GetComponent<PlayerController>();
        newController.playerInputObject = profile.playerInstance; //Sets the new players rewired player object to that of the associated profile.

        profile.controller = newController;
        newController.profile = profile;

        profile.pickUpManager = newPlayer.GetComponent<RacerPickUp>();

        CharacterSwitcher characterSwitcher = newPlayer.GetComponent<CharacterSwitcher>();
        characterSwitcher.characterProfile = profile.characterProfile;
        characterSwitcher.Init();

        return newPlayer;
    }
}
