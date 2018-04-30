using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public List<SongManager> songs = new List<SongManager>();

    SongManager activeTrack;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeSong(Songs newTrack, float fadeTime)
    {
        for (int i = 0; i < songs.Count; i++)
        {
            if(newTrack == songs[i].songType)
            {
                StartCoroutine(SongTransition(activeTrack, songs[i], fadeTime));
                activeTrack = songs[i];
            }
        }
    }

    IEnumerator SongTransition(SongManager song1, SongManager song2, float fadeTime)
    {
        if(song1 != null)
        {
            while (song1.volume > 0)
            {
                song1.volume -= Time.deltaTime / (fadeTime * 0.5f);
                song1.emitter.SetParameter("Volume", song1.volume);
                yield return null;
            }

            song1.emitter.Stop();
        }

        song2.emitter.Play();

        while (song2.volume < 1)
        {
            song2.volume += Time.deltaTime / (fadeTime * 0.5f);
            song2.emitter.SetParameter("Volume", song2.volume);
            yield return null;
        }
    }
}
