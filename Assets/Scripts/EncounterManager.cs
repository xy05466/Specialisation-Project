using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    //public static StatusManager Instance = null;
    public bool enableEncounters = true;
    [SerializeField] private CharacterStatus playerStatus;
    public HUDManager statusHUD;
    public SaveGameManager saveGameManager;
    private CharacterStatus enemyStatus;
    public PlayerMovementController playerMovementController;
    public GameObject bossTalk;
    public GameObject boss1BattleDialogue;
    public UnitStatus status;
    public AudioClip collectSound;
    private ItemEffects itemEffects;

    private bool isAttacked;
    Vector3 targetPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private float encounterChance = 0.15f;
    private float encounterThreshold = 100f;
    private float baseChance = 0.001f;
    private float distanceMoved = 0f;
    private Vector3 lastPos;
    private int encounternum = 1;
    private int keynum;
    private bool inEncounter;
    private bool isTalking;

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
        Debug.Log(transform.position);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        saveGameManager = GameObject.FindWithTag("SaveGame").GetComponent<SaveGameManager>();
        if (scene.name.Contains("Overworld") || scene.name.Contains("Room"))
        {
            if (PlayerPrefs.GetString("LastPos") != "")
            {
                status.Init();
                if (SceneManagement.Instance.GetPreviousScene().Contains("BattleArena"))
                {
                    transform.parent.position = StringToVector3(PlayerPrefs.GetString("LastPos"));
                }
            }
            saveGameManager.TryTempLoadData();
        }
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        //playerMovementController = GetComponent<PlayerMovementController>();
        enemyStatus = new CharacterStatus();
        keynum = PlayerPrefs.GetInt("keynum", 0);
        itemEffects = GetComponent<ItemEffects>();
       
        //statusHUD.SimpleSetHP(status);
       
    }

    private void Update()
    {
        if (playerMovementController.GetIsMoving() && !boss1BattleDialogue.activeSelf)
        {
            float distanceToTarget = Vector3.Distance(targetPosition, transform.parent.position);
            float rand = Random.Range(0, 256);
            if (rand < distanceToTarget * encounterChance && !inEncounter && enableEncounters)
            {
                if (SceneManager.GetActiveScene().name.Contains("Overworld") || SceneManager.GetActiveScene().name.Contains("Room"))
                {
                    inEncounter = true;
                    saveGameManager.TempSaveData();
                    Debug.Log("encounter" + encounternum);
                    PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
                    PlayerPrefs.SetString("LastPos", transform.parent.position.ToString());
                    encounternum++;
                    targetPosition = transform.parent.position;
                    if (PlayerPrefs.GetInt("didtutorial", 0) == 0)
                    {
                        SceneManagement.Instance.LoadScene("TutorialBattleArena");
                    }
                    else
                    {
                        SceneManagement.Instance.LoadScene("BattleArena");
                    }
                }
            }
        }
        else
        {
            targetPosition = transform.parent.position;
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Key")
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(other.gameObject);
            
            
        }

        if (other.tag == "Boss" && !isTalking)
        {
            bossTalk.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                //bossTalk.SetActive(false);
                boss1BattleDialogue.SetActive(true);
                isTalking = true;
            }
               
                    
                   
                    
                
            
        }
        
    }

    public void StartBossBattle()
    {
        PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("LastPos", transform.parent.position.ToString());
        saveGameManager.TempSaveData();
        SceneManagement.Instance.LoadScene("BossBattleArena 1");
    }
    
    public bool SetPartyHealth()
    {
        if (this.playerStatus.health == this.playerStatus.maxHealth &&
            this.playerStatus.party.FirstOrDefault(i => i.health != i.maxHealth) == null)
        {
            return false;
        }
        this.playerStatus.health = this.playerStatus.maxHealth;
        foreach (CharacterStatus status in this.playerStatus.party)
        {
            if (status.health > 0 && status.health < status.maxHealth)
            {
                status.health = status.maxHealth;
                statusHUD.SimpleUpdateHP(status);
            }
        }
        return true;
    }

    public void SetPartyBuffs()
    {
        foreach (CharacterStatus status in this.playerStatus.party)
        {
            statusHUD.UpdateBuffOverlay(status);
        }
    }

    public bool SetPartyCondition()
    {
        if (this.playerStatus.condition == CharacterStatus.Condition.NORMAL &&
           this.playerStatus.party.FirstOrDefault(i => i.condition == CharacterStatus.Condition.DEAD) == null)
        {
            return false;
        }
        this.playerStatus.health = this.playerStatus.maxHealth;
        this.playerStatus.condition = CharacterStatus.Condition.NORMAL;
        statusHUD.UpdateConditionOverlay(this.playerStatus);
        statusHUD.SimpleUpdateHP(this.playerStatus);

        foreach (CharacterStatus status in this.playerStatus.party)
        {
            if (status.condition == CharacterStatus.Condition.DEAD)
            {
                status.condition = CharacterStatus.Condition.NORMAL;
                status.health = status.maxHealth;
                statusHUD.SimpleUpdateHP(status);
                statusHUD.UpdateConditionOverlay(status);
            }
            if (status.health > 0 && status.health < status.maxHealth)
            {
                status.health = status.maxHealth;
                statusHUD.SimpleUpdateHP(status);
            }
        }
        return true;
    }

    private void setBattleData(Collider other)
    {
        // Player Data 
        playerStatus.position[0] = this.transform.position.x;
        playerStatus.position[1] = this.transform.position.y;

        // Enemy Data
        CharacterStatus status = other.gameObject.GetComponent<EnemyData>().enemy;
        enemyStatus.charName = status.charName;
        enemyStatus.characterGameObject = status.characterGameObject.transform.GetChild(0).gameObject;
        enemyStatus.health = status.health;
        enemyStatus.maxHealth = status.maxHealth;
        enemyStatus.mana = status.mana;
        enemyStatus.maxMana = status.maxMana;
    }
}
