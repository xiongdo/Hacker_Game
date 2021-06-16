using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Transformer : MonoBehaviour, IPointerClickHandler
{
    public float _sleepTime = 1f;

    private float _curSleepTime;
    
    protected bool _shouldCalmDown = false;
    
    public int SafeModeLayer;
    private void Awake()
    {
        SafeModeLayer = LayerMask.NameToLayer("MoveObject");
    }

    private void Update()
    {
        if (_shouldCalmDown)
        {
            if (_curSleepTime < _sleepTime)
            {
                _curSleepTime += Time.deltaTime;
                return;
            }

            _curSleepTime = 0f;
            _shouldCalmDown = false;
        }
        if (MachineMgr.Instance.IsSafeMode)
        {
            ScanSafe();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(gameObject);
        }
    }

    private void ScanSafe()
    {
        foreach (CellDirection dir in Enum.GetValues(typeof(CellDirection)))
        {
            var curCellPos = GameWorld.Instance.Map.WorldToCell(transform.position);
            var scanCellPos = dir.GetDirectionCellPos(curCellPos);
            var scanWorldPos = GameWorld.Instance.GetCellCenterWorldPos(scanCellPos);

            var tile = GameWorld.Instance.Map.GetTile(scanCellPos) as MachineTileScriptable;
            if (tile && tile._type == TileType.Safe)
            {
                MachineMgr.Instance.DrawTiles(tile, dir.GetOppositeCellPos(curCellPos));
                MachineMgr.Instance.AddRemove(scanCellPos);
                _shouldCalmDown = true;
            }
            Debug.DrawLine(
                scanWorldPos, 
                scanWorldPos + (Vector3)(dir.GetVectorFromDirection()) * 0.4f);
        }
        // var mouseGridPos = GameWorld.Instance.Map.WorldToCell();
    }
}
