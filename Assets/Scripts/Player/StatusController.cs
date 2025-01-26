using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public bool SetPartyHealth()
    //{
    //    if (this.playerStatus.health == this.playerStatus.maxHealth &&
    //        this.playerStatus.party.FirstOrDefault(i => i.health != i.maxHealth) == null)
    //    {
    //        return false;
    //    }
    //    this.playerStatus.health = this.playerStatus.maxHealth;
    //    foreach (CharacterStatus status in this.playerStatus.party)
    //    {
    //        if (status.health > 0 && status.health < status.maxHealth)
    //        {
    //            status.health = status.maxHealth;
    //            statusHUD.UpdateHP(status);
    //        }
    //    }
    //    return true;
    //}

    //public bool SetPartyCondition()
    //{
    //    if (this.playerStatus.condition == CharacterStatus.Condition.NORMAL &&
    //       this.playerStatus.party.FirstOrDefault(i => i.condition == CharacterStatus.Condition.DEAD) == null)
    //    {
    //        return false;
    //    }
    //    this.playerStatus.health = this.playerStatus.maxHealth;
    //    this.playerStatus.condition = CharacterStatus.Condition.NORMAL;
    //    statusHUD.UpdateOverlay(this.playerStatus);
    //    statusHUD.UpdateHP(this.playerStatus);

    //    foreach (CharacterStatus status in this.playerStatus.party)
    //    {
    //        if (status.condition == CharacterStatus.Condition.DEAD)
    //        {
    //            status.condition = CharacterStatus.Condition.NORMAL;
    //            statusHUD.UpdateOverlay(status);
    //        }
    //        if (status.health > 0 && status.health < status.maxHealth)
    //        {
    //            status.health = status.maxHealth;
    //            statusHUD.UpdateHP(status);
    //        }
    //    }
    //    return true;
    //}
}
