using UnityEngine;

namespace DefaultNamespace
{
    public abstract class MonoGlobalSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var managers = FindObjectsOfType(typeof(T)) as T[];
                if (managers != null && managers.Length != 0)
                {
                    if (managers.Length == 1)
                    {
                        _instance = managers[0];
                        _instance.gameObject.name = typeof(T).Name;
                        return _instance;
                    }
                    Debug.LogError("Class " + typeof(T).Name + " exists multiple times in violation of singleton pattern. Destroying all copies");
                    foreach (var manager in managers)
                    {
                        Destroy(manager.gameObject);
                    }
                }
                var go = new GameObject(typeof(T).Name, typeof(T));
                _instance = go.GetComponent<T>();
                DontDestroyOnLoad(go);

                return _instance;
            }
        }
        private static T _instance;

        private void Awake()
        {
            AwakeImpl();
        }

        protected virtual void AwakeImpl() { }
    }
}
