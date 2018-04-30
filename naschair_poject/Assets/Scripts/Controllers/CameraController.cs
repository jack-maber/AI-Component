using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    Camera_Data data;
    Transform tracker;
    Vector3 velocity, velocity2;

    private void Awake()
    {
        tracker = new GameObject(gameObject.name + "[Tracker]").GetComponent<Transform>(); //Create the tracker object and store it's transform
        tracker.transform.position = transform.position; //Move the tracker to the camera position so it moves behind the player
        data = (Camera_Data)Resources.Load("ActionCamera");
    }

    private void FixedUpdate()
    {
        if(target != null)
            MoveCamera(); //Move the camera in fixed update as it is following an object that uses a rigidbody
    }

    void MoveCamera()
    {
        float dist = Vector3.Distance(target.position, tracker.position); //Distance between the tracker and the target

        tracker.LookAt(target); //Look at the target
        Vector3 tempRot = tracker.eulerAngles; //Store the rotation so we can change just the x axis
        tempRot.x = 0; //Set the x axis to 0 so it's looking forward in the world space
        tracker.eulerAngles = tempRot; //Set the rotation again

        if (dist > 2.1f)
        {
            tracker.position = Vector3.SmoothDamp(tracker.position, target.position, ref velocity, data.smoothTime); //If we are out the buffer range then move the tracker towards the target
        }
        else if(dist < 2f)
        {
            tracker.position -= (tracker.forward * 4) * Time.fixedDeltaTime; //If the target has moved towards the tracker then move the tracker back
        }

        //Set the tracker's y position to match the target
        Vector3 newTrackPos = tracker.position;
        newTrackPos.y = target.position.y;
        tracker.position = Vector3.SmoothDamp(tracker.position, newTrackPos, ref velocity2, data.smoothTime);

        Vector3 finalPos = tracker.position + tracker.TransformDirection(new Vector3(0, data.cameraHeight, -data.cameraDistance)); //Create a position that is offset from the tracker by the defined amount
        //transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref velocity2, data.smoothTime); //Smoothly move the camera to the new offset position
        transform.position = finalPos;
        transform.LookAt(target); //Look at the target
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (Application.isPlaying)
            Gizmos.DrawSphere(tracker.position, 0.1f);
    }
}
