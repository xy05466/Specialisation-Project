using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private CharacterStatus playerStatus;
    public List<SimpleStatusHUD> statusHUD;


    private void OnEnable()
    {

        //statusHUD[0].SetName(playerStatus);
        //statusHUD[0].SimpleSetHP(playerStatus);
        //statusHUD[0].SimpleSetMP(playerStatus);
        //statusHUD[0].SetStatusActive(true);
        //statusHUD[0].SetConditionOverlay(playerStatus.condition);
        //status


        for (int i = 0; i < playerStatus.party.Count; i++)
        {
            statusHUD[i].SetName(playerStatus.party[i]);
            statusHUD[i].SimpleSetHP(playerStatus.party[i]);
            statusHUD[i].SimpleSetMP(playerStatus.party[i]);
            statusHUD[i].SetStatusActive(playerStatus.party[i].isRecruited);
            statusHUD[i].SetConditionOverlay(playerStatus.party[i].condition);
            statusHUD[i].SetBuffOverlay(playerStatus.party[i].buff);

        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }

    public SimpleStatusHUD FindHUD(CharacterStatus charStatus)
    {
        SimpleStatusHUD foundPlayer = statusHUD.FirstOrDefault(i => i.name.text == charStatus.charName);
        return foundPlayer;
    }

    public SimpleStatusHUD FindHUD(UnitStatus charStatus)
    {
        SimpleStatusHUD foundPlayer = statusHUD.FirstOrDefault(i => i.name.text == charStatus.charName);
        return foundPlayer;
    }



    public void SimpleUpdateHP(CharacterStatus charStatus)
    {
        FindHUD(charStatus).SimpleSetHP(charStatus);
    }

    public void UpdateConditionOverlay(CharacterStatus charStatus)
    {
        FindHUD(charStatus).SetConditionOverlay(charStatus.condition);
    }

    public void UpdateBuffOverlay(CharacterStatus charStatus)
    {
        FindHUD(charStatus).SetBuffOverlay(charStatus.buff);
    }

    public void SimpleUpdateMP(CharacterStatus charStatus)
    {
        FindHUD(charStatus).SimpleSetMP(charStatus);
    }

    public void BattleUpdateHP(UnitStatus charStatus)
    {
        FindHUD(charStatus).SimpleSetHP(charStatus);
    }

    public void BattleUpdateMP(UnitStatus charStatus)
    {
        FindHUD(charStatus).BattleSetMP(charStatus);
    }

}
