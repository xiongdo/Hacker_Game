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

    public void HideSetter()
    {
        if (_transformerGo)
        {
            _transformerGo.SetActive(false);
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
            _transformerGo.AddComponent<Transformer>();
            _transformerGo.transform.parent = null;
            _transformerGo = null;
        }
    }
}
