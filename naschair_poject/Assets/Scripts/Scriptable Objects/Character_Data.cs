using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "RockPool/Data Instace/Character Database", order = 1)]
public class Character_Data : ScriptableObject
{
    public List<CharacterStorage_Data> characters = new List<CharacterStorage_Data>();
}
