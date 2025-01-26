using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore.Text;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

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
    private int mpDebt;
    private bool inDebt;
    private List<StatusHUD> hudToChangeList;

    private float maxPlayerMana;
    private float currPlayerMana;
    private float maxEnemyMana;
    private float currEnemyMana;

    private int partyIndex;
    private EnemyAI.Action action;

    public static UnityAction<UnitStatus> OnHealthChanged;
    public static UnityAction<float> OnManaChanged;

    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.START;
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

                index = playerStatuses.Count - 1;
                partyToAttack = playerStatuses;
                arrow.gameObject.transform.position = new Vector3(playerStatuses[index].transform.position.x, playerStatuses[index].transform.position.y + 5f, playerStatuses[index].transform.position.z);

                hudToChangeList = playerStatusHUDList;
                //Debug.Log(enemyToAttack.name);
                
            }
            if (Input.GetKeyDown(KeyCode.O))
            {

                index = enemyStatuses.Count - 1;
                partyToAttack = enemyStatuses;
                arrow.gameObject.transform.position = new Vector3(enemyStatuses[index].transform.position.x, enemyStatuses[index].transform.position.y + 5f, enemyStatuses[index].transform.position.z);
                hudToChangeList = enemyStatusHUDList;
                //Debug.Log(enemyToAttack.name);
                
            }
            if (Input.GetKeyDown(KeyCode.J) && index >0)
            {
                index--;
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
                enemyToAttack = partyToAttack[index];
                arrow.gameObject.transform.position = new Vector3(enemyToAttack.transform.position.x, enemyToAttack.transform.position.y + 5f, enemyToAttack.transform.position.z);
                Debug.Log(enemyToAttack.name);
            }
            if (Input.GetKeyDown(KeyCode.L) && (index + 1) <= encounterLeft - 1)
            {
                index++;
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
                enemyToAttack = partyToAttack[index];
                arrow.gameObject.transform.position = new Vector3(enemyToAttack.transform.position.x, enemyToAttack.transform.position.y + 5f, enemyToAttack.transform.position.z);
                Debug.Log(enemyToAttack.name);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                enemyToAttack = partyToAttack[index];
                Debug.Log("attack" + enemyToAttack.name);
                OnAttackButtonPress();
                hudToChangeList = enemyStatusHUDList;
                partyToAttack = enemyStatuses;
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
            //skillMpCost += mpDebt;
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
        skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.name;


        CheckMpCost(playerMPHUD, target);
        StartCoroutine(playerMPHUD.SetMP((skillMpCost), false));
        StartCoroutine(playerStatuses[partyIndex].animCon.AttackAnimation(skillToUse, target.transform.position));

        yield return StartCoroutine(target.animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
        CheckSkillDmg(hudToChangeList[index], target);

       

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
        if (inDebt || (playerMPHUD.GetMP() <= 0 && !inDebt))
        {
            // if the enemy health is still
            // above 0 when the turn finishes
            // it's enemy's turn!   
            //yield return StartCoroutine(playerMPHUD.SetMP(Mathf.Abs(maxPlayerMana - playerMPHUD.GetMP()) - mpDebt, true));
            //inDebt = false;
            //mpDebt = 0;
            //setPlayerSkills(false);
            //battleState = BattleState.ENEMYTURN;
            //yield return StartCoroutine(EnemyTurn());
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
        if (isPlayerTurn)
        {
            yield return StartCoroutine(playerMPHUD.SetMP(Mathf.Abs(maxPlayerMana - playerMPHUD.GetMP()) - mpDebt, true));
            inDebt = false;
            mpDebt = 0;
            setPlayerSkills(false);
            battleState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyTurn());
        }
        else
        {
            yield return StartCoroutine(enemyMPHUD.SetMP(Mathf.Abs(maxEnemyMana - enemyMPHUD.GetMP()) - mpDebt, true));
            inDebt = false;
            mpDebt = 0;
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

    IEnumerator TryEscape()
    {
        escapeChance = Random.Range(0, 10);
        Debug.Log("escaped");
        SavePlayerStatus();
        yield return new WaitForSeconds(1);
        SceneLoader.instance.LoadLevel("SampleScene");
        //if (escapeChance < 7 + (escapeCount))
        //{
        //    Debug.Log("escaped");
        //    yield return new WaitForSeconds(1);
        //    SceneLoader.instance.LoadLevel("SampleScene");
        //}
        //else
        //{
        //    Debug.Log("failed escape");
        //    battleState = BattleState.ENEMYTURN;
        //    yield return StartCoroutine(EnemyTurn());
        //    escapeCount++;
        //}
    }

    IEnumerator EnemyTurn()
    {
        int i = 0;
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
                    skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.name;

                    StartCoroutine(enemyMPHUD.SetMP(skillMpCost, false));
                    StartCoroutine(enemy.GetComponent<UnitStatus>().animCon.AttackAnimation(skillToUse, unitTarget.transform.position));
                    
                    yield return StartCoroutine(unitTarget.animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration / 2));
                    CheckSkillDmg(playerStatusHUDList[target], unitTarget);
                    OnHealthChanged?.Invoke(unitTarget);
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
                    skillImage.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = skillToUse.name;
                    StartCoroutine(enemyMPHUD.SetMP(skillMpCost, false));
                    StartCoroutine(enemy.GetComponent<UnitStatus>().animCon.AttackAnimation(skillToUse, unitTarget.transform.position));
                    CheckSkillDmg(enemyStatusHUDList[target], unitTarget);
                    
                    yield return StartCoroutine(unitTarget.animCon.ReactAnimation(skillToUse.effect.GetComponent<ParticleSystem>().main.duration));
                    
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
        //yield return StartCoroutine(enemyMPHUD.SetMP(Mathf.Abs(maxEnemyMana - enemyMPHUD.GetMP()) - mpDebt, true));
        //inDebt = false;
        //mpDebt = 0;
        ////yield return new WaitForSeconds(1);
        //battleState = BattleState.PLAYERTURN;
        //yield return StartCoroutine(PlayerTurn());
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
            SavePlayerStatus();
            yield return new WaitForSeconds(1);
            SceneLoader.instance.LoadLevel("SampleScene");
        }
    }
    // Update is called once per frame
    void Update()
    {
        SelectTarget();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.instance.LoadLevel("SampleScene");
        }
    }
}
