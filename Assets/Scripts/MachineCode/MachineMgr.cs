using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class TilePair
{
    public TilePair(Vector3Int pos, MachineTileScriptable tileToDraw)
    {
        cellPos = pos;
        tile = tileToDraw;
    }
    
    public Vector3Int cellPos;
    public MachineTileScriptable tile;
}

public class MachineMgr : Singleton<MachineMgr>
{
    [Header("Machine Setting")] 
    [SerializeField]
    protected bool _isInSafeMode = true;
    public bool IsSafeMode => _isInSafeMode;

    private List<Vector3Int> _tilesToRemove = new List<Vector3Int>();

    private List<TilePair> _tilesToDraw = new List<TilePair>();

    public List<Sprite> _transformerSprites = new List<Sprite>();

    private void LateUpdate()
    {
        foreach (var pos in _tilesToRemove)
        {
            GameWorld.Instance.Map.SetTile(pos, null);
            Debug.Log("Remove at: " + pos);
        }
        foreach (TilePair tilePair in _tilesToDraw)
        {
            //GameWorld.Instance.Map.SetTile(tilePair.cellPos, tilePair.tile);
            CompoundTile(tilePair.cellPos, tilePair.tile);
            Debug.Log("Draw at: " + tilePair.cellPos);
        }
        _tilesToDraw.Clear();
        _tilesToRemove.Clear();
    }
    
    public bool DrawTiles(MachineTileScriptable tile, Vector3Int pos)
    {
        // GameWorld.Instance.Map.SetTile(pos, tile);
        _tilesToDraw.Add(new TilePair(pos, tile));
        
        return true;
    }

    public void AddRemove(Vector3Int tile)
    {
        _tilesToRemove.Add(tile);
    }

    private void CompoundTile(Vector3Int cellPos, MachineTileScriptable addTile)
    {
        var originTile = GameWorld.Instance.Map.GetTile(cellPos) as MachineTileScriptable;
        var retTile = CompoundRule(originTile, addTile);
        GameWorld.Instance.Map.SetTile(cellPos, retTile);
    }

    private MachineTileScriptable CompoundRule(MachineTileScriptable o1, MachineTileScriptable o2)
    {
        return o2;
    }
}
