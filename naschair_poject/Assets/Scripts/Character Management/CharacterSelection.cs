using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Rewired;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{

    public IList<Player> players; //List of Rewired players presently configured.
    public Character_Data characterData; //Reference to scriptable object with character data, updated to reflect availability of characters.
    public GameObject[] profileUIPanels; //The UI elements that represent player character selection.
    public IList<CharacterProfileEditor> profileEditors; //Used to store profile editor instances attached to the elements of profileUIPanels.
    public string nextScene = "LevelSelect";

    private PlayerManager pm; //Tracks players actively in the game, PlayerClass instances store useful player data.
    private int playerCount; //TEMP rudimentary way of assigning player numbers while backing out isn't possible. Will need to be changed when backing out is enabled.

    public GameObject playerObject;
    public Transform playerSpawnPoint;
    public Text readyMessage;

    public virtual void Start()
    {
        pm = GameManager.GetPlayerManager();
        pm.ClearPlayers();

        players = ReInput.players.GetPlayers(); //So their input can be checked.
        playerCount = 0; //TEMP
        profileEditors = new List<CharacterProfileEditor>();

        foreach (CharacterStorage_Data character in characterData.characters)
        {
            character.isSelected = false; //Because previous use can change the values of the scriptable object.
        }

        for (int i = 0; i < profileUIPanels.Length; i++)
        {
            CharacterProfileEditor panelEditor = profileUIPanels[i].GetComponent<CharacterProfileEditor>();
            profileEditors.Add(panelEditor); //So that each panel can be updated.

            panelEditor.UpdateMessage("Press the any button to join", false);
        }

        if (pm.singleplayerMode)
        {
            SinglePlayerSetup();
            //Debug.Log("SP SETUP!");
        } 
    }

    private void Update()
    {
        if (!pm.singleplayerMode)
        {
            NewPlayerCheck();
            UpdateStartMessage();
        }

        if (pm.activePlayers.Count > 0)
            foreach (PlayerClass playerClass in pm.activePlayers)
            {
                ChangeCharacterCheck(playerClass);
                ReadyCheck(playerClass);
                GameStartCheck(playerClass);
                UpdatePlayerPanel(playerClass);
                //LeavingPlayerCheck(playerClass);
            }
    }

    void UpdateStartMessage()
    {
        if (pm.activePlayers.Count > 0 & IsAnyCharacterReady() && !AllPlayersReady())
        {
            readyMessage.text = "All players must be ready to start the game";
        }
        else if(pm.activePlayers.Count > 0 && AllPlayersReady())
        {
            Player player = ReInput.players.GetPlayer(0);
            if(player != null)
            {
                string startGameInput = player.controllers.maps.GetFirstElementMapWithAction("StartGame", true).elementIdentifierName;
                readyMessage.text = "Press " + startGameInput + " to start the game";
            }
        }
        else
        {
            readyMessage.text = "";
        }
    }

    private void SinglePlayerSetup()
    {
        NewPlayerSetup(players[0]);

        for (int panelCount = 1; panelCount < profileUIPanels.Length; panelCount++)
        {
            profileUIPanels[panelCount].SetActive(false);
        }
    }

    private void ChangeCharacterCheck(PlayerClass pClass) //Checks active players for change character input.
    {
        if (!pClass.ready) //Can't change character if already readied up.
        {
            Player player = pClass.playerInstance;

            if (player.GetButtonDown("NextCharacter"))
            {
                CycleCharacter(1, pClass);
            }

            if (player.GetButtonDown("PreviousCharacter"))
            {
                CycleCharacter(-1, pClass);
            }
        }
    }

    private void NewPlayerCheck() //Checks if a new player joined the game.
    {
        foreach (Player player in players) //Checks all player inputs.
        {
            if (player.GetAnyButtonDown())
            {
                if (!pm.PlayerAlreadyPlaying(player)) //Cannot add a player if they are already playing.
                {
                    NewPlayerSetup(player); //Creates a new profile and adds it to the list.
                }
            }
        }
    }

    //private void LeavingPlayerCheck(PlayerClass pClass)
    //{

    //    Player player = pClass.playerInstance;

    //    if (player.GetButtonDown("LeaveGame"))
    //    {
    //        pm.RemovePlayer(pClass); //Toggles player ready status.
    //    }

    //}

    private void ReadyCheck(PlayerClass pClass)//Checks if an active player is attempting to ready up.
    {

        Player player = pClass.playerInstance;

        if (player.GetButtonDown("Ready"))
        {
            PlayerReady(pClass); //Toggles player ready status.
        }

    }

    private void GameStartCheck(PlayerClass pClass)
    {
        Player player = pClass.playerInstance;

        if (player.GetButtonDown("StartGame") & AllPlayersReady())
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    private bool AllPlayersReady()
    {
        foreach (PlayerClass playerClass in pm.activePlayers) //So all active players are checked.
        {
            if (!playerClass.ready)
                return false;
        }
        return true;
    }

    private void PlayerReady(PlayerClass player) //Sets the player ready/unready, sets char as in use/not in use.
    {
        CharacterStorage_Data selectedCharacterData = GetCharacterData(player.characterProfile.characterID);

        if (!selectedCharacterData.isSelected | player.ready) //So that in use characters in use cannot be readied, unless a player is trying to unready.
        {
            player.ready = !player.ready;
            selectedCharacterData.isSelected = player.ready;

            if(player.ready)
                AudioManager.instance.CallAudio(player.characterProfile.readyUpSound, Vector3.zero);
        }
    }

    private void UpdatePlayerPanel(PlayerClass pClass) //Updates panels which match active players. Has TEMP features.
    {        
        int pID = pClass.playerInstance.id; //TEMP this method will likely need to change when adding remove player functionality.
        CharacterProfile currentProfile = pClass.characterProfile;

        UpdatePanel(pClass, currentProfile);
    }

    private void UpdatePanel(PlayerClass player, CharacterProfile currentProfile) //Updates panel text and invokes method to update the image.
    {
        player.profileEditor.UpdateDisplayName(currentProfile.displayName);

        if (player.ready)
        {
            player.profileEditor.UpdateMessage("Ready", false);

            player.profileEditor.UpdateCharacterInputHint(player, false);
        }
        else
        {
            string startAction = player.playerInstance.controllers.maps.GetFirstElementMapWithAction("Ready", true).elementIdentifierName;
            player.profileEditor.UpdateMessage("Press " + startAction, false);

            player.profileEditor.UpdateCharacterInputHint(player, true);
        }

        player.profileEditor.UpdateImage(currentProfile.displayImage);

    }

    private void NewPlayerSetup(Player player) //Creates and sets up a new PlayerClass within the player manaager for a Rewired Player.
    {
        PlayerClass newPlayer = new PlayerClass();

        newPlayer.playerInstance = player; //Sets the profiles player object to the one which hit join.
        newPlayer.profileEditor = profileEditors[playerCount];

        playerCount++;

        int startingCharacterID = AssignStartingCharacterID();
        CharacterStorage_Data newCharacterData = GetCharacterData(startingCharacterID);
        newPlayer.characterProfile = NewCharacterProfile(newCharacterData);

        GameObject clone = Instantiate(playerObject, playerSpawnPoint.position, playerSpawnPoint.rotation);
        newPlayer.activePlayerPrefab = clone;

        newPlayer.controller = clone.GetComponent<PlayerController>();
        newPlayer.controller.playerInputObject = player;

        if (pm.singleplayerMode)
            newPlayer.controller.disableMovement = true;

        CharacterSwitcher switcher = clone.GetComponent<CharacterSwitcher>();
        newPlayer.characterSwitcher = switcher;
        switcher.characterProfile = NewCharacterProfile(newCharacterData);
        switcher.Init();

        pm.AddPlayer(newPlayer);  //Adds a new profile to the player list.     
    }

    private void CycleCharacter(int changeDirection, PlayerClass player) //Takes the change character input from a player, then finds the next appropriate character for selection/
    {
        int currentCharacterID = player.characterProfile.characterID;

        bool newCharacterFound = false;

        int newCharacterID = currentCharacterID;
        while (!newCharacterFound)
        {
            newCharacterID += changeDirection;
            newCharacterID = (newCharacterID < 0) ? (characterData.characters.Count - 1) : newCharacterID; //TOMNOTE Character data count was 3.
            newCharacterID = (newCharacterID > (characterData.characters.Count - 1)) ? 0 : newCharacterID;

            if (!CharacterInUse(newCharacterID) | newCharacterID == currentCharacterID)
            {
                ChangeCharacter(newCharacterID, player);
                newCharacterFound = true;
            }
        }
    }

    private bool CharacterInUse(int characterID) //Finds whether or not a character is already in use by another player.
    {
        bool characterInUse = false;
        foreach (CharacterStorage_Data character in characterData.characters)
        {
            if (character.characterID == characterID)
            {
                characterInUse = (character.isSelected);
                break;
            }
        }
        return characterInUse;
    }

    private CharacterProfile NewCharacterProfile(CharacterStorage_Data characterData) //Creates a character profile from stored data for use within a PlayerClass.
    {
        CharacterProfile newProfile = new CharacterProfile();
        newProfile = characterData;
        return newProfile;
    }

    private int AssignStartingCharacterID() //Pulls a non-used character from the list of characters for initial use by a player.
    {
        for (int i = 0; i < characterData.characters.Count - 1; i++)
        {
            if (!CharacterInUse(i))
            {
                return i;
            }
        }
        return 0;
    }

    private void ChangeCharacter(int newCharacterID, PlayerClass playerClass) //Changes a players selected character to one of an ID, through updating their character profile.
    {
        CharacterStorage_Data newCharacterData = GetCharacterData(newCharacterID);
        playerClass.characterProfile = NewCharacterProfile(newCharacterData);
        playerClass.characterSwitcher.characterProfile = NewCharacterProfile(newCharacterData);
        playerClass.characterSwitcher.Init();
    }

    private CharacterStorage_Data GetCharacterData(int characterID) //Grabs data relevant to a characterID
    {
        CharacterStorage_Data targetCharacter = null;

        characterID = Mathf.Clamp(characterID, 0, (characterData.characters.Count - 1));

        foreach (CharacterStorage_Data character in characterData.characters)
        {
            if (character.characterID == characterID)
            {
                targetCharacter = character;
                break;
            }
        }
        return targetCharacter;
    }

    public bool IsAnyCharacterReady()
    {
        foreach (PlayerClass players in pm.activePlayers)
        {
            if (players.ready)
                return true;
        }

        return false;
    }
}