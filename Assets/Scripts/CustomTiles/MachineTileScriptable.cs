using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public enum TileType
{
    Safe,
    Unsafe
}

[CreateAssetMenu(fileName = "MachineTile", menuName = "MachineTile")]
public class MachineTileScriptable : Tile
{
    public TileType _type = TileType.Unsafe;
}
