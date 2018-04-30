using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public GameObject emmiterPrefab;
    public int emmiterCount;

    public List<AudioEvent> events = new List<AudioEvent>();

    List<StudioEventEmitter> emmiterPool = new List<StudioEventEmitter>();

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        for (int i = 0; i < emmiterCount; i++)
        {
            AddEmmiter();
        }
    }

    public void CallAudio(AudioEventKey key, Vector3 position)
    {
        if (emmiterPool.Count <= 0)
            AddEmmiter();

        StudioEventEmitter newEmitter = emmiterPool[0];
        emmiterPool.Remove(newEmitter);
        newEmitter.gameObject.SetActive(true);

        newEmitter.transform.position = position;

        newEmitter.Event = GetEventFromKey(key);
        StartCoroutine(UseEmmiter(newEmitter, new WaitUntil(() => !newEmitter.IsPlaying())));   
    }

    private void ReCallAudio(StudioEventEmitter emitter)
    {
        emitter.transform.localPosition = Vector3.zero;
        emitter.Stop();
        emitter.Event = "";
        emitter.gameObject.SetActive(false);
        emmiterPool.Add(emitter);
    }

    private IEnumerator UseEmmiter(StudioEventEmitter emitter, WaitUntil wait)
    {
        emitter.Play();
        yield return wait;
        ReCallAudio(emitter);
    }

    private void AddEmmiter()
    {
        GameObject clone = Instantiate(emmiterPrefab);
        clone.transform.parent = transform;
        clone.transform.localPosition = Vector3.zero;

        StudioEventEmitter fEmit = clone.GetComponent<StudioEventEmitter>();
        emmiterPool.Add(fEmit);

        clone.SetActive(false);
    }

    private string GetEventFromKey(AudioEventKey key)
    {
        for (int i = 0; i < events.Count; i ++)
        {
            if (events[i].key == key)
                return events[i].eventPath;
        }

        return "";
    }
}
