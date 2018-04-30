using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBase : MonoBehaviour
{
    public Mesh mesh;
    public Vector3 scale;
	public Color colour = new Color(0,1,0,.6f);

    BoxCollider bc;

    private void Awake()
    {
        bc = gameObject.AddComponent<BoxCollider>();
        bc.size = scale;
        bc.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        if (mesh == null)
            return;

        Gizmos.color = colour;
        Gizmos.DrawMesh(mesh, transform.position, transform.rotation, scale);
    }
}
