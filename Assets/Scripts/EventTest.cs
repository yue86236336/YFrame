using UnityEngine;
using YF;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //注册事件 key值为1
        MessageManager.Instance.Subscribe<string, int>(1, TestFunc);
        MessageManager.Instance.Subscribe<string, int>(1, TestFunc2);

        //调用key为1的事件 传入参数 1 和 “abc”
        MessageManager.Instance.Publish(1, "今天星期三！！@@", 1513);

        MessageManager.Instance.Unsubscribe<string, int>(1, TestFunc);

        MessageManager.Instance.Publish(1, "mingt星期四！！@@", 1514);
        MessageManager.Instance.UnsubscribeAll(1);
        MessageManager.Instance.Publish(1, "mingt星期四！！@@", 1514);
    }
    private void TestFunc(string value, int num)
    {
        Debug.Log(value);
        Debug.Log(num);
    }

    private void TestFunc2(string str, int num)
    {
        Debug.LogFormat("++{0}++{1}", num, str);

    }


}

