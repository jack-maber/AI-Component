using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChairSpawn_Point : PointBase
{
    [SerializeField]
    public int width;
    [SerializeField]
    public float horizontalSpacing;
    [SerializeField]
    public float verticalSpacing;
    [SerializeField]
    public int spawnPointCount;

    private void Awake()
    {
        //ReBuildPoints();
    }

    public void ReBuildPoints()
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform t in transform)
        {
            children.Add(t);
        }

        for (int i = 0; i < children.Count; i++)
        {
            DestroyImmediate(children[i].gameObject);
        }

        points.Clear();

        int height = Mathf.RoundToInt(spawnPointCount / width);

        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                Vector3 newPoint = transform.position;
                newPoint += transform.TransformDirection(new Vector3(y * horizontalSpacing, 0, -x * verticalSpacing));
                GameObject clone = new GameObject("SpawnPoint");
                clone.transform.position = newPoint;
                clone.transform.rotation = transform.rotation;
                clone.transform.parent = transform;
                points.Add(clone.transform);
            }
        }
    }
}
