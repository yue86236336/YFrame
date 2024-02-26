using UnityEngine;
using System.IO;
using LitJson;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YF
{
    public class LocalConfig : MonoBehaviour
    {

        public static char[] keyChars = { 'w', 'a', 'c', 'd', 'e' };

        //加密方法
        public static string Encrypt(string data)
        {
            char[] dataChars = data.ToCharArray();
            for (int i = 0; i < dataChars.Length; i++)
            {
                char dataChar = dataChars[i];
                char keyChar = keyChars[i % keyChars.Length];
                //重点：通过异或获取新的字符
                char newChar = (char)(dataChar ^ keyChar);
                dataChars[i] = newChar;
            }
            Debug.Log(dataChars.ToString());
            return dataChars.ToString();
        }

        //解密方法
        public static string Decrypt(string data)
        {
            return Encrypt(data);
        }
        public static Dictionary<string, UserData> allUserDatas = new Dictionary<string, UserData>();
        //保存用户数据文本
        public static void SaveUserData(UserData userData)
        {
            //在PersistentDataPath下创建users文件夹
            if (!File.Exists(Application.persistentDataPath + "/users"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/users");
            }
            //保存缓存数据，防止重复加载浪费性能
            allUserDatas[userData.name] = userData;
            //转换用户数据为字符串
            //string jsonData = JsonMapper.ToJson(userData);
            string jsonData = JsonConvert.SerializeObject(userData);
            //加密
            jsonData = Encrypt(jsonData);
            //将加密后的字符串写入文件(文件名为userData.name)
            File.WriteAllText(Application.persistentDataPath + string.Format("/users/{0}.json", userData.name), jsonData);
        }

        //读取用户数据到内存
        public static UserData LoadUserData(string userName)
        {
            //率先从字典中检测是否存在数据
            if (allUserDatas.ContainsKey(userName))
                return allUserDatas[userName];
            string path = Application.persistentDataPath + string.Format("/users/{0}.json", userName);
            //检查用户配置文件是否存在
            if (File.Exists(path))
            {
                //从文本中加载加密字符串
                string jsonData = File.ReadAllText(path);
                //解密
                jsonData = Decrypt(jsonData);
                //将Json字符串转换为用户内存数据
                //UserData userData = JsonMapper.ToObject<UserData>(jsonData);
                UserData userData = JsonConvert.DeserializeObject<UserData>(jsonData);
                return userData;
            }
            else
            {
                Debug.Log("文件不存在");
                return null;
            }

        }
    }
    public class UserData
    {
        public string name;
        public int level;
        public Vector3 pos;
    }
}

