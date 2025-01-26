using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        CharacterStatus foundPlayer = status.party.FirstOrDefault(i => i.name == name);
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
            statusHUD.UpdateOverlay(playerRevive);
            statusHUD.SimpleUpdateHP(playerRevive);

            //foreach (CharacterStatus status in status.party)
            //{
            //    if (status.condition == CharacterStatus.Condition.DEAD)
            //    {
            //        status.condition = CharacterStatus.Condition.NORMAL;
            //        statusHUD.UpdateOverlay(status);
            //    }
            //    if (status.health > 0 && status.health < status.maxHealth)
            //    {
            //        status.health = status.maxHealth;
            //        statusHUD.UpdateHP(status);
            //    }
            //}
            return true;
        }
        else
        {
            return false;
        }
       
    }

    //public void SetMana()
    //{
    //    if 
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
