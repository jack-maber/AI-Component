using UnityEngine;
using System.Collections;

public class SetNewSelected : MonoBehaviour
{
    InputModuleManager mm;
    public static bool lookForSelection = false;

    private void OnEnable()
    {
        mm = GameManager.GetInputModuleManager();
        StartCoroutine(SetActive());
    }

    private void Update()
    {
        if (lookForSelection)
        {
            StartCoroutine(SetActive());
            lookForSelection = false;
        }
    }

    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(0.01f);
        mm.UpdateSelected(gameObject);
    }
}
