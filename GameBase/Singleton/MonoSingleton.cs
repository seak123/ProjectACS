using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var name = typeof(T).ToString();
                var obj = new GameObject(name);
                GameObject.DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<T>();
            }
            return _instance;
        }
        private set { }
    }

    private static T _instance;
}
