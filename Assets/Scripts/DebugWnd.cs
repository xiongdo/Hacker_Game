using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Log
{
    public string Message;
    public string StackTrace;
    public LogType LogType;
}

public class DebugWnd : Singleton<DebugWnd>
{
    public KeyCode shortCut = KeyCode.BackQuote;

    public int edge = 20;

    private const string _initStr = "Hello, boy";

    private const string _windowTitle = "console";

    private bool _visible = false;

    private readonly List<Log> logs = new List<Log>();

    private Rect _windowRect;

    private Vector2 _scrollPosition;

    private string _commondStr = _initStr;

    static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
    {
        {LogType.Assert, Color.white},
        {LogType.Error, Color.red},
        {LogType.Exception, Color.red},
        {LogType.Log, Color.white},
        {LogType.Warning, Color.yellow},
    };

    void OnEnable()
    {
        _windowRect = new Rect(edge, edge, Screen.width / 2, Screen.height / 2);
        Application.logMessageReceived += HandleLog;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Interrupter.Instance.Play();
        }    
        if (Input.GetKeyDown(shortCut))
        {
            _visible = !_visible;
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 20), "Play"))
        {
            Interrupter.Instance.Play();
        }
        if (GUI.Button(new Rect(10, 40, 100, 20), "Stop"))
        {
            Interrupter.Instance.Stop();
        }
        if (GUI.Button(new Rect(10, 70, 100, 20), "Pause"))
        {
            Interrupter.Instance.Pause();
        }
        if (GUI.Button(new Rect(10, 100, 100, 20), "Step"))
        {
            Interrupter.Instance.InterruptAll();
        }
        if (!_visible) return;
        _windowRect = GUILayout.Window(666, _windowRect, DrawWindow, _windowTitle);
    }

    void HandleLog(string message, string stackTrace, LogType type)
    {
        logs.Add(new Log
        {
            Message = message,
            StackTrace = stackTrace,
            LogType = type,
        });
    }

    void DrawWindow(int windowId)
    {
        DrawLogView();
    }

    void DrawLogView()
    {
        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        foreach (var log in logs)
        {
            GUI.contentColor = logTypeColors[log.LogType];
            GUILayout.Label(log.Message);
        }
        GUILayout.EndScrollView();
        GUI.contentColor = Color.white;
    }
}
