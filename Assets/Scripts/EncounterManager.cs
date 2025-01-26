using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour
{
    //public static StatusManager Instance = null;
    [SerializeField] private CharacterStatus playerStatus;
    public HUDManager statusHUD;
    public SaveGameManager saveGameManager;
    private CharacterStatus enemyStatus;
    public PlayerMovementController playerMovementController;
    private ItemEffects itemEffects;

    private bool isAttacked;
    Vector3 targetPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private int encounternum = 1;
    private int keynum;
    private bool inEncounter;

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("SampleScene"))
        {
            if (PlayerPrefs.GetString("EncounterPos") != "")
            {
                transform.position = StringToVector3(PlayerPrefs.GetString("EncounterPos"));
            }
            saveGameManager.TryTempLoadData();
        }
        Debug.Log("OnSceneLoaded: " + scene.name);
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
        if (playerMovementController.GetIsMoving())
        {
            float distanceToTarget = Vector3.Distance(targetPosition, transform.position);
            float rand = Random.Range(1, 101);
            if (rand < distanceToTarget * 0.1f && !inEncounter)
            {
                if (SceneManager.GetActiveScene().name == "SampleScene")
                {
                    inEncounter = true;
                    saveGameManager.TempSaveData();
                    Debug.Log("encounter" + encounternum);
                    PlayerPrefs.SetString("SceneName", SceneManager.GetActiveScene().name);
                    PlayerPrefs.SetString("EncounterPos", this.transform.localPosition.ToString());
                    encounternum++;
                    targetPosition = transform.position;
                    SceneLoader.instance.LoadLevel("BattleArena");
                }
            }
        }
        else
        {
            targetPosition = transform.position;
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
        if (other.tag == "Key" && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt("keynum", PlayerPrefs.GetInt("keynum", 0) + 1);
            keynum = PlayerPrefs.GetInt("keynum", 0);
            Destroy(other.gameObject);
            Debug.Log(keynum);
        }
        if (this.playerStatus.health > 0)
        {
            if (other.tag == "Boss" && Input.GetKeyDown(KeyCode.E))
            {

                    //isAttacked = true;
                    //setBattleData(other);
                if (SceneManager.GetActiveScene().name == "SampleScene")
                {

                    SceneLoader.instance.LoadLevel("BattleArena");
                }
                    
                   
                    
                
            }
        }
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

    public bool SetPartyCondition()
    {
        if (this.playerStatus.condition == CharacterStatus.Condition.NORMAL &&
           this.playerStatus.party.FirstOrDefault(i => i.condition == CharacterStatus.Condition.DEAD) == null)
        {
            return false;
        }
        this.playerStatus.health = this.playerStatus.maxHealth;
        this.playerStatus.condition = CharacterStatus.Condition.NORMAL;
        statusHUD.UpdateOverlay(this.playerStatus);
        statusHUD.SimpleUpdateHP(this.playerStatus);

        foreach (CharacterStatus status in this.playerStatus.party)
        {
            if (status.condition == CharacterStatus.Condition.DEAD)
            {
                status.condition = CharacterStatus.Condition.NORMAL;
                statusHUD.UpdateOverlay(status);
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
