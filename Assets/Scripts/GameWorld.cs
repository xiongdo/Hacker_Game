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

    private Dictionary<Vector3Int, Transformer> _transformDict;
    
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
        _transformDict = new Dictionary<Vector3Int, Transformer>();
    }

    public void InitSceneParm()
    {
        if (_globalLight2D)
        {
            // _globalLight2D.intensity = _globalLightIntesity;
        }
    }

    public void TurnOffGlobalLight()
    {
        _globalLight2D.intensity = 0f;
    }

    public void TurnOnGloablLight()
    {
        _globalLight2D.intensity = _globalLightIntesity;
    }

    public void TurnOnGloablLight(float intensity)
    {
        _globalLight2D.intensity = intensity;
    }

    public Vector3 GetCellCenterWorldPos(Vector3Int cellPos)
    {
        Vector3 cellSize;
        return Map.CellToWorld(cellPos) + new Vector3((cellSize = Map.cellSize).x / 2f, cellSize.y / 2f, 0f);
    }

    public void AddTransformer(Transformer transformer)
    {
        var cellPos = Map.WorldToCell(transformer.transform.position);
        _transformDict.Add(cellPos, transformer);
    }

    public Transformer GetCellPosTransformerOrNot(Vector3Int cellPos)
    {
        Transformer ret = null;
        _transformDict.TryGetValue(cellPos, out ret);
        return ret;
    }
}
