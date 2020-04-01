using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character : ScriptableObject
{
    public bool isMale;
    public string characterName;
    public string characterRelation;
    public string characterEmotion;
    public bool hasAlibi;
}
