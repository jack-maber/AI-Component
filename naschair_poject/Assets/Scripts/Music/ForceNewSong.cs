using UnityEngine;

public class ForceNewSong : MonoBehaviour
{
    public Songs newSong;
    public float fadeTime = 1;

    private void OnEnable()
    {
        MusicManager.instance.ChangeSong(newSong, fadeTime);
    }
}
