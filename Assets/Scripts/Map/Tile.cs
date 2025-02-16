using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int posX;
    private int posZ;

    public bool isPlayerOn;

    public bool discovered;
    private MapManager mapManager;

    // Start is called before the first frame update
    void Start()
    {
        //PostOffice.GetInstance().Subscribe(this.gameObject);
        mapManager = FindObjectOfType<MapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "NormalPresence")
        {
            mapManager.CheckPlayerRot(other.transform.eulerAngles.y);
            if (!isPlayerOn)
            {
                mapManager.CheckPlayerPos(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z));
                isPlayerOn = true;
                if (!discovered)
                {
                    discovered = true;
                    mapManager.SetDiscovered(new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z));
                }
                Debug.Log(this.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NormalPresence")
        {
            isPlayerOn = false;
            //Debug.Log(this.name);
        }
    }
}
