using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TransformerSetter : MonoBehaviour
{
    public GameObject _transformerPrefab;

    private GameObject _transformerGo;

    private float _maxScale = 3.0f;

    private float _minScale = 1.0f;

    private int _curSize = 1;

    public void HideSetter()
    {
        if (_transformerGo)
        {
            _transformerGo.SetActive(false);
        }
    }

    public void SetScale(float mul)
    {
        if (_transformerGo)
        {
            float scale = Mathf.Max(Mathf.Min(mul, _maxScale), _minScale);
            _transformerGo.transform.localScale = new Vector3(scale, scale, scale);
            _curSize = scale > 1.0f ? 3 : 1;
        }
    }

    public void Preview(Vector3Int cellPos)
    {
        if (!_transformerGo)
        {
            _transformerGo = Instantiate(_transformerPrefab, transform);
            _transformerGo.transform.localPosition = Vector3.zero;
        }
        
        _transformerGo.SetActive(true);
        _transformerGo.transform.position = GameWorld.Instance.GetCellCenterWorldPos(cellPos);
    }

    public void SetTransformer()
    {
        if (_transformerGo)
        {
            _transformerGo.AddComponent<BoxCollider2D>();
            var transformer = _transformerGo.AddComponent<Transformer>()
                                            .ListenInterrupt()
                                            .SetSize(_curSize);
            _transformerGo.transform.parent = null;
            _transformerGo = null;
            _curSize = 1;
            GameWorld.Instance.AddTransformer(transformer);
        }
    }
}
