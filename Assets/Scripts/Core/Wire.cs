using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    private List<GameObject> _lens;

    private List<Vector3> _worldPoints;

    private List<Vector3Int> _cellPoints;

    private GameObject _canvas;

    private GameObject _len;

    private float _lenWidth = 5f;

    private float _alpha = 1.0f;

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
    }

    public void SetEndPosition(Vector3Int cellPos)
    {
        var count = _worldPoints.Count;
        if (count > 1)
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
    }

    public Wire AddPoint(Vector3Int cellPos)
    {
        AddPointInternel(cellPos);
        if (PointsCount == 1)
        {
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
        var nextPixel = Camera.main.WorldToScreenPoint(nextPos);
        var lastPixel = Camera.main.WorldToScreenPoint(lastPos);
        lastPixel -= new Vector3(_lenWidth / 2f, 0f, 0f);
        lenTf.position = Camera.main.ScreenToWorldPoint(lastPixel);

        float pixelDistance = Vector3.Distance(nextPixel, lastPixel);
        var rtf = lenTf.GetComponent<RectTransform>();
        var lenLength = pixelDistance + _lenWidth / 2f;
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
}
