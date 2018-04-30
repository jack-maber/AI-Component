using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour 
{
    public bool dontDestroyOnLoad = false;
    bool isGlobal = false;

	public virtual void Awake() 
	{
        if (GameManager.CheckInstanceIsSet(this))
        {
            Destroy(this);
        }
        else
        {
            isGlobal = true;
            GameManager.UpdateInstance(this.MemberwiseClone());

            if (dontDestroyOnLoad)
                GameObject.DontDestroyOnLoad(this);
        }
	}

    private void OnDestroy()
    {
        if(isGlobal)
            GameManager.ClearInstance(this);
    }
}
