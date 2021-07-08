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
        _previewWire = WireMgr.Instance.SpawnWire();
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
        else
        {

        }
    }

    private void DrawUpdate()
    {
        var mouseGridPos = Utility.GetMouseCellPosition();
        
        if (Input.GetMouseButtonUp(0))
        {
            _previewWire.AddPoint(mouseGridPos);
        }
        else if(Input.GetMouseButton(1))
        {
            // _previewWire.AddPoint(mouseGridPos);
            WireMgr.Instance.AddWire(_previewWire);
            EndDrawWire();
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            MonoBehaviour.Destroy(_previewWire);
            EndDrawWire();
        }
        else
        {
            _previewWire.SetEndPosition(mouseGridPos);
        }
    }
}
