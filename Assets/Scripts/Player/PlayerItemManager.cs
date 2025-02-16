using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerItemManager : MonoBehaviour
{
    public static PlayerItemManager Instance = null;
    public CharacterStatus status;
    public HUDManager statusHUD;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }
        Instance = this;
        if (transform.parent == null)
            DontDestroyOnLoad(this);
    }

    private CharacterStatus SaveUnitStatus(string name)
    {
        CharacterStatus foundPlayer = status.party.FirstOrDefault(i => i.charName == name);
        return foundPlayer;
    }

    public bool SetPartyHealth()
    {
        if (status.health == status.maxHealth &&
            status.party.FirstOrDefault(i => i.health != i.maxHealth) == null)
        {
            return false;
        }
        status.health = status.maxHealth;
        foreach (CharacterStatus status in status.party)
        {
            if (status.health > 0 && status.health < status.maxHealth)
            {
                status.health = status.maxHealth;
                statusHUD.SimpleUpdateHP(status);
            }
        }
        return true;
    }

    public bool SetPartyCondition(string name)
    {
        //if (status.condition == CharacterStatus.Condition.NORMAL &&
        //   status.party.FirstOrDefault(i => i.condition == CharacterStatus.Condition.DEAD) == null)
        //{
        //    return false;
        //}
        CharacterStatus playerRevive = SaveUnitStatus(name);
        if (playerRevive != null)
        {
            playerRevive.health = playerRevive.maxHealth * 0.5f;
            playerRevive.condition = CharacterStatus.Condition.NORMAL;
            statusHUD.UpdateConditionOverlay(playerRevive);
            statusHUD.SimpleUpdateHP(playerRevive);
            return true;
        }
        else
        {
            return false;
        }
       
    }

    public bool SetMaxMP(string name)
    {
        Debug.Log(name);
        CharacterStatus playerMP = SaveUnitStatus(name);
        if (playerMP != null)
        {
            if (playerMP.buff == CharacterStatus.Buff.INCHP)
            {
                playerMP.health = playerMP.health / 1.5f;
                playerMP.maxHealth = playerMP.maxHealth / 1.5f;
                statusHUD.SimpleUpdateHP(playerMP);
            }
            if (playerMP.buff != CharacterStatus.Buff.INCMP)
            {
                playerMP.buff = CharacterStatus.Buff.INCMP;
                playerMP.mana = Mathf.RoundToInt(playerMP.mana * 1.5f);
                playerMP.maxMana = Mathf.RoundToInt(playerMP.maxMana * 1.5f);
                statusHUD.UpdateBuffOverlay(playerMP);
                statusHUD.SimpleUpdateMP(playerMP);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool SetMaxHP(string name)
    {
        Debug.Log(name);
        CharacterStatus playerHP = SaveUnitStatus(name);
        if (playerHP != null)
        {
            if (playerHP.buff == CharacterStatus.Buff.INCMP)
            {
                playerHP.mana = playerHP.mana / 1.5f;
                playerHP.maxMana = playerHP.maxMana / 1.5f;
                statusHUD.SimpleUpdateMP(playerHP);
            }
            if (playerHP.buff != CharacterStatus.Buff.INCHP)
            {
                playerHP.buff = CharacterStatus.Buff.INCHP;
                playerHP.health = Mathf.RoundToInt(playerHP.health * 1.5f);
                playerHP.maxHealth = Mathf.RoundToInt(playerHP.maxHealth * 1.5f);
                statusHUD.UpdateBuffOverlay(playerHP);
                statusHUD.SimpleUpdateHP(playerHP);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
