using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using TMPro;

public class BattleSystemManager : MonoBehaviour
{
    [SerializeField] private Encounters encounters;

    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }
    private List<UnitStatus> enemyStatuses;
    //private List<GameObject> players;
    private List<UnitStatus> playerStatuses;
    private List<string> skillNames;
         
    public List<Transform> enemyBattlePositions;
    public List<Transform> playerBattlePositions;

    //public Transform enemyBattlePosition;
    //public Transform playerBattlePosition;

    public GameObject playerObject;
    //private UnitStatus playerStatus;
    private PlayerParty playerParty;
    //public UnitStatus enemyStatus;
    private List<UnitStatus> partyToAttack;
    private UnitStatus enemyToAttack;
    
    //public StatusHUD playerStatusHUD;
    public List<StatusHUD> playerStatusHUDList;
    //public StatusHUD enemyStatusHUD;
    public List<StatusHUD> enemyStatusHUDList;
    public StatusHUD enemyMPHUD;
    public StatusHUD playerMPHUD;
    public GameObject skillImage;

    public GameObject content;
    public GameObject arrow;
    //private GameObject enemyHUD;
    
    private BattleState battleState;

    private bool hasClicked = true;
    private bool canSelect = false;
    private int index = 1;
    private bool hasSetIndex;

    private int escapeChance = 0;
    private int escapeCount = 0;

    private int encounterLeft;
    private int partyLeft;
    private Skill skillToUse;
    private GameObject skillEffect;
    private int skillMpCost;

    private float maxPlayerMana;
    private float currPlayerMana;
    private float maxEnemyMana;
    private float currEnemyMana;

    private int partyIndex;
    private EnemyAI.Action action;

    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.START;
        StartCoroutine(BeginBattle());
    }

    IEnumerator BeginBattle()
    {
        encounters.randomiseEnemy();
        enemyStatuses = new List<UnitStatus>();
        playerStatuses = new List<UnitStatus>();
        skillNames = new List<string>();

        for (int i = 0; i < encounters.enemyNum; i++)
        {
            enemyStatuses.Add(Instantiate(encounters.enemyToSpawn[i], enemyBattlePositions[i]).GetComponent<UnitStatus>());
            //enemyStatuses.Add(enemies[i].GetComponent<UnitStatus>());
            enemyStatuses[i].Init();
            enemyStatuses[i].gameObject.SetActive(true);
            enemyStatuses[i].animCon.ChangeAnimation(enemyStatuses[i].animCon.Combat, 0f, 0, 0);
            //enemyMPHUD.SetMPHUD(encounters.enemyToSpawn[i].GetComponent<UnitStatus>());
            enemyStatusHUDList[i].SetStatusHUD(enemyStatuses[i]);
            enemyStatusHUDList[i].gameObject.SetActive(true);
            maxEnemyMana += enemyStatuses[i].maxMana;
        }
        currEnemyMana = maxEnemyMana;
        enemyMPHUD.SetMPHUD(maxEnemyMana,currEnemyMana);
        encounterLeft = encounters.enemyNum;
        
        playerParty = playerObject.transform.GetChild(0).gameObject.GetComponent<PlayerParty>();
        partyLeft = playerParty.party.Count;
        int a = 0;
        foreach (CharacterStatus characterStatus in playerParty.party)
        {
            if (characterStatus.isRecruited)
            {
                //players.Add(Instantiate(characterStatus.characterGameObject.transform.GetChild(0).gameObject, playerBattlePositions[a]));
                playerStatuses.Add(Instantiate(characterStatus.characterGameObject.transform.GetChild(0).gameObject, playerBattlePositions[a]).GetComponent<UnitStatus>());
                playerStatuses[a].Init();
                playerStatuses[a].gameObject.SetActive(true);
                playerStatuses[a].animCon.ChangeAnimation(playerStatuses[a].animCon.Combat, 0f, 0, 0);
                //playerMPHUD.SetMPHUD(players[a].GetComponent<UnitStatus>());
                playerStatusHUDList[a].SetStatusHUD(playerStatuses[a]);
                playerStatusHUDList[a].gameObject.SetActive(true);
                maxPlayerMana += playerStatuses[a].maxMana;
                currPlayerMana += playerStatuses[a].mana;
                if (playerStatuses[a].condition == CharacterStatus.Condition.DEAD)
                {
                    playerStatuses[a].animCon.ChangeAnimation(playerStatuses[a].animCon.Die, 0f, 0, 0);
                }
                else
                {
                    if (!hasSetIndex)
                    {
                        partyIndex = a;
                        hasSetIndex = true;
                    }
                }
                a++;
            }
        }
        playerMPHUD.SetMPHUD(maxPlayerMana, currPlayerMana);


        setPlayerSkills(false);
        //int n = 0;
        //foreach (Transform skill in content.transform)
        //{

        //    if (skill.name == playerStatuses[partyIndex].skills[0].skillName)
        //    {
        //        content.transform.GetChild(n).gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        content.transform.GetChild(n).gameObject.SetActive(false);
        //    }
        //    n++;

        //}

        // set the characters stats in HUD displays
        //playerStatusHUD.SetStatusHUD(player.GetComponent<UnitStatus>());
        index = Mathf.RoundToInt(encounters.enemyNum / 2);
        arrow.gameObject.transform.position = new Vector3(enemyStatuses[index].transform.position.x, enemyStatuses[index].transform.position.y + 5f, enemyStatuses[index].transform.position.z);

        // player turn
        battleState = BattleState.PLAYERTURN;
        // let player select his action
        yield return StartCoroutine(PlayerTurn());
    }

    private void setPlayerSkills(bool isPlayerTurn)
    {
        int n = 0;
        int j = 0;
        if (isPlayerTurn)
        {
            foreach (UnitStatus status in playerStatuses)
            {
                j++;
                if (status.condition != CharacterStatus.Condition.DEAD && status.name != playerStatuses[partyIndex].name)
                {
                    partyIndex = playerStatuses.IndexOf(status);
                    break;
                }
            }
            //partyIndex = j;
            //if (partyIndex > playerParty.party.Count - 1)
            //{
            //    partyIndex = 0;
            //}
        }
        else
        {
            for (int a = playerStatuses.Count - 1; a >= 0; a--)
            {
                if (playerStatuses[a].condition != CharacterStatus.Condition.DEAD)
                {
                    partyIndex = a;
                }
            }
        }

        foreach (Skill skill in playerStatuses[partyIndex].skills)
        {
            skillNames.Add(skill.name);
        }

        foreach (Transform skill in content.transform)
        {
           skill.gameObject.SetActive(false);
        }

        foreach (Transform skill in content.transform)
        {
            foreach(string name in skillNames)
            {
                if (skill.name == name)
                {
                    content.transform.GetChild(n).gameObject.SetActive(true);
                }
            }
            n++;
        }
        skillNames.Clear();
    }

    IEnumerator PlayerTurn()
    {
        yield return new WaitForSeconds(1);

        // release the blockade on clicking 
        // so that player can click on 'attack' button    
        hasClicked = false;
    }

    public void SetCanSelect(bool canselect)
    {
        canSelect = canselect;
        
        arrow.gameObject.SetActive(canselect);
    }

    public void SetSkillToUse(Skill skill)
    {
        skillToUse = skill;
    }

    public void SelectTarget()
    {
        if (canSelect)
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                partyToAttack = playerStatuses;
                if (partyToAttack.Count < index)
                {
                    index = partyToAttack.Count - 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                partyToAttack = enemyStatuses;
                if (partyToAttack.Count < index)
                {
                    index = partyToAttack.Count - 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.J) && index >0)
            {
                index--;
                if (enemyStatuses[index].condition == CharacterStatus.Condition.DEAD)
                {
                    if (enemyStatuses.Count > 2)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
                enemyToAttack = enemyStatuses[index];
                arrow.gameObject.transform.position = new Vector3(enemyStatuses[index].transform.position.x, enemyStatuses[index].transform.position.y + 5f, enemyStatuses[index].transform.position.z);
                Debug.Log(enemyToAttack.name);
            }
            if (Input.GetKeyDown(KeyCode.L) && (index + 1) <= encounterLeft - 1)
            {
                index++;
                if (enemyStatuses[index].condition == CharacterStatus.Condition.DEAD)
                {
                    if (enemyStatuses.Count > 2)
                    {
                        index = 2;
                    }
                    else
                    {
                        index--;
                    }
                }
                enemyToAttack = enemyStatuses[index];
                arrow.gameObject.transform.position = new Vector3(enemyStatuses[index].transform.position.x, enemyStatuses[index].transform.position.y + 5f, enemyStatuses[index].transform.position.z);
                Debug.Log(enemyToAttack.name);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                enemyToAttack = enemyStatuses[index];
                Debug.Log("attack" + enemyToAttack.name);
                OnAttackButtonPress();
            }
        }
       
    }

    public void OnAttackButtonPress()
    {
        // don't allow player to click on 'attack'
        // button if it's not his turn!
        if (battleState != BattleState.PLAYERTURN && playerStatuses[partyIndex].mana <= 0)
            return;

        // allow only a single action per turn
        if (!hasClicked)
        {
            content.transform.parent.parent.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
            StartCoroutine(PlayerAttack(enemyToAttack));

            // block user from repeatedly 
            // pressing attack button  
            hasClicked = true;
        }
    }
   

    private bool CheckSkillDmg(StatusHUD hud,UnitStatus target)
    {
        if (skillToUse.currentType == target.weakness)
        {
            StartCoroutine(hud.SetHP(target, skillToUse.power * 1.5f, false));
            return true;
        }
        if (skillToUse.currentType == target.heal)
        {
            if (target.health + skillToUse.power < target.maxHealth)
            {
                StartCoroutine(hud.SetHP(target, -skillToUse.power,true));
                
            }
            else
            {
                StartCoroutine(hud.SetHP(target, target.maxHealth - target.health, true));
            }
            return false;
        }
        else
        {
            StartCoroutine(hud.SetHP(target, skillToUse.power, false));
            return true;
        }
    }

    IEnumerator PlayerAttack(UnitStatus character)
    {
        // trigger the execution of attack animation
        // in 'BattlePresence' animator
        skillImage.SetActive(true);
        skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.name;
        //if (skillToUse.currentType == enemyStatuses[index].weakness)
        //{
        //    skillMpCost = Mathf.RoundToInt(skillToUse.mpCost * 0.5f);
        //}
        //else if (skillToUse.currentType == enemyStatuses[index].heal)
        //{
        //    skillMpCost = Mathf.RoundToInt(skillToUse.mpCost * 1.5f);
        //}
        StartCoroutine(playerMPHUD.SetMP(skillToUse.mpCost, false));
        StartCoroutine(playerStatuses[partyIndex].animCon.AttackAnimation(skillToUse, enemyStatuses[index].transform.position));

        yield return StartCoroutine(enemyStatuses[index].animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
        CheckSkillDmg(enemyStatusHUDList[index], character);

        skillImage.SetActive(false);

        for (int i = 0; i < enemyStatuses.Count; i++)
        {
            if (enemyStatuses[i].health <= 0)
            {
                enemyStatuses[i].animCon.ChangeAnimation(enemyStatuses[i].animCon.Die, 0.1f, 0, 0);
                enemyStatuses[i].condition = CharacterStatus.Condition.DEAD;
                encounterLeft--;
                enemyStatuses.Remove(enemyStatuses[i]);

                //index = 0;
                if (encounterLeft == 0)
                {
                    battleState = BattleState.WIN;
                    yield return StartCoroutine(EndBattle());
                }
            }
        }
        int n = -1;
        foreach (UnitStatus status in enemyStatuses)
        {
            if (status.condition != CharacterStatus.Condition.DEAD)
            {
                arrow.transform.position = new Vector3(status.transform.position.x, status.transform.position.y + 5f, status.transform.position.z);
            }
            n++;
        }
        index = n;
        if (playerMPHUD.GetMP() <= 0)
        {
            // if the enemy health is still
            // above 0 when the turn finishes
            // it's enemy's turn!   
            yield return StartCoroutine(playerMPHUD.SetMP(-(maxPlayerMana - playerMPHUD.GetMP()),true));
            //partyIndex = 0;
            setPlayerSkills(true);
            //yield return new WaitForSeconds(1);
            battleState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyTurn());
        }

        else
        {
            if (playerParty.party.Count > 1)
            {
                setPlayerSkills(true);
            }
            StartCoroutine(PlayerTurn());
        }
    }

    public void OnTryEscape()
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        if (!hasClicked)
        {
            StartCoroutine(TryEscape());

            // block user from repeatedly 
            // pressing attack button  
            hasClicked = true;
        }
    }

    IEnumerator TryEscape()
    {
        escapeChance = Random.Range(0, 10);
        if (escapeChance < 7 + (escapeCount))
        {
            Debug.Log("escaped");
            yield return new WaitForSeconds(1);
            SceneLoader.instance.LoadLevel("SampleScene");
        }
        else
        {
            Debug.Log("failed escape");
            battleState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyTurn());
            escapeCount++;
        }
    }

    IEnumerator EnemyTurn()
    {
        // as before, decrease playerhealth by a fixed
        // amount of 10. You probably want to have some
        // more complex logic here.
        int i = 0;
        while (enemyMPHUD.GetMP() > 0)
        {
            foreach (UnitStatus enemy in enemyStatuses)
            {
                if (enemy.condition == CharacterStatus.Condition.DEAD)
                {
                    continue;
                }
                int target = Random.Range(0, playerStatuses.Count);
                while (playerStatuses[target].condition == CharacterStatus.Condition.DEAD)
                {
                    target = Random.Range(0, playerStatuses.Count);
                }
                action = enemy.GetComponent<EnemyAI>().Decide(playerStatuses[target], enemyStatuses, enemy);
                if (action == EnemyAI.Action.ATTACK)
                {
                    int skill;
                    if (enemy.GetComponent<UnitStatus>().skills.Count > 1)
                    {
                        skill = Random.Range(0, enemy.GetComponent<UnitStatus>().skills.Count);
                    }
                    else
                    {
                        skill = 0;
                    }
                    SetSkillToUse(enemy.GetComponent<UnitStatus>().skills[skill]);
                    //if (skillToUse.currentType == playerStatuses[target].weakness)
                    //{
                    //    skillMpCost = Mathf.RoundToInt(skillToUse.mpCost * 0.5f);
                    //}
                    //else if (skillToUse.currentType == playerStatuses[target].heal)
                    //{
                    //    skillMpCost = Mathf.RoundToInt(skillToUse.mpCost * 1.5f);
                    //}
                    if (enemyMPHUD.GetMP() - skillToUse.mpCost < 0)
                    {
                        break;
                    }
                    skillImage.SetActive(true);
                    skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.name;

                    StartCoroutine(enemyMPHUD.SetMP(skillToUse.mpCost, false));
                    StartCoroutine(enemy.GetComponent<UnitStatus>().animCon.AttackAnimation(skillToUse, playerStatuses[target].transform.position));
                    
                    yield return StartCoroutine(playerStatuses[target].animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration / 2));
                    CheckSkillDmg(playerStatusHUDList[target], playerStatuses[target]);
                    skillImage.SetActive(false);
                }
                else
                {
                    foreach (Skill skill in enemy.GetComponent<UnitStatus>().skills)
                    {
                        if (skill.currentType == enemy.GetComponent<UnitStatus>().heal)
                        {
                            SetSkillToUse(skill);
                        }
                    }

                    skillImage.SetActive(true);
                    skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.name;
                    if (enemyMPHUD.GetMP() - skillToUse.mpCost <= 0)
                    {
                        break;
                    }
                    StartCoroutine(enemyMPHUD.SetMP(skillToUse.mpCost, false));
                    StartCoroutine(enemy.GetComponent<UnitStatus>().animCon.AttackAnimation(skillToUse, enemy.GetComponent<EnemyAI>().Defend().gameObject.transform.position));

                    CheckSkillDmg(enemyStatusHUDList[i], enemy.GetComponent<EnemyAI>().Defend());
                    
                    yield return StartCoroutine(enemy.GetComponent<EnemyAI>().Defend().animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
                    
                    skillImage.SetActive(false);
                }
                foreach (UnitStatus status in playerStatuses)
                {
                    if (status.health <= 0 && status.condition != CharacterStatus.Condition.DEAD)
                    {
                        status.animCon.ChangeAnimation(status.animCon.Die, 0.1f, 0, 0);
                        status.condition = CharacterStatus.Condition.DEAD;
                        partyLeft--;
                        for (int a = playerStatuses.Count - 1; a >= 0; a--)
                        {
                            if (playerStatuses[a].condition != CharacterStatus.Condition.DEAD)
                            {
                                partyIndex = a;
                            }
                        }
                        setPlayerSkills(false);
                        if (partyLeft == 0)
                        {
                            battleState = BattleState.LOST;
                            yield return StartCoroutine(EndBattle());
                        }
                    }
                }
            
                
                i++;
                if (i > encounterLeft - 1)
                {
                    i = 0;
                }
                //enemy.GetComponent<UnitStatus>().animCon.ChangeAnimation(enemyStatuses[index].GetComponent<UnitStatus>().animCon.Attack, 0.1f, 0, 0);
                //SetSkillToUse(enemy.GetComponent<UnitStatus>().skills[0]);
                //enemyMPHUD.SetMP(skillToUse.mpCost);
                ////Instantiate(skillToUse.effect, players[partyIndex].transform.position, Quaternion.identity);
                //PlaySkillAnim(players[partyIndex].transform.position);

                //yield return new WaitForSeconds(1);

                //CheckSkillDmg(playerStatusHUDList[partyIndex], players[partyIndex].GetComponent<UnitStatus>());
                //players[partyIndex].GetComponent<UnitStatus>().animCon.ChangeAnimation(players[partyIndex].GetComponent<UnitStatus>().animCon.Hurt, 0.1f, 0, 0);
                //yield return new WaitForSeconds(1);
                //players[partyIndex].GetComponent<UnitStatus>().animCon.ChangeAnimation(players[partyIndex].GetComponent<UnitStatus>().animCon.Combat, 0.1f, 0, 0);


            }
            if (enemyMPHUD.GetMP() - skillToUse.mpCost < 0)
            {
                break;
            }
        }
        yield return StartCoroutine(enemyMPHUD.SetMP(-(maxEnemyMana - enemyMPHUD.GetMP()), true));
        //yield return new WaitForSeconds(1);
        battleState = BattleState.PLAYERTURN;
        yield return StartCoroutine(PlayerTurn());
        // if the player health is still
        // above 0 when the turn finishes
        // it's our turn again!


        //for (int i = 0; i < encounters.enemyNum; i++)
        //{
        //    playerStatusHUD.SetHP(playerStatus, 10);
        //    player.GetComponent<UnitStatus>().animCon.ChangeAnimation(player.GetComponent<UnitStatus>().animCon.Hurt, 0.1f, 0, 0);
        //    yield return new WaitForSeconds(1);
        //    if (playerStatus.health <= 0)
        //    {
        //        // if the player health drops to 0 
        //        // we have lost the battle...
        //        battleState = BattleState.LOST;
        //        yield return StartCoroutine(EndBattle());
        //    }
        //    else
        //    {
        //        // if the player health is still
        //        // above 0 when the turn finishes
        //        // it's our turn again!
        //        battleState = BattleState.PLAYERTURN;
        //        yield return StartCoroutine(PlayerTurn());
        //    }
        //}

        //playerStatusHUD.SetHP(playerStatus, 10);

        // play attack animation by triggering
        // it inside the enemy animator
        //enemy.GetComponent<Animator>().SetTrigger("Attack");

        //yield return new WaitForSeconds(1);


    }
    IEnumerator EndBattle()
    {
        // check if we won
        if (battleState == BattleState.WIN)
        {
            // you may wish to display some kind
            // of message or play a victory fanfare
            // here

            yield return new WaitForSeconds(1);

            SceneLoader.instance.LoadLevel("SampleScene");
        }
        // otherwise check if we lost
        // You probably want to display some kind of
        // 'Game Over' screen to communicate to the 
        // player that the game is lost
        else if (battleState == BattleState.LOST)
        {
            // you may wish to display some kind
            // of message or play a sad tune here!
            yield return new WaitForSeconds(1);
            SceneLoader.instance.LoadLevel("SampleScene");
        }
    }
    // Update is called once per frame
    void Update()
    {
        SelectTarget();
    }
}
