using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _lens;

    private List<Vector3> _worldPoints;

    [SerializeField]
    private List<Vector3Int> _cellPoints;

    private GameObject _canvas;

    private GameObject _len;

    private float _lenWidth = 0.1f;

    private float _alpha = 1.0f;

    private Color _color;

    private int _unstable = 0;

    private Boolean _isoutput = false;

    public float Alpha
    {
        get
        {
            return _alpha;
        }
        set
        {
            _alpha = value;
            foreach (var len in _lens)
            {
                Image image = len.GetComponent<Image>();
                var color = image.color;
                image.color = new Color(color.r, color.g, color.b, _alpha);
            }
        }
    }

    public Color WireColor
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
            foreach (var len in _lens)
            {
                Image image = len.GetComponent<Image>();
                var alpha = image.color.a;
                image.color = new Color(value.r, value.g, value.b, alpha);
            }
        }
    }

    public int PointsCount
    {
        get
        {
            return _worldPoints.Count;
        }
    }

    private void Awake()
    {
        _lens = new List<GameObject>();
        _worldPoints = new List<Vector3>();
        _cellPoints = new List<Vector3Int>();
        _canvas = WireMgr.Instance.WireCanvas;
        _len = WireMgr.Instance.LenPrefab;
        _color = Color.red;
    }

    public void PerviewEndPosition(Vector3Int cellPos)
    {
        if (PointsCount > 1)
        {
            RemoveLast();
            AddPoint(cellPos);
        }
    }

    private void AddPointInternel(Vector3Int cellPos)
    {
        Vector3 worldPos = GameWorld.Instance.GetCellCenterWorldPos(cellPos);

        _worldPoints.Add(worldPos);
        _cellPoints.Add(cellPos);
        if (PointsCount > 1) SpawnLen();
    }

    private void RemoveLastPointInternel()
    {
        _worldPoints.RemoveAt(_worldPoints.Count - 1);
        _cellPoints.RemoveAt(_cellPoints.Count - 1);
        GameObject.Destroy(_lens[_lens.Count - 1]);
        _lens.RemoveAt(_lens.Count - 1);
        if (_unstable == 2 )
        {
            _worldPoints.RemoveAt(_worldPoints.Count - 1);
            _cellPoints.RemoveAt(_cellPoints.Count - 1);
            GameObject.Destroy(_lens[_lens.Count - 1]);
            _lens.RemoveAt(_lens.Count - 1);
        }
    }

    public Wire AddPoint(Vector3Int cellPos)
    {
        if (PointsCount == 0)
        {
            AddPointInternel(cellPos);
            AddPointInternel(cellPos);
        }
        else
        {
            var lastPos = _cellPoints[PointsCount - 1];
            if (cellPos.x != lastPos.x && cellPos.y != lastPos.y)
            {
                AddPointInternel(new Vector3Int(cellPos.x, lastPos.y, 0));
                _unstable = 2;
            }
            else _unstable = 1;
            AddPointInternel(cellPos);
        }

        return this;
    }

    private void RemoveLast()
    {
        if (PointsCount >= 2)
        {
            RemoveLastPointInternel();
        }
    }

    private void SetLenPositionAndRotation(Transform lenTf)
    {
        Vector3 nextPos = _worldPoints[PointsCount - 1];
        Vector3 lastPos = _worldPoints[PointsCount - 2];
        var lastPixel = Camera.main.WorldToScreenPoint(lastPos);
        lastPixel -= new Vector3(_lenWidth / 2f, 0f, 0f);
        lenTf.position = Camera.main.ScreenToWorldPoint(lastPixel);

        float Distance = Vector3.Distance(nextPos, lastPos);
        var rtf = lenTf.GetComponent<RectTransform>();
        var lenLength = Distance + _lenWidth;
        rtf.sizeDelta = new Vector2(lenLength, _lenWidth);
        rtf.pivot = new Vector2(_lenWidth / (2 * lenLength), 0.5f);
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, (nextPos - lastPos).normalized);
        rtf.rotation = rot;
    }

    private void SpawnLen()
    {
        var retLenTf = Instantiate<GameObject>(_len, _canvas.transform).transform;

        SetLenPositionAndRotation(retLenTf);

        _lens.Add(retLenTf.gameObject);
    }

    private void OnDestroy()
    {
        foreach(var len in _lens)
        {
            GameObject.Destroy(len);
        }
    }

    public void SetOutput()
    {
        _isoutput = true;
    }

    public bool IsIntersect(Wire wire)
    {
        foreach (var lenA in _lens)
        {
            foreach(var lenB in wire._lens)
            {
                var rectA = lenA.GetComponent<RectTransform>().rect;
                var rectB = lenB.GetComponent<RectTransform>().rect;
                if (rectA.Overlaps(rectB))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private List<Transformer> GetLineIntersectTransformers(Vector3Int start, Vector3Int end)
    {
        List<Transformer> ret = new List<Transformer>();
        int distanceInt = (int)Vector3Int.Distance(end - start, Vector3Int.zero);
        var step = (end - start) / distanceInt;
        for (Vector3Int iterVector = start; start != end; start += step)
        {
            var transformer = GameWorld.Instance.GetCellPosTransformerOrNot(iterVector);
            if (transformer) ret.Add(transformer);
        }

        return ret;
    }

    public List<Transformer> GetTransformers()
    {
        List<Transformer> ret = new List<Transformer>();
        int count = _cellPoints.Count;
        for (int i = 1; i < count; ++i)
        {
            ret.AddRange(GetLineIntersectTransformers(_cellPoints[i - 1], _cellPoints[i]));
        }

        return ret;
    }
}
