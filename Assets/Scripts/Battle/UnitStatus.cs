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
    public float totalMana;

    public int level;
    public List<Skill> skills;
    public Type weakness;
    public Type heal;
    public bool isRecruited;
    public AnimationController animCon;
    public CharacterStatus.Condition condition;
    public List<CharacterStatus> party;

    //public static UnitStatus Instance = null;

    public void Init()
    {
        name = characterStatus.name;
        health = characterStatus.health;
        maxHealth = characterStatus.maxHealth;
        mana = characterStatus.mana;
        maxMana = characterStatus.maxMana;

        foreach (CharacterStatus status in party)
        {
            totalMana += status.mana;
        }

        level = characterStatus.level;
        skills = characterStatus.skills;
        weakness = characterStatus.weakness;
        heal = characterStatus.heal;
        isRecruited = characterStatus.isRecruited;
       
        condition = characterStatus.condition;
        party = characterStatus.party;
    }
    private void OnDestroy()
    {
        UnityEngine.Debug.Log("is destroyed");
    }
    //private void Awake()
    //{
    //    if (Instance != null)
    //    {
    //        Destroy(gameObject.transform.parent.gameObject);
    //        return;
    //    }
    //    Instance = this;
    //    if (transform.parent == null)
    //        DontDestroyOnLoad(this);
    //}


}
