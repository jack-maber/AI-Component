#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChairSpawn_Point)), System.Serializable]
public class ChairSpawn_PointEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ChairSpawn_Point newTarget = (ChairSpawn_Point)target;

        EditorGUILayout.BeginVertical("box");
        newTarget.width = EditorGUILayout.IntField("Width", newTarget.width);
        newTarget.spawnPointCount = EditorGUILayout.IntField("Spawn Point Count", newTarget.spawnPointCount);
        newTarget.horizontalSpacing = EditorGUILayout.FloatField("Horiztonal Spacing", newTarget.horizontalSpacing);
        newTarget.verticalSpacing = EditorGUILayout.FloatField("Vertical Spacing", newTarget.verticalSpacing);
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("ReBuild Spawn Points"))
            newTarget.ReBuildPoints();

        EditorUtility.SetDirty(target);
    }
}

#endif