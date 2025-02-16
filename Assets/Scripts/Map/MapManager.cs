using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
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

    private GameObject virtualCamera;
    //public int height;
    //public int width;
    public TMP_Text playerPos;
    private RectTransform tiletoCreate;
    private GameObject[,] mapGrid;
    private List<TileInfo> tiles;
    private List<RectTransform> map;
    private int tilesCount = 5;
    // Start is called before the first frame update
    void Start()
    {
        //check if map data for this scene exists, if not generate new map
        string mappath = Application.persistentDataPath + "/SaveData/" + SceneManager.GetActiveScene().name + ".sav";

            GenerateTiles();
            GenerateMap();
        //if (!File.Exists(mappath))
        //{

        //}

         
       
        //tilesCount = tileParent.transform.childCount;
        //tiles = new List<TileInfo>();
        //map = new List<RectTransform>();
        //foreach (Transform child in tileParent.transform.GetComponentsInChildren<Transform>())
        //{
        //    if (child.gameObject.GetComponent<Tile>() != null)
        //    {
        //        TileInfo tileInfo = new TileInfo();
        //        tileInfo.tilePos = new Vector2(child.position.x, child.position.z);
        //        tileInfo.tileRot = child.localEulerAngles;

        //        tileInfo.tag = child.tag;
        //        tiles.Add(tileInfo);
        //    }

        //}
        //tiles.Sort((v1, v2) => v1.tilePos.y != v2.tilePos.y ? v1.tilePos.y.CompareTo(v2.tilePos.y) : v1.tilePos.x.CompareTo(v2.tilePos.x));
        //GenerateMap();

       
    }

    private void Awake()
    {
        SaveLoad.OnLoadMap += LoadMap;
        SaveLoad.OnSaveMap += SaveMap;
    }

    private void OnDestroy()
    {
        SaveLoad.OnLoadMap -= LoadMap;
        SaveLoad.OnSaveMap -= SaveMap;
    }

    private void SaveMap()
    {
        SaveGameManager.mapData.name = SceneManager.GetActiveScene().name;
        SaveGameManager.mapData.tileInfos = tiles;
    }

    private void LoadMap(MapSaveData data)
    {
        if (data.tileInfos != null)
        {
            this.tiles = data.tileInfos;
           
        }
        else
        {
            GenerateTiles();
        }
       
        GenerateMap();
        //LoadTiles();
    }

    private void LoadTiles()
    {
        
        //int i = 0;
        //List<TileInfo> infos = tiles.FindAll(x => x.discovered);
        //List<RectTransform> discoveredTiles = map.Where(x => infos.Any(z => z.tilePos == new Vector2(x.transform.position.x, x.transform.position.z))).ToList();
        //foreach (RectTransform rect in discoveredTiles)
        //{
        //    foreach (Image image in rect)
        //    {
        //        image.enabled = true;
        //    }
        //}
    }

    private void GenerateMap()
    {
        map = new List<RectTransform>();
        int n = 0;
        foreach (TileInfo child in tiles)
        {
            if (n == 0)
            {
                mapOrigin = Instantiate(mapIcons[1], new Vector3(tiles[0].tilePos.x, mapIcons[1].anchoredPosition.y + 50, tiles[0].tilePos.y) * 0.2f, mapIcons[1].gameObject.transform.rotation, mapParent.transform);
                mapOrigin.anchoredPosition3D = new Vector3(mapOrigin.anchoredPosition3D.x, mapOrigin.anchoredPosition3D.y, 0);

                if (!child.discovered)
                {
                    foreach (Image image in mapOrigin.gameObject.GetComponentsInChildren<Image>())
                    {
                        image.enabled = false;
                    }
                }
                else
                {
                    Debug.Log("discovered");
                }
               
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
                case "RoomDoor":
                    tiletoCreate = mapIcons[5];
                    break;
            }
            map.Add(Instantiate(tiletoCreate, new Vector3(child.tilePos.x, tiletoCreate.anchoredPosition.y, child.tilePos.y) * 0.2f, Quaternion.identity, mapOrigin.transform));
            map[n].transform.position = new Vector3(map[n].transform.position.x, mapOrigin.transform.position.y, map[n].transform.position.z);
            map[n].localRotation = Quaternion.Euler(0, 0, -child.tileRot.y);
            if (!child.discovered)
            {
                foreach (Image image in map[n].gameObject.GetComponentsInChildren<Image>())
                {
                    image.enabled = false;
                }
            }
            else
            {
                Debug.Log("discovered");
            }

            n++;
        }
        mapOrigin.anchoredPosition = mapIcons[1].anchoredPosition;
        mapIcons[0].transform.position = new Vector3(map[0].transform.position.x, mapOrigin.transform.position.y, map[0].transform.position.z);

        //virtualCamera.gameObject.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        foreach (RectTransform icon in mapIcons)
        {
            if (icon.name == "PlayerIcon")
            {
                continue;
            }
            icon.gameObject.SetActive(false);
        }
    }

    private void GenerateTiles()
    {
        tilesCount = tileParent.transform.childCount;
        tiles = new List<TileInfo>();
       
        foreach (Transform child in tileParent.transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.GetComponent<Tile>() != null)
            {
                TileInfo tileInfo = new TileInfo();
                tileInfo.tilePos = new Vector2(child.position.x, child.position.z);
                tileInfo.tileRot = child.eulerAngles;
                if (tileInfo.tileRot.y > 0)
                {
                    Debug.Log("rotate");
                }
                tileInfo.tag = child.tag;
                tiles.Add(tileInfo);
            }

        }
        //v1.X != v2.X ? v1.X.CompareTo(v2.X) : v1.Y.CompareTo(v2.Y));
        tiles.Sort((v1, v2) => v1.tilePos.x != v2.tilePos.x ? v1.tilePos.x.CompareTo(v2.tilePos.x) : v1.tilePos.y.CompareTo(v2.tilePos.y));
    }



    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.M))
        {
            mapParent.gameObject.SetActive(!mapParent.gameObject.activeSelf);
        }
    }


    public void SetDiscovered(Vector2 tilePos)
    {
        tiles.Find(x => x.tilePos == tilePos).discovered = true;
    }


    public void CheckPlayerPos(Vector2 tilePos)
    {
        int i = 0;

        foreach(TileInfo child in tiles)
        {
            if (tilePos == child.tilePos)
            {
                Debug.Log(tilePos);
                //playerPos.text = tilePos.ToString();
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

}
