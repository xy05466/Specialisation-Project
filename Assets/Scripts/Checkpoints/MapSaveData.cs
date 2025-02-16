using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaveData
{
    public string name;
    public List<TileInfo> tileInfos;

    public MapSaveData() 
    {
        tileInfos = new List<TileInfo>();
    }    
}
