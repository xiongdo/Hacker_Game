using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutputMgr : Singleton<OutputMgr>
{
    [SerializeField]
    private List<Transformer> _outputTransformers;

    [SerializeField]
    private Text _outputText;

    [SerializeField]
    private Text _stateText;

    private void Awake()
    {
        _outputTransformers = new List<Transformer>();
    }

    private void LateUpdate()
    {
        if (Interrupter.Instance.IsPlay)
        {
            int output = 0;
            foreach (var transformer in _outputTransformers)
            {
                output += transformer.TransformCount;
            }
            _outputText.text = output.ToString();
        }
    }

    public void AddListenTransformer(Transformer transformer)
    {
        _outputTransformers.Add(transformer);
    }

    public void ClearListenTransformer()
    {
        _outputTransformers.Clear();
    }
}
