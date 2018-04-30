using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour 
{
    public CharacterProfile characterProfile;
    public CapsuleCollider capCol;
    public bool isAI;
    ChairMotor ctrl;

    GameObject prefab;

    public CharacterProfile SetRandomCharacter()
    {
        PlayerManager playerManager = GameManager.GetPlayerManager();
        int charCount = Random.Range(0, playerManager.characters.characters.Count);
        characterProfile = playerManager.characters.characters[charCount];
        Init();

        return characterProfile;
    }

	public void Init()
    {
        if (prefab != null)
            Destroy(prefab);

        if (ctrl != null)
            ctrl = null;

        prefab = Instantiate(characterProfile.characterPrefab, transform) as GameObject;
        prefab.transform.position -= new Vector3(0,capCol.height/2,0);
        ctrl = GetComponent<ChairMotor>();
        ctrl.animController = prefab.GetComponent<PlayerAnimationController>();
    }
}
