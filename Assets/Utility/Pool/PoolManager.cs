using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YF
{
    /// <summary>
    /// 缓存池（对象池）模块管理器
    /// </summary>
    public class PoolManager : Singleton<PoolManager>
    {
        //柜子当中有抽屉的体现
        private Dictionary<string, Stack<GameObject>> poolDic = new Dictionary<string, Stack<GameObject>>();

        //声明私有构造函数，防止在外面被实例化
        private PoolManager() { }

        /// <summary>
        /// 拿东西的方法
        /// </summary>
        /// <param name="抽屉的名字"></param>
        /// <returns>从缓存池中取出的对象</returns>
        public GameObject GetObj(string name)
        {
            GameObject obj;
            //有抽屉并且抽屉里有对象，可以直接拿
            if (poolDic.ContainsKey(name) && poolDic[name].Count > 0)
            {
                //弹出栈中的对象，直接返回给外部使用
                obj = poolDic[name].Pop();
                //激活对象再返回
                obj.SetActive(true);
            }
            //否则应该去创造
            else
            {
                //没有的时候通过资源加载去实例化一个Gameobject
                obj = GameObject.Instantiate(Resources.Load<GameObject>(name));
                //避免实例化的对象名字默认后面加上clone
                obj.name = name;
            }
            return obj;
        }

        /// <summary>
        /// 往缓存池中放入对象
        /// </summary>
        /// <param name="抽屉（对象）"></param>
        /// <param name="要放入的物体"></param>
        public void PushObj(GameObject obj)
        {
            //并非直接销毁，而是失活等待启用
            obj.SetActive(false);
            //如果没有抽屉先创建再添加
            if (!poolDic.ContainsKey(obj.name))
                poolDic.Add(obj.name, new Stack<GameObject>());
            poolDic[obj.name].Push(obj);
        }

        /// <summary>
        /// 用于清除整个对象池当中的数据，主要用于切换场景 
        /// </summary>
        public void ClearPool()
        {
            poolDic.Clear();
        }
    }
}

