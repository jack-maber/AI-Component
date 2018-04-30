using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    public Camera_Data camData;
    public Transform target;
    public Camera uiCam;
    public Camera viewCam;

    PlayerClass playerInstance;
    Vector3 targetPosition;
    Vector3 velocity, rotVelocity;

    Transform tracker;

    RaycastHit hit;
    Ray ray;

    private void Awake()
    {
        if(camData == null)
            camData = (Camera_Data)Resources.Load("CameraData");
        //Debug.Assert(target, "No target found on: " + gameObject.name, gameObject);

        ray = new Ray();

        tracker = new GameObject("Tracker").GetComponent<Transform>();
    }

    public void Init()
    {
        uiCam.rect = GetComponent<Camera>().rect;
    }

    public void UpdatePlayerInstance(PlayerClass p)
    {
        playerInstance = p;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void UpdateTracker()
    {
        tracker.position = target.position;
        //tracker.eulerAngles = Vector3.SmoothDamp(tracker.eulerAngles, target.eulerAngles, ref rotVelocity, camData.smoothRotTime);
        tracker.rotation = Quaternion.Slerp(tracker.rotation, target.rotation, camData.smoothRotTime * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (target == null || camData == null)
            return;

        UpdateTracker();
        Vector3 direction = tracker.TransformDirection(new Vector3(0, camData.cameraHeight, -camData.cameraDistance));
        targetPosition = tracker.position + direction;

        float dist = Vector3.Distance(tracker.position, targetPosition); //Distance the target positon is from the cameras target
        if (Physics.Raycast(tracker.position, direction.normalized, out hit, dist))
        {
            ray.origin = tracker.position;
            ray.direction = direction.normalized;

            targetPosition = ray.GetPoint(hit.distance - camData.collisionSize);
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, camData.smoothTime);
        transform.LookAt(tracker.position + (tracker.forward * camData.centerOffset));

        Vector3 newRot = transform.eulerAngles;
        //newRot.x = 0;
        newRot.z = 0;
        transform.eulerAngles = newRot;
    }

    public void InverseCameraView()
    {
        camData.cameraDistance = -camData.cameraDistance;
        camData.centerOffset = -camData.centerOffset;
    }
}
