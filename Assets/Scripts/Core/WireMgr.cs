using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireMgr : Singleton<WireMgr>
{
    private List<Wire> _wires;

    [SerializeField]
    private GameObject _lenPrefab;
    [SerializeField]
    private GameObject _canvas;

    public GameObject LenPrefab
    {
        get
        {
            return _lenPrefab;
        }
    }

    public GameObject WireCanvas
    {
        get
        {
            return _canvas;
        }
    }

    private void Awake()
    {
        _wires = new List<Wire>();    
    }

    public Wire SpawnWire()
    {
        return gameObject.AddComponent<Wire>();
    }

    public void AddWire(Wire wire)
    {
        _wires.Add(wire);
    }

    public void RemoveWire(Wire wireD)
    {
        foreach (var wire in _wires)
        {
            if (wireD.Equals(wire))
            {
                _wires.Remove(wire);
                MonoBehaviour.Destroy(wire);
            }
        }
    }
}
