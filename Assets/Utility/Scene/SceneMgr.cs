using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace YF
{
    public class SceneMgr : Singleton<SceneMgr>
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="loadSceneMode">加载方式 叠加或替换</param>
        /// <param name="onSceneLoaded">场景加载完成后的回调</param>
        public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single, UnityAction onSceneLoaded = null)
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
            onSceneLoaded?.Invoke();
        }

        public void LoadSceneAsyn(string sceneName, UnityAction onSceneLoaded = null)
        {
            MonoMgr.Instance.StartCoroutine(LoadSceneAsynCoroutine(sceneName, onSceneLoaded));
        }

        /// <summary>
        /// 协程异步加载场景
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsynCoroutine(string name, UnityAction onSceneLoaded)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync(name);
            //可以得到场景加载的一个进度
            while (!ao.isDone)
            {
                //事件中心 向外分发 进度情况  外面想用就用
                EventCenter.Instance.Publish(1, ao.progress);
                //这里面去更新进度条
                yield return ao.progress;
            }
            //加载完成过后 才会去执行fun
            onSceneLoaded?.Invoke();
        }

        /// <summary>
        /// 返回当前激活场景的名字
        /// </summary>
        /// <returns></returns>
        public string GetActiveSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
