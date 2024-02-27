using UnityEngine;
using YF;

public class EventTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //注册事件 key值为1
        YF.EventCenter.Instance.Subscribe<string, int>(1, TestFunc);
        YF.EventCenter.Instance.Subscribe<string, int>(1, TestFunc2);

        //调用key为1的事件 传入参数 1 和 “abc”
        YF.EventCenter.Instance.Publish(1, "今天星期三！！@@", 1513);

        YF.EventCenter.Instance.Unsubscribe<string, int>(1, TestFunc);

        YF.EventCenter.Instance.Publish(1, "mingt星期四！！@@", 1514);
        YF.EventCenter.Instance.UnsubscribeAll(1);
        YF.EventCenter.Instance.Publish(1, "mingt星期四！！@@", 1514);
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

