using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireRopeSystem : MonoBehaviour
{
    public GameObject attatchedObject;
    public int nodes = 5;
    public float nodeSpacer = 0.05f, nodeRadius = 0.05f;
    public bool setNodeAsChild = false; //Setting node as child can make the system more stable but will cause issues with interacting with the base objects physics.
    public Material material;

    Vector3 direction;
    LineRenderer lineRend;
    GameObject clone, previousClone;
    float nodeLength = 0, distance = 0;
    List<GameObject> nodeList = new List<GameObject>();

    void Start()
    {
        //Set the base object as previousClone to avoid out of reach errors.
        previousClone = this.gameObject;
        //Store the base object for later use in drawring the wire/rope.
        nodeList.Add(gameObject);
        //The direction from base object to attatched object.
        direction = (attatchedObject.transform.position - transform.position).normalized;

        lineRend = gameObject.AddComponent<LineRenderer>();

        lineRend.SetWidth(nodeRadius, nodeRadius);
        lineRend.useWorldSpace = false;
        lineRend.material = material;
        lineRend.SetVertexCount((int)nodes + 2);

        if (attatchedObject.GetComponent<Collider>() != false)
        {
            //We use a raycast for three reasons
            //1) To get check if the attatched object is in a clear path
            //2) To make sure the nodes don't spawn inside the attatched object.
            //3) To check the distance. 
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit))
            {
                if (hit.collider.gameObject == attatchedObject)
                {
                    distance = Vector3.Distance(transform.position, hit.point);
                    nodeLength = (distance - (nodeSpacer * nodes)) / nodes;
                }
                else
                {
                    Debug.LogError("The space between " + name + " and " + attatchedObject.name + " has a undefined object obstructing the path.");
                    Debug.Break();
                }
            }
        }
        else
        {
            distance = Vector3.Distance(transform.position, attatchedObject.transform.position);
            nodeLength = (distance - (nodeSpacer * nodes)) / nodes;
        }

        for (int i = 0; i < nodes; i ++)
        {
            //Create a new node add it to the list for drawring the line and set it's position.
            clone = new GameObject(gameObject.name + "_Wire");
            nodeList.Add(clone);
            clone.transform.position = (transform.position - ((nodeLength/2) * direction)) + ((direction * (nodeLength + nodeSpacer)) * (i + 1));

            //Set the rotation of the node to face the attatched object.
            Quaternion toRotation = Quaternion.FromToRotation(clone.transform.up, attatchedObject.transform.position - clone.transform.position);
            clone.transform.rotation = toRotation;

            //Add a Collider and set it's length and radius.
            CapsuleCollider col;
            col = clone.AddComponent<CapsuleCollider>();
            col.height = nodeLength;
            col.radius = nodeRadius;

            //Add a Rigidbody.
            clone.AddComponent<Rigidbody>();

            //Add a joint.
            ConfigurableJoint joint;
            joint = clone.AddComponent<ConfigurableJoint>();
            joint.connectedBody = previousClone.GetComponent<Rigidbody>();
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.anchor -= new Vector3(0, (nodeLength / 2) + nodeSpacer, 0);

            //If set node as child is selected set each node to a child of the previous node.
            if (setNodeAsChild)
            {
                clone.transform.parent = previousClone.transform;
            }

            previousClone = clone;
        }

        //For the attatched object.
        //If set node as child is selected set attatched object to a child of the previous node.
        if (setNodeAsChild)
        {
            attatchedObject.transform.parent = clone.transform;
        }

        //Add a fixed joint to the attatched object and store it in the list to draw the line.
        FixedJoint joint2;
        joint2 = attatchedObject.AddComponent<FixedJoint>();
        joint2.connectedBody = nodeList[nodeList.Count - 1].GetComponent<Rigidbody>();
        nodeList.Add(attatchedObject);
    }

    void Update()
    {
        //Draw the line at each point in the list.
        for (int w = 0; w < nodeList.Count; w++)
        {
            if (w < nodeList.Count)
            {
                Vector3 offset = Vector3.zero;

                if (nodeList[w] != gameObject && nodeList[w] != attatchedObject)
                {
                    offset = (nodeLength / 2) * nodeList[w].transform.up;
                }

                lineRend.SetPosition(w, transform.InverseTransformPoint(nodeList[w].transform.position - offset));
            }
        }
    }
}
