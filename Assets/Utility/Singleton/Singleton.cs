using System;
using UnityEngine;

namespace YF
{
    //不继承Mono,抽象类只能继承不能创建实例
    public abstract class Singleton<T> where T : class
    {
        private static T instance;

        public static T Instance
        {
            get => instance ??= new Lazy<T>().Value; //线程安全

            //子类必须显式声明私有无参构造函数，就可以防止在外部被实例化
            // Type type = typeof(T);
            // ConstructorInfo info = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
            // if (info != null)
            //     instance = info.Invoke(null) as T;
            // else
            //     Debug.LogError("没有得到对应的无参构造函数");

        }

    }

    //继承Mono
    //继承了Mono的脚本 不能够直接new
    //只能通过拖动到对象上 或者 通过 加脚本的api AddComponent去加脚本
    //U3D内部帮助我们实例化它
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get => instance ??= new Lazy<GameObject>(new GameObject($"{typeof(T).Name}Single")).Value.AddComponent<T>();
        }

        protected virtual void Awake()
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
