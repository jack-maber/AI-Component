using UnityEngine;

public class BackWardsChecker : MonoBehaviour
{
    public float notifyTime = 3;

    PlayerController controller;
    float backWardsTimer = 0;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        controller.profile.uiManager.ShowTurnAround(false);
    }

    private void Update()
    {
        if (controller == null)
            return;

        Vector3 forwardDirection = controller.splineProjector.result.position + controller.splineProjector.result.direction;
        forwardDirection = forwardDirection.normalized;
        forwardDirection.y = transform.position.y;

        if (Vector3.Angle(transform.InverseTransformDirection(forwardDirection) , -transform.forward) <= 30) //If the player has their back to the forward direction
        {
            backWardsTimer += Time.deltaTime;
        }
        else
        {
            backWardsTimer = 0;
        }

        //controller.profile.uiManager.ShowTurnAround((backWardsTimer >= notifyTime));
    }
}
