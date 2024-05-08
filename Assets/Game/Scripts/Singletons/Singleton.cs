using Unity.VisualScripting;
using UnityEngine;

namespace Game.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public bool BootStrap = true;
        protected static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                transform.SetParent(GameObject.Find("Managers").transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public abstract class PersistantSingleton<T> : Singleton<T> where T : Component
    {
        public override void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                transform.SetParent(GameObject.Find("Systems").transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}