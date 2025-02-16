using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthStatusData", menuName = "StatusObjects/Health", order = 1)]
[System.Serializable]
public class CharacterStatus : ScriptableObject
{
    public string charName = "name";
    public float[] position = new float[2];
    public GameObject characterGameObject;
    public int level = 1;
    public float maxHealth = 100;
    public float maxMana = 100;
    public float health = 100;
    public float mana = 100;
    public List<Skill> skills;
    public Type weakness;
    public Type heal;
    public bool isRecruited;
    public bool isMainPlayer;
    public List<CharacterStatus> party;

    public enum Condition
    {
        NORMAL,
        DEAD
    }

    public Condition condition;

    public enum Buff
    {
        NORMAL,
        INCMP,
        INCHP
    }

    public Buff buff;
}
