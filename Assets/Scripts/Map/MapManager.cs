using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject tileParent;
    public Canvas canvas;
    public GameObject mapParent;
    private RectTransform mapOrigin;
    public List<RectTransform> mapIcons;
    //public RectTransform image;
    //public RectTransform regularDoor;
    //public RectTransform oneWayDoor;
    //public RectTransform lockedDoor;

    public GameObject virtualCamera;
    public int height;
    public int width;
    public TMP_Text playerPos;
    private RectTransform tiletoCreate;
    private GameObject[,] mapGrid;
    private List<TileInfo> tiles;
    private List<RectTransform> map;
    private int tilesCount = 5;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        //virtualCamera.gameObject.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
        virtualCamera = GameObject.FindWithTag("VirtualCamera");

        cam = Camera.main;
        tilesCount = tileParent.transform.childCount;
        tiles = new List<TileInfo>();
        map = new List<RectTransform>();
        //CheckGridNum();
        foreach (Transform child in tileParent.transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.GetComponent<Tile>() != null)
            {
                TileInfo tileInfo = new TileInfo();
                tileInfo.tilePos = new Vector2(child.position.x, child.position.z);
                tileInfo.tileRot = child.localEulerAngles;
                
                tileInfo.tag = child.tag;
                tiles.Add(tileInfo);
            }
            
        }
        tiles.Sort((v1, v2) => v1.tilePos.y != v2.tilePos.y ? v1.tilePos.y.CompareTo(v2.tilePos.y) : v1.tilePos.x.CompareTo(v2.tilePos.x));

        //Debug.Log(mapParent.transform.localPosition);
        int n = 0;
        
        //mapParent.anchoredPosition = new Vector2(mapParent.anchoredPosition.x, image.anchoredPosition.y);

        foreach (TileInfo child in tiles)
        {
            if (n == 0)
            {
                mapOrigin = Instantiate(mapIcons[1], new Vector3(tiles[0].tilePos.x, mapIcons[1].anchoredPosition.y + 50, tiles[0].tilePos.y) * 0.2f, mapIcons[1].gameObject.transform.rotation, mapParent.transform);
                mapOrigin.anchoredPosition3D = new Vector3(mapOrigin.anchoredPosition3D.x, mapOrigin.anchoredPosition3D.y, 0);
                foreach (Image image in mapOrigin.gameObject.GetComponentsInChildren<Image>())
                {
                    image.enabled = false;
                }
               //mapOrigin.gameObject.GetComponentInChildren<Image>().enabled = false;
                map.Add(mapOrigin);
                n++;
                continue;
            }
            switch (child.tag)
            {
                case "Floor":
                    tiletoCreate = mapIcons[1];
                    break;
                case "RegularDoor":
                    tiletoCreate = mapIcons[2];
                    break;
                case "OneWayDoor":
                    tiletoCreate = mapIcons[3];
                    break;
                case "LockedDoor":
                    tiletoCreate = mapIcons[4];
                    break;
            }
            map.Add(Instantiate(tiletoCreate, new Vector3(child.tilePos.x, tiletoCreate.anchoredPosition.y, child.tilePos.y) * 0.2f, Quaternion.identity, mapOrigin.transform));
            map[n].transform.position = new Vector3(map[n].transform.position.x, mapOrigin.transform.position.y, map[n].transform.position.z);
            map[n].localRotation = Quaternion.Euler(0, child.tileRot.x, child.tileRot.y);
            //map[n].gameObject.GetComponentInChildren<Image>().enabled = false;
            foreach (Image image in map[n].gameObject.GetComponentsInChildren<Image>())
            {
                image.enabled = false;
            }
            n++;
        }
        mapOrigin.anchoredPosition = mapIcons[1].anchoredPosition;
        mapIcons[0].transform.position = new Vector3(map[0].transform.position.x, mapOrigin.transform.position.y, map[0].transform.position.z);

        virtualCamera.gameObject.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        foreach (RectTransform icon in mapIcons)
        {
            if (icon.name == "PlayerIcon")
            {
                continue;
            }
            icon.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.M))
        {
            mapParent.gameObject.SetActive(!mapParent.gameObject.activeSelf);
        }
    }

    public void CheckPlayerPos(Vector2 tilePos)
    {
        int i = 0;
        foreach(TileInfo child in tiles)
        {
            if (tilePos == child.tilePos)
            {
                Debug.Log(tilePos);
                playerPos.text = tilePos.ToString();
                mapIcons[0].anchoredPosition = map[i].anchoredPosition;
                if (!map[i].gameObject.GetComponent<Image>().enabled)
                {
                    foreach (Image image in map[i].gameObject.GetComponentsInChildren<Image>())
                    {
                        image.enabled = true;
                    }
                }
            }
            i++;
        }
    }

    public void CheckPlayerRot(float playerRot)
    {
        mapIcons[0].localRotation = Quaternion.Euler(0, 0, -playerRot);
    }

    private void CreateGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

            }
        }
    }

    private void CheckGridNum()
    {
        for (int i = 0; i < tileParent.transform.childCount; i++)
        {

        }
    }
}
