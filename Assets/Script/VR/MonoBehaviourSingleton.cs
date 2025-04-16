using UnityEngine;
public class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : Component
{
    [SerializeField]
    private bool _isDestroyOnLoad = true;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindFirstObjectByType(typeof(T)) as T[];
                if (objs.Length > 0)
                    _instance = objs[0];
                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "----" + typeof(T).Name + "----";
                    //obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    virtual protected void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _instance = this as T;

        if (!_isDestroyOnLoad && Application.isPlaying)
        {
            Debug.Log("DontDestroyOnLoad " + gameObject.name);
            DontDestroyOnLoad(this);
        }
    }
}