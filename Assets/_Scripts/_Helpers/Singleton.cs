using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = FindObjectOfType<T>();

            if (_instance != null) return _instance;

            GameObject singletonObject = new();

            _instance = singletonObject.AddComponent<T>();

            singletonObject.name = $"{typeof(T)} (Singleton)";

            //DontDestroyOnLoad(singletonObject);

            return _instance;
        }

        set => _instance ??= value;
    }
}