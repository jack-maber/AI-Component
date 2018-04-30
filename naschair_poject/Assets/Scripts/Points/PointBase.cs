using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointBase : MonoBehaviour
{
    [SerializeField]
    public string gizmoName = "Chair_SpawnPoint.png";
    [HideInInspector, SerializeField]
    public List<Transform> points = new List<Transform>();

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawIcon(points[i].position, gizmoName);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(points[i].position, points[i].position + points[i].forward);
        }
    }
}
