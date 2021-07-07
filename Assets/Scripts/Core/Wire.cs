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

    private float _lenWidth = 10f;

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
        _canvas = GameObject.Find("Canvas");
        _len = GameObject.Find("Len");
    }

    public void SetEndPosition(Vector3Int cellPos)
    {
        var count = _points.Count;
        _points[count-1] = GameWorld.Instance.GetCellCenterWorldPos(cellPos);
    }

    public Wire AddPoint(Vector3Int cellPos)
    {
        Vector3 worldPos = GameWorld.Instance.GetCellCenterWorldPos(cellPos);
        _points.Add(worldPos);
        if (_points.Count > 1)
            _lens.Add(SpawnLen(worldPos));

        return this;
    }

    private GameObject SpawnLen(Vector3 worldPos)
    {
        var retLen = Instantiate<GameObject>(_len, _canvas.transform);
        Vector3 endPos = _points[_points.Count - 2];
        var lastPos = _canvas.transform.InverseTransformVector(endPos);
        retLen.transform.localPosition = lastPos - new Vector3(_lenWidth/2, 0f, 0f);
        float distance = Vector3.Distance(_points[_points.Count - 2], worldPos);
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
