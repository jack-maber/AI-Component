using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageOffset : MonoBehaviour
{
    public Vector2 direction;
    public float minCycleTime = 1f, maxCycleTime = 2f;
    public AnimationCurve speedOverRange;
    public RawImage image;

    float cycleTime = 0;

    private void Awake()
    {
        cycleTime = Random.Range(minCycleTime, maxCycleTime);

        if (image == null)
            image = GetComponent<RawImage>();

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            float range = 0;
            while(range < 1)
            {
                Rect newRect = image.uvRect;
                newRect.x = Mathf.Lerp(0, direction.x, speedOverRange.Evaluate(range));
                newRect.y = Mathf.Lerp(0, direction.y, speedOverRange.Evaluate(range));
                image.uvRect = newRect;
                range += Time.deltaTime / cycleTime;
                yield return null;
            }
            yield return null;
        }
    }
}
