using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "RockPool/Data Instace/Map Data", order = 1)]
public class Map_Data : ScriptableObject
{
    public string displayName = "";
    public string sceneName = "";
    public Sprite displayImage;
}
