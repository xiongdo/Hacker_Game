using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWire : MonoBehaviour
{
    enum DrawWireState
    {
        Drawing,
        hanging
    }

    private Wire _previewWire;

    private DrawWireState _state;

    private void Awake()
    {
        _state = DrawWireState.hanging;
    }

    public void StartDrawWire()
    {
        _state = DrawWireState.Drawing;
        // _previewWire = WireMgr.Instance.SpawnWire();
    }

    public void EndDrawWire()
    {
        _state = DrawWireState.hanging;
        _previewWire = null;
    }

    private void Update()
    {
        if (_state == DrawWireState.Drawing)
        {
            DrawUpdate();
        }
        else if (_state == DrawWireState.hanging)
        {
            HangUpdate();
        }
    }

    private void DrawUpdate()
    {
        var mouseGridPos = Utility.GetMouseCellPosition();
        if (_previewWire == null)
            _previewWire = WireMgr.Instance.SpawnWire();

        if (Input.GetMouseButtonDown(0))
        {
            _previewWire.AddPoint(mouseGridPos);
        }
        else if(Input.GetMouseButtonDown(1))
        {
            // _previewWire.AddPoint(mouseGridPos);
            WireMgr.Instance.AddWire(_previewWire);
            _previewWire = null;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_previewWire)
                MonoBehaviour.Destroy(_previewWire);
            EndDrawWire();
        }
        else
        {
            _previewWire.PerviewEndPosition(mouseGridPos);
        }
    }

    private void HangUpdate()
    {
        var mouseGridPos = Utility.GetMouseCellPosition();
        Wire needDelete = null;
        if (Input.GetMouseButtonDown(1))
        {
            foreach (var wire in WireMgr.Instance.Wires)
            {
                if (wire.ContainsPoint(mouseGridPos))
                {
                    needDelete = wire;
                    break;
                }
            }
            if (needDelete)
                WireMgr.Instance.RemoveWire(needDelete);
        }
    }
}
