using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this as T;
    }

    private void OnDestroy()
    {
        if (this == _instance)
            _instance = null;
    }
}
