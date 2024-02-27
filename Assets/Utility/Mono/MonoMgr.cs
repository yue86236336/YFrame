using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

namespace YF
{
    public class MonoMgr : Singleton<MonoMgr>
    {
        private MonoController controller;

        public MonoMgr()
        {
            //保证了MonoController对象的唯一性
            GameObject obj = new GameObject("MonoController");
            controller = obj.AddComponent<MonoController>();
        }
        public void AddUpdateListener(UnityAction func)
        {
            controller.AddUpdateListener(func);
        }
        public void RemoveUpdateListener(UnityAction func)
        {
            controller.RemoveUpdateListener(func);
        }
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return controller.StartCoroutine(routine);
        }

        public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return controller.StartCoroutine(methodName, value);
        }
        public Coroutine StartCoroutine(string methodName)
        {
            return controller.StartCoroutine(methodName);
        }

    }
}
