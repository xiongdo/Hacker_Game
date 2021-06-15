using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField]
    protected Tilemap _curMap;

    public Tilemap Map
    {
        get
        {
            return _curMap;
        }
    }

}
