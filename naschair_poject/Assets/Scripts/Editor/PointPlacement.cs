using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PointPlacement : EditorWindow
{
    public GameObject pointToSpawn;
    public Vector3 offset = Vector3.zero;
    bool isOn = false;
    string oppositeState = "On";
    Color activeColour = Color.green;

    [MenuItem("Naschair/Point Placment")]
    public static void Init()
    {
        PointPlacement window = (PointPlacement)EditorWindow.GetWindow(typeof(PointPlacement));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Point Placment");
        DrawSpawnPointPlacment();
    }

    void DrawSpawnPointPlacment()
    {
        EditorGUILayout.BeginVertical("box");

        GUI.color = activeColour;
        if(GUILayout.Button("Turn " + oppositeState))
        {
            isOn = !isOn;
            oppositeState = (isOn) ? "Off" : "On";
            activeColour = (isOn) ? Color.green : Color.red;
        }
        GUI.color = Color.white;

        if (isOn)
        {
            pointToSpawn = EditorGUILayout.ObjectField(pointToSpawn, typeof(Object), true) as GameObject;
            offset = EditorGUILayout.Vector3Field("Offset", offset);
        }

        EditorGUILayout.EndVertical();
    }

    void OnSceneGUI(SceneView sceneView)
    {
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && pointToSpawn != null && isOn)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
                return;

            GameObject clone = PrefabUtility.InstantiatePrefab(pointToSpawn) as GameObject;
            clone.transform.position = hit.point + offset;

            ChairSpawn_Point spawnPoint = clone.GetComponent<ChairSpawn_Point>();               

            Undo.RegisterCreatedObjectUndo(clone, "Placed: " + clone.name);
        }
    }

    void OnFocus()    // Window has been selected
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;        // Remove delegate listener if it has previously been assigned.
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;        // Add (or re-add) the delegate.
    }
}
