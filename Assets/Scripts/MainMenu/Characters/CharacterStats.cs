using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public string[] characterName;
    public string[] relation;
    public string[] emotion;
}
