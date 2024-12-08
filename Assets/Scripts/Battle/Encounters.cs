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
    public int enemyNum = 0;

    public void randomiseEnemy()
    {
        if (isBoss)
        {
            enemyNum = 1;
        }
        else
        {
            enemyNum = Random.Range(1, 4);
        }

        enemyToSpawn = new List<GameObject>();
        for (int i = 0; i < enemyNum; i++) 
        {
            int index = Random.Range(0, scriptableEnemies.Count);
            //scriptableEnemies[index].GetComponent<UnitStatus>().Init();
            enemyToSpawn.Add(scriptableEnemies[index]);
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
