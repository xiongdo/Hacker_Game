using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWnd : Singleton<DebugWnd>
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Interrupter.Instance.Play();
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
    }
}
