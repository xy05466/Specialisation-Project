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
    private RectTransform mapParent;
    public List<RectTransform> mapIcons;
    public RectTransform image;
    public RectTransform regularDoor;
    public RectTransform oneWayDoor;
    public RectTransform lockedDoor;

    public CinemachineVirtualCamera virtualCamera;
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
                mapParent = Instantiate(image, new Vector3(tiles[0].tilePos.x, image.anchoredPosition.y + 50, tiles[0].tilePos.y) * 0.2f, image.gameObject.transform.rotation, canvas.transform);
                map.Add(mapParent);
                map[0].gameObject.SetActive(false);
                n++;
                continue;
            }
            switch (child.tag)
            {
                case "Floor":
                    tiletoCreate = mapIcons[0];
                    break;
                case "RegularDoor":
                    tiletoCreate = mapIcons[1];
                    break;
                case "OneWayDoor":
                    tiletoCreate = mapIcons[2];
                    break;
                case "LockedDoor":
                    tiletoCreate = mapIcons[3];
                    break;
            }
            //if (child.tag == "Floor")
            //{
            //    tiletoCreate = image;
            //}
            //else if (child.tag == "RegularDoor")
            //{
            //    tiletoCreate = regularDoor;
            //}
            //else if (child.tag == "OneWayDoor")
            //{
            //    tiletoCreate = oneWayDoor;
            //}
            //else if (child.tag == "LockedDoor")
            //{
            //    tiletoCreate = lockedDoor;
            //}
            map.Add(Instantiate(tiletoCreate, new Vector3(child.tilePos.x, tiletoCreate.anchoredPosition.y, child.tilePos.y) * 0.2f, Quaternion.identity, mapParent.transform));
            map[n].transform.position = new Vector3(map[n].transform.position.x, mapParent.transform.position.y, map[n].transform.position.z);
            map[n].localRotation = Quaternion.Euler(0, child.tileRot.x, child.tileRot.y);

            //map[n].rotation = child.tileRot;
            n++;
        }
        mapParent.anchoredPosition = image.anchoredPosition;
        virtualCamera.gameObject.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        foreach (RectTransform icon in mapIcons)
        {
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

    public void CheckTile(Vector2 tilePos)
    {
        foreach(TileInfo child in tiles)
        {
            if (tilePos == child.tilePos)
            {
                Debug.Log(tilePos);
                Vector3 screenPos = (RectTransformUtility.WorldToScreenPoint(Camera.main, tilePos));
                playerPos.text = tilePos.ToString();
                //image.anchoredPosition = screenPos / 5;
            }
        }
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
