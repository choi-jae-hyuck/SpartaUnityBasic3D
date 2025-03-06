using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 싱글톤 제네릭
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindAnyObjectByType(typeof(T));
                
                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    
    protected virtual void Awake()
    {
        if (_instance != null) 
            Destroy(gameObject);
        else
            DontDestroyOnLoad(this);
    }
}
