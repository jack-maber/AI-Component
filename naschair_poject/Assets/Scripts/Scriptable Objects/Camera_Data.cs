using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraData", menuName = "RockPool/Data Instace/Camera Data", order = 1)]
public class Camera_Data : ScriptableObject
{
    public float cameraHeight = 6;
    public float cameraDistance = 10;
    public float centerOffset = 2;
    [Range(0.1f, 2)]
    public float smoothTime = 0.1f;
    public float smoothRotTime = 3f;
    public float collisionSize = 0.5f;
}
