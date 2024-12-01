using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public CharacterStatus characterStatus;
    public string name;
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;
    public int level;
    public List<Skill> skills;
    public Type weakness;
    public Type heal;
    public bool isRecruited;
    public AnimationController animCon;
    public CharacterStatus.Condition condition; 
    // Start is called before the first frame update
    //void Start()
    //{
    //    name = characterStatus.name;
    //    health = characterStatus.health;
    //    maxHealth = characterStatus.maxHealth;
    //    mana = characterStatus.mana;
    //    maxMana = characterStatus.maxMana;
    //    level = characterStatus.level;
    //    skills = characterStatus.skills;
    //    weakness = characterStatus.weakness;
    //    heal = characterStatus.heal;
    //    isRecruited = characterStatus.isRecruited;
    //    condition = characterStatus.condition;
    //}

    public void Init()
    {
        name = characterStatus.name;
        health = characterStatus.health;
        maxHealth = characterStatus.maxHealth;
        mana = characterStatus.mana;
        maxMana = characterStatus.maxMana;
        level = characterStatus.level;
        skills = characterStatus.skills;
        weakness = characterStatus.weakness;
        heal = characterStatus.heal;
        isRecruited = characterStatus.isRecruited;
       
        condition = characterStatus.condition;
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
