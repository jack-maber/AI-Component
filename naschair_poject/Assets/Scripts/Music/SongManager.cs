using UnityEngine;
using FMODUnity;

[System.Serializable]
public class SongManager
{
    public Songs songType;
    public StudioEventEmitter emitter;
    [Range(0,1)]
    public float volume;
}
