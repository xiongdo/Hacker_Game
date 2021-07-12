using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InterrupterState
{
    Stop,
    Runing,
    Pause,
    Debug
}

public class Interrupter : Singleton<Interrupter>
{
    private float _tick = 0.5f;

    private int _ticks;

    private float _curlevel;

    private List<Transformer> _transformers;

    private InterrupterState _state;

    public int Ticks
    {
        get
        {
            return _ticks;
        }
    }

    void Awake()
    {
        _curlevel = 0.0f;
        _transformers = new List<Transformer>();
        _state = InterrupterState.Stop;
    }

    void Update()
    {
        if (_state == InterrupterState.Stop)
        {

        }
        else if (_state == InterrupterState.Runing)
        {
            _curlevel += Time.deltaTime;
            if (_curlevel >= _tick)
            {
                _ticks += 1;
                _curlevel -= _tick;
                InterruptAll();
            }
        }
        else if (_state == InterrupterState.Pause)
        {

        }
        
        if (_state != InterrupterState.Runing)
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                InterruptAll();
            }
        }
    }

    public void Play()
    {
        WireMgr.Instance.ParseWires();
        _state = InterrupterState.Runing;
    }

    public void Stop()
    {
        _state = InterrupterState.Stop;
        _curlevel = 0.0f;
    }

    public void Pause()
    {
        _state = InterrupterState.Pause;
    }

    public void AddTransformer(Transformer transformer)
    {
        _transformers.Add(transformer);
    }

    public void RemoveTransformer(Transformer transformer)
    {
        _transformers.Remove(transformer);
    }

    public void InterruptAll()
    {
        foreach(var transformer in _transformers)
        {
            if (transformer != null)
                transformer.Work();
        }
    }
}
