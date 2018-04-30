using UnityEngine;

public class AIChangePathTrigger : TriggerBase
{
    private void OnTriggerEnter(Collider other)
    {
        AI_Racer racer = other.GetComponent<AI_Racer>();
        if (racer != null)
            racer.SetRandomAiPath();
    }
}
