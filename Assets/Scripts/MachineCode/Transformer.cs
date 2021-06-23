using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;

enum TransformerState
{
    Sleep,
    Ready
}

public class Transformer : MonoBehaviour, IPointerClickHandler
{
    public float _sleepTime = 1f;

    private float _curSleepTime;
    
    protected bool _shouldCalmDown = false;

    private TransformerState _state;

    private Light2D _light;

    private int _curSize;

    public int SafeModeLayer;

    private void Awake()
    {
        _state = TransformerState.Ready;
        _light = transform.Find("Point Light 2D").GetComponent<Light2D>();
        SafeModeLayer = LayerMask.NameToLayer("MoveObject");
    }

    private void Update()
    {
        if (_state == TransformerState.Sleep)
        {
            _light.color = new Color(1.0f, 0.0f, 0.0f);
        }
        else if (_state == TransformerState.Ready)
        {
            _light.color = new Color(0.0f, 0.0f, 1.0f);
        }
        //if (_shouldCalmDown)
        //{
        //    if (_curSleepTime < _sleepTime)
        //    {
        //        _curSleepTime += Time.deltaTime;
        //        return;
        //    }

        //    _curSleepTime = 0f;
        //    _shouldCalmDown = false;
        //}
        //if (MachineMgr.Instance.IsSafeMode)
        //{
        //    ScanSafe();
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Destroy(gameObject);
        }
    }

    public void Work()
    {
        if (_state == TransformerState.Sleep)
        {
            _state = TransformerState.Ready;
            return;
        }
        else if (_state == TransformerState.Ready)
        {
            if (ScanSafe())
                _state = TransformerState.Sleep;
        }
    }

    public Transformer ListenInterrupt()
    {
        Interrupter.Instance.AddTransformer(this);

        return this;
    }

    public Transformer SetSize(int size)
    {
        _curSize = size;

        return this;
    }

    private bool ScanSafe()
    {
        var iswork = false;
        foreach (CellDirection dir in Enum.GetValues(typeof(CellDirection)))
        {
            var curCellPos = GameWorld.Instance.Map.WorldToCell(transform.position);
            var scanCellPoss = dir.GetDirectionCellPoss(curCellPos, _curSize);
            foreach (var scanCellPos in scanCellPoss)
            {
                var scanWorldPos = GameWorld.Instance.GetCellCenterWorldPos(scanCellPos);

                var tile = GameWorld.Instance.Map.GetTile(scanCellPos) as MachineTileScriptable;
                if (tile && tile._type == TileType.Safe)
                {
                    iswork = true;
                    MachineMgr.Instance.DrawTiles(tile, dir.GetOppositeCellPosTrans(curCellPos, scanCellPos));
                    MachineMgr.Instance.AddRemove(scanCellPos);
                    //_shouldCalmDown = true;
                }
                Debug.DrawLine(
                    scanWorldPos,
                    scanWorldPos + (Vector3)(dir.GetVectorFromDirection()) * 0.4f
                );
            }
        }
        return iswork;
        // var mouseGridPos = GameWorld.Instance.Map.WorldToCell();
    }

    void OnDestroy()
    {
        if (Interrupter.JustInstance)
            Interrupter.Instance.RemoveTransformer(this);
    }
}
