using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputModuleManager : BaseManager
{
    public EventSystem eventSystem;

    public void UpdateSelected(GameObject newObject)
    {
        eventSystem.SetSelectedGameObject(newObject);
    }
}
