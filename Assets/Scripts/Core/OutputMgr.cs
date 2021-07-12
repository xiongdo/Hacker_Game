using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputMgr : Singleton<OutputMgr>
{
    List<Transformer> _outputTransformers;

    private void Awake()
    {
        _outputTransformers = new List<Transformer>();
    }

    public void AddListenTransformer(Transformer transformer)
    {
        _outputTransformers.Add(transformer);
    }
}
