using UnityEngine;
using UnityEngine.Events;

namespace YF
{
    public class MonoController : MonoBehaviour
    {
        private event UnityAction updateEvent;

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            updateEvent?.Invoke();
        }

        /// <summary>
        /// 给外部提供的 添加帧更新事件的函数
        /// </summary>
        /// <param name="fun"></param>
        public void AddUpdateListener(UnityAction fun)
        {
            updateEvent += fun;
        }

        /// <summary>
        /// 提供给外部 用于移除帧更新事件函数
        /// </summary>
        /// <param name="fun"></param>
        public void RemoveUpdateListener(UnityAction fun)
        {
            updateEvent -= fun;
        }
    }
}
