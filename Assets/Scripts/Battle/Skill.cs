using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    FIRE,
    ICE,
    EARTH
}

[CreateAssetMenu(fileName = "SkillStatusData", menuName = "StatusObjects/Skill", order = 1)]
public class Skill : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public float power;
    public float mpCost;
    public bool isUnlocked;
  
    public Type currentType;
    public GameObject effect;
    public GameObject aura;
}
