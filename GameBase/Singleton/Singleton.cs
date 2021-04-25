using UnityEngine;

public class Singleton<T> where T : class, new()
{
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                CreateInstance();
            }
            return _instance;
        }
        private set { }
    }

    private static void CreateInstance()
    {
        _instance = new T();
    }

    private static T _instance;
}
