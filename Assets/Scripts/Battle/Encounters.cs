using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounters : MonoBehaviour
{
    public List<GameObject> scriptableEnemies = null;
    public List<ScriptableObject> scriptableItems = null;
    public List<GameObject> enemyToSpawn;
    //public List<GameObject> enemies = null;
    public bool isBoss;
    public bool isTutorial;
    public int enemyNum = 0;

    public void randomiseEnemy()
    {
        if (isBoss)
        {
            enemyNum = 3;
            enemyToSpawn = new List<GameObject>();
            foreach (GameObject enemy in scriptableEnemies)
            {
                enemyToSpawn.Add(enemy);
            }
            return;
        }
        else if (isTutorial)
        {
            enemyNum = 2;
            enemyToSpawn = new List<GameObject>();
            foreach (GameObject enemy in scriptableEnemies)
            {
                enemyToSpawn.Add(enemy);
            }
            return;
        }
        else
        {
            enemyNum = UnityEngine.Random.Range(1, 3);
            enemyToSpawn = new List<GameObject>();
            for (int i = 0; i < enemyNum; i++)
            {
                int index = UnityEngine.Random.Range(0, scriptableEnemies.Count - 1);
                //scriptableEnemies[index].GetComponent<UnitStatus>().Init();
                enemyToSpawn.Add(scriptableEnemies[index]);
            }
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
