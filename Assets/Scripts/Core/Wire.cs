using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    private List<GameObject> _lens;

    private List<Vector3> _points;

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

    private void Awake()
    {
        _lens = new List<GameObject>();
        _points = new List<Vector3>();
        _canvas = WireMgr.Instance.WireCanvas;
        _len = WireMgr.Instance.LenPrefab;
    }

    public void SetEndPosition(Vector3Int cellPos)
    {
        var count = _points.Count;
        if (count > 1)
        {
            RemoveLast();
            AddPoint(cellPos);
        }
    }

    public Wire AddPoint(Vector3Int cellPos)
    {
        Vector3 worldPos = GameWorld.Instance.GetCellCenterWorldPos(cellPos);
        _points.Add(worldPos);
        if (_points.Count == 1)
            _points.Add(worldPos);
        if (_points.Count > 1)
            _lens.Add(SpawnLen(worldPos));

        return this;
    }

    private void RemoveLast()
    {
        if (_points.Count >= 2)
        {
            _points.RemoveAt(_points.Count - 1);
            GameObject.Destroy(_lens[_lens.Count - 1]);
            _lens.RemoveAt(_lens.Count - 1);
        }
    }

    private GameObject SpawnLen(Vector3 worldPos)
    {
        var retLen = Instantiate<GameObject>(_len, _canvas.transform);
        Vector3 endPos = _points[_points.Count - 2];
        retLen.transform.position = endPos;
        var worldPixel = Camera.main.WorldToScreenPoint(worldPos);
        var endPixel = Camera.main.WorldToScreenPoint(endPos);
        float distance = Vector3.Distance(worldPixel, endPixel);
        Debug.Log("The distance :" + distance);
        retLen.GetComponent<RectTransform>().sizeDelta = new Vector2(distance, _lenWidth);
        Quaternion rot = Quaternion.FromToRotation(Vector3.right, (worldPos - endPos).normalized);
        retLen.transform.localRotation = rot;

        return retLen;
    }

    private void OnDestroy()
    {
        foreach(var len in _lens)
        {
            GameObject.Destroy(len);
        }
    }
}
