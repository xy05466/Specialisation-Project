using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TileInfo : IComparable
{
    public Vector2 tilePos;
    public Vector2 mapPos;
    public Vector3 tileRot;
    public String tag;

    public int CompareTo(object obj)
    {
        var a = this;
        var b = obj as TileInfo;

        if (a.tilePos.x < b.tilePos.x)
            return -1;

        if (a.tilePos.x > b.tilePos.x)
            return 1;

        return 0;
    }
}
