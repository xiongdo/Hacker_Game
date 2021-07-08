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

    public Wire AddPoint(Vector3Int cellPos)
    {
        Vector3 worldPos = GameWorld.Instance.GetCellCenterWorldPos(cellPos);
        _worldPoints.Add(worldPos);
        _cellPoints.Add(cellPos);
        if (_worldPoints.Count == 1)
        {
            _worldPoints.Add(worldPos);
            _cellPoints.Add(cellPos);
        }
        if (_worldPoints.Count > 1)
            _lens.Add(SpawnLen(worldPos));

        return this;
    }

    private void RemoveLast()
    {
        if (_worldPoints.Count >= 2)
        {
            _worldPoints.RemoveAt(_worldPoints.Count - 1);
            _cellPoints.RemoveAt(_cellPoints.Count - 1);
            GameObject.Destroy(_lens[_lens.Count - 1]);
            _lens.RemoveAt(_lens.Count - 1);
        }
    }

    private GameObject SpawnLen(Vector3 worldPos)
    {
        var retLen = Instantiate<GameObject>(_len, _canvas.transform);
        Vector3 endPos = _worldPoints[_worldPoints.Count - 2];
        var worldPixel = Camera.main.WorldToScreenPoint(worldPos);
        var endPixel = Camera.main.WorldToScreenPoint(endPos);
        float pixelDistance = Vector3.Distance(worldPixel, endPixel);
        endPixel -= new Vector3(_lenWidth / 2, 0f, 0f);
        retLen.transform.position = Camera.main.ScreenToWorldPoint(endPixel);
        Debug.Log("The distance :" + pixelDistance);
        retLen.GetComponent<RectTransform>().sizeDelta = new Vector2(pixelDistance + _lenWidth, _lenWidth);
        retLen.GetComponent<RectTransform>().pivot = new Vector2(_lenWidth / (2 * pixelDistance + 2 * _lenWidth), 0.5f);
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
