using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class CharacterProfileEditor : MonoBehaviour
{
    public Text displayName;
    public Image profile;
    public Text message;
    public Image baseImage;
    public Text characterInputHint;
    public StudioEventEmitter readySound;

    public void UpdateProfile(CharacterProfile newProfile)
    {
        UpdateDisplayName(newProfile.displayName);
        UpdateImage(newProfile.displayImage);
    }

    public void UpdateDisplayName(string newName)
    {
        displayName.text = newName;
    }

    public void UpdateMessage(string newMessage, bool addMessage = false)
    {
        if (addMessage)
            message.text += newMessage;

        if (!addMessage)
            message.text = newMessage;
    }

    public void UpdateImage(Sprite newImage)
    {
        profile.overrideSprite = newImage;
    }

    public void UpdateBaseColour(Color newColor, bool addColour = false)
    {
        if (addColour)
            baseImage.color += newColor;

        if (!addColour)
            baseImage.color = newColor;
    }

    public void UpdateCharacterInputHint(PlayerClass playerProfile, bool enabled)
    {
        if (enabled)
        {
            string leftButton = playerProfile.playerInstance.controllers.maps.GetFirstElementMapWithAction("NextCharacter", true).elementIdentifierName;
            string rightButton = playerProfile.playerInstance.controllers.maps.GetFirstElementMapWithAction("PreviousCharacter", true).elementIdentifierName;
            characterInputHint.text = leftButton + " and " + rightButton + " to change character";
        }
        else
        {
            characterInputHint.text = "";
        }
    }

    public void UpdateSound(string newEvent)
    {
        readySound.Event = newEvent;
    }
}
