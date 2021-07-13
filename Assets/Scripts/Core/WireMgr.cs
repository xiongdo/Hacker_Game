using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireMgr : Singleton<WireMgr>
{
    private List<Wire> _wires;

    [SerializeField]
    private GameObject _lenPrefab;
    [SerializeField]
    private GameObject _canvas;
    [SerializeField]
    private float _wireDepth;

    private Wire _output;

    [SerializeField]
    private Text _outputText;

    private static int _colorIndex = 0;

    public int NextIndex
    {
        get
        {
            return (_colorIndex += 1) % 5;
        }
    }
        
    public float WireDepth 
    { 
        get
        {
            return _wireDepth;
        }
    }

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

    public Wire Output
    {
        private set
        {
            _output = value;
        }
        get
        {
            return _output;
        }

    }

    public List<Wire> Wires
    {
        get
        {
            return _wires;
        }
    }

    private void Awake()
    {
        SetCanvasSize();
        _wires = new List<Wire>();
        _wireDepth = 10f;

        // »­OutputµçÀÂ
        var wire = SpawnWire();
        var leftbottom = new Vector3(0, Screen.height/2f, 0);
        var originWorldCellPos = GameWorld.Instance.Map.WorldToCell(Camera.main.ScreenToWorldPoint(leftbottom));
        wire.AddPoint(originWorldCellPos);
        wire.PerviewEndPosition(originWorldCellPos + new Vector3Int(2, 0, 0));
        wire.WireColor = Color.blue;
        wire.SetOutput();
        Output = wire;
    }

    private void SetCanvasSize()
    {
        var size = Camera.main.orthographicSize;
        _canvas.GetComponent<RectTransform>().sizeDelta = new Vector2((float)Camera.main.pixelWidth / (float)Camera.main.pixelHeight * size * 2, size * 2);
    }

    public Wire SpawnWire()
    {
        return gameObject.AddComponent<Wire>();
    }

    public void AddWire(Wire wire)
    {
        _wires.Add(wire);
        wire.WireColor = new List<Color>() { Color.white, Color.black, Color.gray, Color.green, Color.cyan }[NextIndex];
        // ParseWire(wire);
    }

    public void ParseWires()
    {
        var visited = new Dictionary<Wire, bool>();
        foreach (var wire in _wires)
        {
            visited.Add(wire, false);
        }
        Queue<Wire> needVisit = new Queue<Wire>(GetIntersects(Output));
        OutputMgr.Instance.ClearListenTransformer();
        while (needVisit.Count > 0)
        {
            var wire = needVisit.Dequeue();
            var transformers = wire.GetTransformers();
            foreach (var transformer in transformers)
            {
                OutputMgr.Instance.AddListenTransformer(transformer);
            }

            visited[wire] = true;
            var intersectWires = GetIntersects(wire);
            foreach (var intersectWire in intersectWires)
            {
                if (!visited[intersectWire])
                    needVisit.Enqueue(intersectWire);
            }
        }
    }

    public void RemoveWire(Wire wireD)
    {
        Wire needDeleteWire = null;
        foreach (var wire in _wires)
        {
            if (wireD.Equals(wire))
            {
                needDeleteWire = wire;

            }
        }
        if (needDeleteWire)
            _wires.Remove(needDeleteWire);
            MonoBehaviour.Destroy(needDeleteWire);
    }

    private List<Wire> GetIntersects(Wire wire)
    {
        List<Wire> ret = new List<Wire>();

        foreach (var wireB in _wires)
        {
            if (wire.IsIntersect(wireB) && !wire.Equals(wireB))
                ret.Add(wireB);
        }
        return ret;
    }
}
