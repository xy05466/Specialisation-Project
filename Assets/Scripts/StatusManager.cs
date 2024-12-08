using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatusManager : MonoBehaviour
{
    [SerializeField] private CharacterStatus playerStatus;
    
    private CharacterStatus enemyStatus;
    private PlayerMovementController playerMovementController;

    private bool isAttacked;
    Vector3 targetPosition = new Vector3(0.0f, 0.0f, 0.0f);

    private int encounternum = 1;
    private int keynum;

    private void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
        enemyStatus = new CharacterStatus();
        keynum = PlayerPrefs.GetInt("keynum", 0);
        //if (PlayerPrefs.GetString("EncounterPos") != "")
        //{
        //    transform.position = StringToVector3(PlayerPrefs.GetString("EncounterPos"));
        //}
    }

    private void Update()
    {
        if (playerMovementController.GetIsMoving() && this.playerStatus.health > 0)
        {
            float distanceToTarget = Vector3.Distance(targetPosition, transform.position);
            float rand = Random.Range(1, 101);
            if (rand < distanceToTarget * 0.075f)
            {
                if (SceneManager.GetActiveScene().name == "SampleScene")
                {
                    Debug.Log("encounter" + encounternum);
                    PlayerPrefs.SetString("EncounterPos", this.transform.localPosition.ToString());
                    encounternum++;
                    targetPosition = transform.position;
                   // SceneLoader.instance.LoadLevel("BattleArena");
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

                    SceneLoader.instance.LoadLevel("BattleArena 1");
                }
                    
                   
                    
                
            }
        }
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
