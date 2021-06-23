using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTools
{
    public static bool IsThreePower(int n)
    {
        return n > 0 && 1162261467 % n == 0;
    }

    public static int GetSize(Vector3Int c, Vector3Int o)
    {
        int d = Math.Max(Math.Abs(o.x - c.x), Math.Abs(o.y - c.y));
        return (int)Math.Pow(3, (int)Math.Log(2*d-1, 3));
    }
}
