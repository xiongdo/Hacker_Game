using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Lumin;

public class GameWorld : Singleton<GameWorld>
{
    [SerializeField]
    protected Tilemap _curMap;

    [Header("Global Light")]
    [SerializeField]
    protected Light2D _globalLight2D;

    [Range(0f, 1f)] 
    public float _globalLightIntesity;
    
    public Tilemap Map
    {
        get
        {
            return _curMap;
        }
    }

    private void Start()
    {
        InitSceneParm();
    }

    public void InitSceneParm()
    {
        if (_globalLight2D)
        {
            // _globalLight2D.intensity = _globalLightIntesity;
        }
    }
}
