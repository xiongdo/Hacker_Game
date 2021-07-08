using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                GameObject singleton = GameObject.Find("SingletonMgr");
                if (!singleton)
                {
                    singleton = new GameObject("SingletonMgr");
                }
                instance = singleton.GetComponent<T>();
                DontDestroyOnLoad(singleton);
                if (!instance)
                {
                    instance = singleton.AddComponent<T>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    
    public static T JustInstance
    {
        get
        {
            return instance;
        }
    }

    private static bool applicationIsQuitting = false;

    protected void OnDestroy()
    {
        Dispose();
    }

    public virtual void InitMgr()
    {

    }

    protected virtual void Dispose()
    {
        instance = null;
    }

    protected virtual void OnApplicationQuit()
    {
        applicationIsQuitting = true;
        instance = null;
        //SnkDebug.Log("OnApplicationQuit");
    }
}
