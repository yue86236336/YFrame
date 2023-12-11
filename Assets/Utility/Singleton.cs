using System;
using UnityEngine;

namespace YF
{
    //不继承Mono
    public class Singleton<T> where T : class
    {
        private static T instance;

        public static T Instance
        {
            get => instance ??= new Lazy<T>().Value; //线程安全
        }

    }

    //继承Mono
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get => instance ??= new Lazy<GameObject>(new GameObject($"{typeof(T).Name}Single")).Value.AddComponent<T>();
        }

        public virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this as T;
            }

            DontDestroyOnLoad(gameObject);
        }
    }

}
