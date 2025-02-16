using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public CharacterStatus characterStatus;
    public string charName;
    public float health;
    public float maxHealth;
    public float mana;
    public float maxMana;

    public int level;
    public List<Skill> skills;
    public Type weakness;
    public Type heal;
    public bool isRecruited;
    public bool isMainPlayer;
    public AnimationController animCon;
    public CharacterStatus.Condition condition;
    public CharacterStatus.Buff buff;
    public List<CharacterStatus> party;

    //public static UnitStatus Instance = null;

    public void Init()
    {
        charName = characterStatus.charName;
        health = characterStatus.health;
        maxHealth = characterStatus.maxHealth;
        mana = characterStatus.mana;
        maxMana = characterStatus.maxMana;

        level = characterStatus.level;
        skills = characterStatus.skills;
        weakness = characterStatus.weakness;
        heal = characterStatus.heal;
        isRecruited = characterStatus.isRecruited;
        isMainPlayer = characterStatus.isMainPlayer;
       
        condition = characterStatus.condition;
        buff = characterStatus.buff;
        party = characterStatus.party;
    }

    private void OnEnable()
    {
        SaveLoad.OnSaveStatus += SaveStatus;
        SaveLoad.OnLoadParty += LoadStatus;
    }

    private void LoadStatus(List<CharacterStatus> partyStatusData)
    {
        CharacterStatus status = partyStatusData.Find(x => x.charName == charName);
        characterStatus.health = status.health;
        characterStatus.maxHealth = status.maxHealth;
        characterStatus.mana = status.mana;
        characterStatus.maxMana = status.maxMana;
        characterStatus.condition = status.condition;
        characterStatus.buff = status.buff;
        characterStatus.isRecruited = status.isRecruited;

    }

    private void SaveStatus()
    {
        //SaveGameManager.data.playerParty = party;
        if (characterStatus.isMainPlayer)
        {
            foreach (CharacterStatus status in party)
            {
                CharacterStatus newStatus = new CharacterStatus();
                newStatus.charName = status.charName;
                newStatus.health = status.health;
                newStatus.maxHealth = status.maxHealth;
                newStatus.mana = status.mana;
                newStatus.maxMana = status.maxMana;
                //newStatus.weakness = status.    weakness;
                //newStatus.heal = status.heal;
                newStatus.isRecruited = status.isRecruited;
                newStatus.isMainPlayer = status.isMainPlayer;
                newStatus.condition = status.condition;
                newStatus.buff = status.buff;   
                SaveGameManager.partyStatusData.Add(newStatus);
                //SaveGameManager.data.playerParty.Add(status);
                //SaveGameManager.data.playerParty[i].charName = status.charName;
                //SaveGameManager.data.playerParty[i].health = status.health;
                //SaveGameManager.data.playerParty[i].maxHealth = status.maxHealth;
                //SaveGameManager.data.playerParty[i].mana = status.mana;
                //SaveGameManager.data.playerParty[i].maxMana = status.maxMana;
                //SaveGameManager.data.playerParty[i].level = status.level;
                //SaveGameManager.data.playerParty[i].heal = status.heal;
                //SaveGameManager.data.playerParty[i].weakness = status.weakness;
                //SaveGameManager.data.playerParty[i].isRecruited = status.isRecruited;
                //SaveGameManager.data.playerParty[i].isMainPlayer = status.isMainPlayer;
                //SaveGameManager.data.playerParty[i].condition = status.condition;

            }
        }
    }

    private void OnDestroy()
    {
        SaveLoad.OnSaveStatus -= SaveStatus;
        SaveLoad.OnLoadParty -= LoadStatus;
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
