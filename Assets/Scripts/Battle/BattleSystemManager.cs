using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using Eflatun.SceneReference;
using static CharacterStatus;

public class BattleSystemManager : MonoBehaviour
{
    [SerializeField] private Encounters encounters;
    [SerializeField] private SceneReference winScene;
    [SerializeField] private SceneReference loseScene;

    public GameObject commandPanel;
    public GameObject victory;
    public GameObject defeat;
    public GameObject playerTurn;
    public GameObject enemyTurn;
    public GameObject escapeSuccess;
    public GameObject escapeFail;
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST,ESCAPE }
    private List<UnitStatus> enemyStatuses;
    //private List<GameObject> players;
    private List<UnitStatus> playerStatuses;
    private List<string> skillNames;
         
    public List<Transform> enemyBattlePositions;
    public List<Transform> playerBattlePositions;

    //public Transform enemyBattlePosition;
    //public Transform playerBattlePosition;

    private GameObject player;
    private CharacterStatus playerObject;

    //private UnitStatus playerStatus;
    private List<CharacterStatus> playerParty;
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
    public Button escape;
    //private GameObject enemyHUD;
    
    private BattleState battleState;
    private IEnumerator enemyCoroutine;

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
    private int mpDebt;
    private bool inDebt;
    private List<StatusHUD> hudToChangeList;

    private float maxPlayerMana;
    private float currPlayerMana;
    private float maxEnemyMana;
    private float currEnemyMana;

    private int partyIndex;
    private EnemyAI.Action action;

    private SaveGameManager saveGameManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.gameObject.name +this.gameObject.GetInstanceID() + " start");
        saveGameManager = GameObject.FindWithTag("SaveGame").GetComponent<SaveGameManager>();
        battleState = BattleState.START;
        if (encounters.isBoss)
        {
            escape.interactable = false;
        }
        StartCoroutine(BeginBattle());
    }

    void SetUpPlayer()
    {
        player = GameObject.FindWithTag("Player");
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.transform.GetChild(1).gameObject.SetActive(true);
        
        //player.transform.position = playerBattlePositions[0].position;
        //player.transform.rotation = playerBattlePositions[0].rotation;

        playerObject = player.transform.GetChild(1).GetComponent<UnitStatus>().characterStatus;

        //playerStatuses.Add(player.transform.GetChild(0).GetComponent<UnitStatus>());
        //playerStatuses[0].animCon.ChangeAnimation(playerStatuses[0].animCon.Combat, 0f, 0, 0);
        //playerStatusHUDList[0].SetStatusHUD(playerStatuses[0]);
        //playerStatusHUDList[0].gameObject.SetActive(true);
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

        SetUpPlayer();

        playerParty = playerObject.party;
        partyLeft = playerParty.Count;
        int a = 0;
        foreach (CharacterStatus characterStatus in playerParty)
        {
            if (!characterStatus.isMainPlayer)
            {
                playerStatuses.Add(Instantiate(characterStatus.characterGameObject.transform.GetChild(0).gameObject, playerBattlePositions[a]).GetComponent<UnitStatus>());
            }
            else
            {
                playerStatuses.Add(player.transform.GetChild(1).GetComponent<UnitStatus>());
                player.transform.SetParent(playerBattlePositions[a]);
                player.transform.localPosition = Vector3.zero;
                player.transform.rotation = playerBattlePositions[a].rotation;
                player.transform.localScale = Vector3.one;
            }
                playerStatuses[a].Init();
                playerStatuses[a].gameObject.SetActive(true);
                playerStatuses[a].animCon.ChangeAnimation(playerStatuses[a].animCon.Combat, 0f, 0, 0);
                playerStatusHUDList[a].SetStatusHUD(playerStatuses[a]);
                playerStatusHUDList[a].gameObject.SetActive(true);
                maxPlayerMana += playerStatuses[a].maxMana;
                currPlayerMana += playerStatuses[a].mana;
                if (playerStatuses[a].condition == CharacterStatus.Condition.DEAD)
                {
                    playerStatuses[a].animCon.ChangeAnimation(playerStatuses[a].animCon.Die, 0f, 0, 0);
                    playerStatusHUDList[a].SetDeadStatus(true);
                    currPlayerMana -= playerStatuses[a].mana;
                    partyLeft--;
                }
                else
                {
                    if (!hasSetIndex)
                    {
                        partyIndex = a;
                        hasSetIndex = true;
                    }
                }
                playerStatusHUDList[a].SetBuffStatus(playerStatuses[a].buff);
                a++;
            
        }
        playerMPHUD.SetMPHUD(maxPlayerMana, currPlayerMana);
        setPlayerSkills(false);

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
        if (isPlayerTurn)
        {
            partyIndex = (partyIndex + 1) % playerStatuses.Count;
            while (playerStatuses[partyIndex].condition == CharacterStatus.Condition.DEAD)
            {
                partyIndex = (partyIndex + 1) % playerStatuses.Count;
            }
            //foreach (UnitStatus status in playerStatuses)
            //{
            //    j++;
            //    if (status.condition != CharacterStatus.Condition.DEAD && status.charName != playerStatuses[partyIndex].charName)
            //    {
            //        partyIndex = playerStatuses.IndexOf(status);
            //        break;
            //    }
            //}
        }
        else
        {
            partyIndex = (partyIndex) % playerStatuses.Count;
            while (playerStatuses[partyIndex].condition == CharacterStatus.Condition.DEAD)
            {
                partyIndex = (partyIndex + 1) % playerStatuses.Count;
            }
            //for (int a = playerStatuses.Count - 1; a >= 0; a--)
            //{
            //    if (playerStatuses[a].condition != CharacterStatus.Condition.DEAD)
            //    {
            //        partyIndex = a;
            //    }
            //}
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
        playerTurn.SetActive(true);
        yield return new WaitForSeconds(1);
        playerTurn.SetActive(false);
        // release the blockade on clicking 
        // so that player can click on 'attack' button    
        commandPanel.SetActive(true);
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
            if (hudToChangeList == null)
            {
                hudToChangeList = enemyStatusHUDList;
            }
            if (partyToAttack == null)
            {
                partyToAttack = enemyStatuses;
            }
            
            if (Input.GetKeyDown(KeyCode.U))
            {
                index = playerStatuses.FindLastIndex(x => x.condition != CharacterStatus.Condition.DEAD);
                partyToAttack = playerStatuses;
                arrow.gameObject.transform.position = new Vector3(playerStatuses[index].transform.position.x, playerStatuses[index].transform.position.y + 5f, playerStatuses[index].transform.position.z);

                hudToChangeList = playerStatusHUDList;
                //Debug.Log(enemyToAttack.name);
                
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                index = enemyStatuses.FindLastIndex(x => x.condition != CharacterStatus.Condition.DEAD);
                partyToAttack = enemyStatuses;
                arrow.gameObject.transform.position = new Vector3(enemyStatuses[index].transform.position.x, enemyStatuses[index].transform.position.y + 5f, enemyStatuses[index].transform.position.z);
                hudToChangeList = enemyStatusHUDList;
                //Debug.Log(enemyToAttack.name);
                
            }
            if (Input.GetKeyDown(KeyCode.J) && index >-1)
            {
                index--;
                if (index < 0)
                {
                    index = 0;
                }
                if (partyToAttack[index].condition == CharacterStatus.Condition.DEAD)
                {
                    if (partyToAttack.Count > 2)
                    {
                        index = 0;
                    }
                    else
                    {
                        index++;
                    }
                }
                Debug.Log("index: " + index);
                enemyToAttack = partyToAttack[index];
                arrow.gameObject.transform.position = new Vector3(enemyToAttack.transform.position.x, enemyToAttack.transform.position.y + 5f, enemyToAttack.transform.position.z);
                Debug.Log(enemyToAttack.charName);
            }
            if (Input.GetKeyDown(KeyCode.L) && (index + 1) <= partyToAttack.Count - 1)
            {
                index++;
                if (index > partyToAttack.Count - 1)
                {
                    index = partyToAttack.Count - 1;
                }
                if (partyToAttack[index].condition == CharacterStatus.Condition.DEAD)
                {
                    if (partyToAttack.Count > 2)
                    {
                        index = 2;
                    }
                    else
                    {
                        index--;
                    }
                }
                Debug.Log("index: " + index);
                enemyToAttack = partyToAttack[index];
                arrow.gameObject.transform.position = new Vector3(enemyToAttack.transform.position.x, enemyToAttack.transform.position.y + 5f, enemyToAttack.transform.position.z);
                Debug.Log(enemyToAttack.charName);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                enemyToAttack = partyToAttack[index];
                Debug.Log("attack" + enemyToAttack.charName);
                OnAttackButtonPress();
                //hudToChangeList = enemyStatusHUDList;
                //partyToAttack = enemyStatuses;
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
                StartCoroutine(hud.SetHP(target, skillToUse.power, true));
                
            }
            else
            {
                StartCoroutine(hud.SetHP(target, target.maxHealth, true));
            }
            return false;
        }
        else
        {
            StartCoroutine(hud.SetHP(target, skillToUse.power,false));
            return true;
        }
    }

    private float CheckMpCost(StatusHUD hud, UnitStatus target)
    {
        //if skill type matches target's weakness
        if (skillToUse.currentType == target.weakness)
        {
            skillMpCost = Mathf.RoundToInt(skillToUse.mpCost * 0.5f);
        }
        //if skill type matches target's healing 
        else if (skillToUse.currentType == target.heal)
        {
            skillMpCost = Mathf.RoundToInt(skillToUse.mpCost * 1.5f);
        }
        //if skill type is neutral
        else
        {
            skillMpCost = Mathf.RoundToInt(skillToUse.mpCost);
        }
        //check if player has spent more mp than remaining
        if (hud.GetMP() - skillMpCost < 0)
        {
            mpDebt = Mathf.FloorToInt((Mathf.Abs(hud.GetMP() - skillMpCost) * 1.5f));
            inDebt = true;
        }
        else
        {
            mpDebt = 0;
            inDebt = false;
        }
        return mpDebt;
    }

    IEnumerator PlayerAttack(UnitStatus target)
    {
        // trigger the execution of attack animation
        // in 'BattlePresence' animator
        skillImage.SetActive(true);
        skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.skillName;

        CheckMpCost(playerMPHUD, target);
        StartCoroutine(playerMPHUD.SetMP((skillMpCost), false));
        StartCoroutine(playerStatuses[partyIndex].animCon.AttackAnimation(skillToUse, target.transform.position));

        yield return StartCoroutine(target.animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
        CheckSkillDmg(hudToChangeList[index], target);

        skillImage.SetActive(false);

        UnitStatus deadEnemy = enemyStatuses.FindLast(x => x.health <= 0 && x.condition != CharacterStatus.Condition.DEAD);
        int deadEnemyIndex = enemyStatuses.FindLastIndex(x => x.health <= 0 && x.condition != CharacterStatus.Condition.DEAD);
        if (deadEnemy != null)
        {
            deadEnemy.animCon.ChangeAnimation(deadEnemy.animCon.Die, 0.1f, 0, 0);
            deadEnemy.condition = CharacterStatus.Condition.DEAD;
            encounterLeft--;
            setPlayerSkills(false);
            enemyStatusHUDList[deadEnemyIndex].SetDeadStatus(true);
            
            if (encounterLeft == 0)
            {
                battleState = BattleState.WIN;
                yield return StartCoroutine(EndBattle());
            }
        }

        UnitStatus deadPlayer = playerStatuses.FindLast(x => x.health <= 0 && x.condition != CharacterStatus.Condition.DEAD);
        int deadPlayerIndex = playerStatuses.FindLastIndex(x => x.health <= 0 && x.condition != CharacterStatus.Condition.DEAD);
        if (deadPlayer != null)
        {
            deadPlayer.animCon.ChangeAnimation(deadPlayer.animCon.Die, 0.1f, 0, 0);
            deadPlayer.condition = CharacterStatus.Condition.DEAD;
            partyLeft--;
           
            playerStatusHUDList[deadPlayerIndex].SetDeadStatus(true);
            if (partyLeft == 0)
            {
                battleState = BattleState.LOST;
                yield return StartCoroutine(EndBattle());
            }
        }

        index = enemyStatuses.FindLastIndex(x => x.condition != CharacterStatus.Condition.DEAD);
        Vector3 newArrowPos = enemyStatuses.FindLast(x => x.condition != CharacterStatus.Condition.DEAD).transform.position;
        arrow.transform.position = new Vector3(newArrowPos.x, newArrowPos.y + 5f, newArrowPos.z) ;
        partyToAttack = null;
        hudToChangeList = null;

        Debug.Log("index: " + index);
        if (inDebt || (playerMPHUD.GetMP() <= 0 && !inDebt))
        {
            // if the enemy health is still
            // above 0 when the turn finishes
            // it's enemy's turn!   
            StartEndTurn(true);
        }

        else
        {
            if (playerParty.Count > 1)
            {
                setPlayerSkills(true);
            }
            StartCoroutine(PlayerTurn());
        }
    }

    public void StartEndTurn(bool isPlayerTurn)
    {
        StartCoroutine(EndTurn(isPlayerTurn));
    }

    private IEnumerator EndTurn(bool isPlayerTurn)
    {
        int deadMP = 0;
        if (isPlayerTurn)
        {
            commandPanel.SetActive(false);
            List<UnitStatus> deadPlayers = playerStatuses.FindAll(x => x.condition == CharacterStatus.Condition.DEAD);
            foreach (UnitStatus player in deadPlayers)
            {
                deadMP += (int)player.maxMana;
            }
            yield return StartCoroutine(playerMPHUD.SetMP(Mathf.Abs(maxPlayerMana - playerMPHUD.GetMP()) - mpDebt - deadMP, true));
            inDebt = false;
            mpDebt = 0;
            partyIndex = 0;
            setPlayerSkills(false);
            battleState = BattleState.ENEMYTURN;
            enemyTurn.SetActive(true);
            yield return new WaitForSeconds(1);
            enemyTurn.SetActive(false) ;
            enemyCoroutine = EnemyTurn();
            yield return StartCoroutine(enemyCoroutine);
        }
        else
        {
            
            List<UnitStatus> deadEnemies = enemyStatuses.FindAll(x => x.condition == CharacterStatus.Condition.DEAD);
            foreach (UnitStatus enemy in deadEnemies)
            {
                deadMP += (int)enemy.maxMana;
            }
            yield return StartCoroutine(enemyMPHUD.SetMP(Mathf.Abs(maxEnemyMana - enemyMPHUD.GetMP()) - mpDebt - deadMP, true));
            inDebt = false;
            mpDebt = 0;
            setPlayerSkills(false);
            //yield return new WaitForSeconds(1);
            battleState = BattleState.PLAYERTURN;
            yield return StartCoroutine(PlayerTurn());
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

    private void ResetBuffs()
    {
        foreach (UnitStatus playerStatus in playerStatuses)
        {
            if (playerStatus.buff == CharacterStatus.Buff.INCHP)
            {
                playerStatus.characterStatus.health  = playerStatus.health / 1.5f;
                playerStatus.characterStatus.maxHealth = playerStatus.maxHealth / 1.5f;
                playerStatus.buff = CharacterStatus.Buff.NORMAL;
            }
            else if (playerStatus.buff == CharacterStatus.Buff.INCMP)
            {
                playerStatus.characterStatus.health = playerStatus.mana / 1.5f;
                playerStatus.characterStatus.maxHealth = playerStatus.maxMana / 1.5f;
                playerStatus.buff = CharacterStatus.Buff.NORMAL;
            }
            
        }
    }

    IEnumerator TryEscape()
    {
        //escapeChance = Random.Range(0, 10);
        //Debug.Log("escaped");
        //ResetBuffs();
        //battleState = BattleState.ESCAPE;
        //SavePlayerStatus();
        //escapeSuccess.SetActive(true);
        //yield return new WaitForSeconds(1.5f);
        //escapeSuccess.SetActive(false);
        //player.transform.GetChild(0).gameObject.SetActive(true);
        //player.transform.GetChild(1).gameObject.SetActive(false);
        //Vector3 spawnPos = SaveGameManager.StringToVector3(PlayerPrefs.GetString("LastPos"));
        //saveGameManager.StartLoadScene(spawnPos, winScene.Name);
        //Debug.Log("escaped");
        //SavePlayerStatus();
        yield return new WaitForSeconds(1);
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.GetChild(1).gameObject.SetActive(false);
        SceneLoader.instance.LoadLevel(winScene.Name);
        if (escapeChance < 7 + (escapeCount))
        {
            Debug.Log("escaped");
            ResetBuffs();
            battleState = BattleState.ESCAPE;
            SavePlayerStatus();
            escapeSuccess.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            escapeSuccess.SetActive(false);
            player.transform.GetChild(0).gameObject.SetActive(true);
            player.transform.GetChild(1).gameObject.SetActive(false);
            Vector3 spawnPos = SaveGameManager.StringToVector3(PlayerPrefs.GetString("LastPos"));
            saveGameManager.StartLoadScene(spawnPos, winScene.Name);
        }
        else
        {
            Debug.Log("failed escape");
            battleState = BattleState.ENEMYTURN;
            escapeFail.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            escapeFail.SetActive(false);
            yield return StartCoroutine(EnemyTurn());
            escapeCount++;
        }
    }

    IEnumerator EnemyTurn()
    {
        UnitStatus unitTarget;
        while (!inDebt)
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
                    unitTarget = playerStatuses[target];
                    if (enemy.GetComponent<UnitStatus>().skills.Count > 1)
                    {
                        skill = Random.Range(0, enemy.GetComponent<UnitStatus>().skills.Count);
                    }
                    else
                    {
                        skill = 0;
                    }
                    SetSkillToUse(enemy.GetComponent<UnitStatus>().skills[skill]);
                    CheckMpCost(enemyMPHUD, unitTarget);
                    skillImage.SetActive(true);
                    skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.skillName;

                    StartCoroutine(enemyMPHUD.SetMP(skillMpCost, false));
                    StartCoroutine(enemy.GetComponent<UnitStatus>().animCon.AttackAnimation(skillToUse, unitTarget.transform.position));
                    
                    yield return StartCoroutine(unitTarget.animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
                    CheckSkillDmg(playerStatusHUDList[target], unitTarget);
                    skillImage.SetActive(false);
                }
                else
                {
                    unitTarget = enemy.GetComponent<EnemyAI>().Defend(enemyStatuses, enemy);
                    target = enemyStatuses.IndexOf(unitTarget);
                    foreach (Skill skill in enemy.GetComponent<UnitStatus>().skills)
                    {
                        if (skill.currentType == unitTarget.heal)
                        {
                            SetSkillToUse(skill);
                        }
                    }
                    CheckMpCost(enemyMPHUD, unitTarget);
                    skillImage.SetActive(true);
                    skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.skillName;
                    StartCoroutine(enemyMPHUD.SetMP(skillMpCost, false));
                    StartCoroutine(enemy.GetComponent<UnitStatus>().animCon.AttackAnimation(skillToUse, unitTarget.transform.position));
                    
                    yield return StartCoroutine(unitTarget.animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
                    CheckSkillDmg(enemyStatusHUDList[target], unitTarget);
                    skillImage.SetActive(false);
                }
                UnitStatus deadPlayer = playerStatuses.FindLast(x => x.health <= 0 && x.condition != CharacterStatus.Condition.DEAD);
                int deadPlayerIndex = playerStatuses.FindLastIndex(x => x.health <= 0 && x.condition != CharacterStatus.Condition.DEAD);
                if (deadPlayer != null)
                {
                    deadPlayer.animCon.ChangeAnimation(deadPlayer.animCon.Die, 0.1f, 0, 0);
                    deadPlayer.condition = CharacterStatus.Condition.DEAD;
                    partyLeft--;
                    playerStatusHUDList[deadPlayerIndex].SetDeadStatus(true);
                    if (partyLeft == 0)
                    {
                        battleState = BattleState.LOST;
                        StopCoroutine(enemyCoroutine);
                        yield return StartCoroutine(EndBattle());

                    }
                }
                if (inDebt || (enemyMPHUD.GetMP() <= 0 && !inDebt))
                {
                    break;
                }
            }
            if (inDebt || (enemyMPHUD.GetMP() <= 0 && !inDebt))
            {
                break;
            }
        }
        StartEndTurn(false);

        // if the player health is still
        // above 0 when the turn finishes
        // it's our turn again!

    }
    private void SavePlayerStatus()
    {
        foreach(UnitStatus playerStatus in playerStatuses)
        {
            playerStatus.characterStatus.health = playerStatus.health;
            playerStatus.characterStatus.condition = playerStatus.condition;
        }
    }

    IEnumerator EndBattle()
    {
        // check if we won
        if (battleState == BattleState.WIN)
        {
            // you may wish to display some kind
            // of message or play a victory fanfare
            // here
            SavePlayerStatus();
            playerTurn.SetActive(false);
            victory.SetActive(true);
            yield return new WaitForSeconds(1);
            Vector3 spawnPos = SaveGameManager.StringToVector3(PlayerPrefs.GetString("LastPos"));
            saveGameManager.StartLoadScene(spawnPos, winScene.Name);
        }
        // otherwise check if we lost
        // You probably want to display some kind of
        // 'Game Over' screen to communicate to the 
        // player that the game is lost
        if (battleState == BattleState.LOST)
        {
            // you may wish to display some kind
            // of message or play a sad tune here!
            //playerTurn.SetActive(false);
            //enemyTurn.SetActive(false);
            defeat.SetActive(true);
            yield return new WaitForSeconds(1);
            Vector3 spawnPos = SaveGameManager.StringToVector3(PlayerPrefs.GetString("LastPos"));
            SceneManagement.Instance.LoadScene(loseScene.Name);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (battleState == BattleState.PLAYERTURN)
        {
            SelectTarget();
        }

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    Vector3 spawnPos = SaveGameManager.StringToVector3(PlayerPrefs.GetString("LastPos"));
        //    saveGameManager.StartLoadScene(spawnPos, winScene.Name);
        //}
    }
}
