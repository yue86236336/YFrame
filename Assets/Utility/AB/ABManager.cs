using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YF
{
    public class ABManager : MonoSingleton<ABManager>
    {
        //AB包管理器目的让外部更方便的进行资源加载

        //主包
        private AssetBundle mainAB = null;
        //依赖包获取用的配置文件
        private AssetBundleManifest mainfest = null;

        //AB包不能重复加载，重复加载会报错
        //字典 用字典存储加载过的AB包
        private Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// AB包存放路径
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string pathUrl
        {
            get
            {
                return Application.streamingAssetsPath + "/";
            }
        }

        /// <summary>
        /// 主包名 方便修改
        /// </summary>
        private string mainABName
        {
            get
            {
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#else
                return "StandaloneWindows";
#endif
            }
        }
        /// <summary>
        /// 加载AB包
        /// </summary>
        public void LoadAB(string abName)
        {
            //加载AB包
            if (mainAB == null)
            {
                mainAB = AssetBundle.LoadFromFile(pathUrl + mainABName);
                mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            //获取依赖包相关信息
            AssetBundle ab = null;
            string[] strs = mainfest.GetAllDependencies(abName);
            for (int i = 0; i < strs.Length; i++)
            {
                //判断包是否加载过
                if (!abDic.ContainsKey(strs[i]))
                {
                    ab = AssetBundle.LoadFromFile(pathUrl + strs[i]);
                    abDic.Add(strs[i], ab);
                }
            }
            //加载资源来源包
            //如果没有加载过再加载
            if (!abDic.ContainsKey(abName))
            {
                ab = AssetBundle.LoadFromFile(pathUrl + abName);
                abDic.Add(abName, ab);
            }
        }

        //同步加载不指定类型
        public Object LoadRes(string abName, string resName)
        {
            //加载AB包
            LoadAB(abName);
            //加载资源时，判断一下是否是GameObject
            //如果是直接实例化再返回给外部
            Object obj = abDic[abName].LoadAsset(resName);
            if (obj is GameObject)
                return Instantiate(obj);
            else
                return obj;
        }

        //同步加载 指定type类型
        public Object LoadRes<T>(string abName, string resName)
        {
            //加载AB包
            LoadAB(abName);
            //加载资源时，判断一下是否是GameObject
            //如果是直接实例化再返回给外部
            Object obj = abDic[abName].LoadAsset(resName);
            if (obj is GameObject)
                return Instantiate(obj);
            else
                return obj;

        }
    }
}